using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilService.Util.Model;

namespace UtilService.Util;

/// <summary>
/// Service class for PIX (Brazilian instant payment system) operations and validations
/// </summary>
public static partial class PixService
{
    /// <summary>
    /// Generates a PNG image stream containing a QR code for the specified EMV code
    /// </summary>
    /// <param name="emvCode">The EMV code to be encoded in the QR code</param>
    /// <param name="validationRequirement">Optional validation requirements for PIX operations</param>
    /// <returns>MemoryStream containing the QR code PNG image</returns>
    public static async Task<MemoryStream> GeneratePixQrCodePngImageStream(string emvCode, PixValidationRequirementModel validationRequirement = null)
    {
        await ValidatePixEmvCode(emvCode,validationRequirement);
        
        return emvCode.GenerateQrCodePngImageStream();
    }

    /// <summary>
    /// Generates a Base64 string (PNG) for the QR code representing the specified EMV code
    /// </summary>
    /// <param name="emvCode">The EMV code to be encoded in the QR code</param>
    /// <param name="validationRequirement">Optional validation requirements for PIX operations</param>
    /// <returns>Base64 string of the PNG QR code image (without data URL prefix)</returns>
    public static async Task<string> GeneratePixQrCodeImageBase64(string emvCode, PixValidationRequirementModel validationRequirement = null)
    {
        using var imageStream = await GeneratePixQrCodePngImageStream(emvCode, validationRequirement);
        var bytes = imageStream.ToArray();
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Validates the EMV code format and content
    /// </summary>
    /// <param name="pixQrCodePngImageStream"></param>
    /// <param name="validationRequirement"></param>
    /// <exception cref="ArgumentException">Thrown when EMV code is invalid</exception>
    public static async Task ValidatePixQrCodePngImageStream(MemoryStream pixQrCodePngImageStream, PixValidationRequirementModel validationRequirement = null)
    {
        if(pixQrCodePngImageStream==null)
            throw new ArgumentException("Image memory stream is missing or empty");
            
        var qrCodeStrings = pixQrCodePngImageStream.ReadQrCodesFromImage();
        if (qrCodeStrings.Any())
        {
            await ValidatePixEmvCode(qrCodeStrings.FirstOrDefault(),validationRequirement);            
        }
        else
        {
            throw new ArgumentException("QR Code not found.");
        }
    }

    /// <summary>
    /// Validates the EMV code format and content
    /// </summary>
    /// <param name="emvCode">The EMV code to validate</param>
    /// <param name="validationRequirement"></param>
    /// <exception cref="ArgumentException">Thrown when EMV code is invalid</exception>
    public static async Task ValidatePixEmvCode(string emvCode, PixValidationRequirementModel validationRequirement = null)
    {
        if (string.IsNullOrWhiteSpace(emvCode))
        {
            throw new ArgumentException("EMV code is missing or empty", nameof(emvCode));
        }

        if (emvCode.Length < 50)
        {
            throw new ArgumentException($"EMV code too short ({emvCode.Length} characters). Expected at least 50 characters", nameof(emvCode));
        }

        if (emvCode.Length > 512)
        {
            throw new ArgumentException($"EMV code too long ({emvCode.Length} characters). Expected maximum 512 characters", nameof(emvCode));
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(emvCode, @"^[0-9A-Za-z\-\.\+\*\/@\s]*$"))
        {
            throw new ArgumentException("EMV code contains invalid characters. Only alphanumeric and basic symbols are allowed", nameof(emvCode));
        }
        
        var validations = new List<(string pattern, string description)>
        {
            (@"^00\d{2}", "Payload Format Indicator (00)"),
            (@"01\d{2}", "Point of Initiation Method (01)"),
            (@"26\d{2}", "Merchant Account Information (26)"),
            (@"52\d{2}", "Merchant Category Code (52)"),
            (@"53\d{2}", "Transaction Currency (53)"),
            (@"58\d{2}", "Country Code (58)"),
            (@"59\d{2}", "Merchant Name (59)"),
            (@"60\d{2}", "Merchant City (60)"),
            (@"62\d{2}", "Additional Data Field (62)"),
            (@"63\d{2}", "CRC (63)")
        };

        var missingCriticalFields = new List<string>();

        foreach (var (pattern, description) in validations)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(emvCode, pattern))
            {
                if (pattern.StartsWith("00") || pattern.StartsWith("52") ||
                    pattern.StartsWith("53") || pattern.StartsWith("58") ||
                    pattern.StartsWith("63"))
                {
                    missingCriticalFields.Add(description);
                }
            }
        }

        if (missingCriticalFields.Count > 0)
        {
            throw new ArgumentException($"Missing critical EMV fields: {string.Join(", ", missingCriticalFields)}");
        }        
        
        ValidatePixEmvCrc(emvCode);

        if (validationRequirement != null)
        {
            if (!string.IsNullOrWhiteSpace(validationRequirement.JwtPublicKeyDomain) || !string.IsNullOrWhiteSpace(validationRequirement.PixKey))
            {
                var pixUrlInfo = ValidatePixDomainInEmv(emvCode, validationRequirement);
                
                if (pixUrlInfo.Result != null)
                {
                    var decodeResult = PixJwtValidattionService.DecodeJwtToken(pixUrlInfo.Result.JwtToken);
                    
                    if (!decodeResult.IsSuccess)
                    {
                        throw new ArgumentException($"JWT decode failed: {decodeResult.ErrorMessage}");
                    }

                    if (decodeResult.Token != null)
                    {
                        var pixKeyValidation = PixJwtValidattionService.ValidatePixKey(decodeResult.Token.Payload,validationRequirement.PixKey);
                        
                        if (!pixKeyValidation.IsValid)
                        {
                            throw new ArgumentException($"PIX key validation failed: {pixKeyValidation.ErrorMessage}");                            
                        }
                        
                        var publicKeyUrlResult = PixJwtValidattionService.ExtractPublicKeyUrl(decodeResult.Token);
                        
                        if (!publicKeyUrlResult.IsSuccess)
                        {
                            throw new ArgumentException($"Public key URL extraction failed: {publicKeyUrlResult.ErrorMessage}");                            
                        }

                        if (!string.IsNullOrWhiteSpace(validationRequirement.JwtPublicKeyDomain))
                        {
                            var rawUrl = publicKeyUrlResult.Url ?? string.Empty;
                            var urlToParse = rawUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? rawUrl : $"https://{rawUrl}";
                            try
                            {
                                var uri = new Uri(urlToParse);
                                var actualHost = uri.Host.ToLowerInvariant();
                                var expectedDomain = validationRequirement.JwtPublicKeyDomain.ToLowerInvariant();

                                var matches = actualHost.Equals(expectedDomain) || actualHost.EndsWith($".{expectedDomain}");
                                if (!matches)
                                {
                                    throw new ArgumentException($"Public key URL domain mismatch. Expected '{expectedDomain}', got '{actualHost}'. URL: '{rawUrl}'");
                                }
                            }
                            catch (UriFormatException)
                            {
                                throw new ArgumentException($"Invalid public key URL format: '{rawUrl}'");
                            }
                        }

                        var kid = decodeResult.Header.Kid ?? "";

                        var publicKeyResult = await PixJwtValidattionService.FetchPublicKeyAsync(publicKeyUrlResult.Url, kid);
                        
                        if (!publicKeyResult.IsSuccess)
                        {
                            throw new ArgumentException($"Public key fetch failed: {publicKeyResult.ErrorMessage}");
                        }                      

                        var signatureValidationResult = PixJwtValidattionService.ValidateJwtSignature(pixUrlInfo.Result.JwtToken, publicKeyResult.PublicKey);
                        
                        if (!signatureValidationResult.IsValid)
                        {
                            throw new ArgumentException($"JWT signature validation failed: {signatureValidationResult.ErrorMessage}");
                        }                        
                    }
                    else
                    {
                        throw new ArgumentException($"JWT decode failed");
                    }
                }                
            }
        }
    }    

    #region Private Methods

    /// <summary>
    /// Validates that the PIX domain is present in the correct EMV field position and returns URL info with JWT token
    /// </summary>
    /// <param name="emvCode">The EMV code to validate</param>
    /// <param name="validationRequirement">Optional validation requirements containing expected domain</param>
    /// <returns>PixUrlInfo containing the complete URL and extracted JWT token</returns>
    /// <exception cref="ArgumentException">Thrown when PIX domain validation fails</exception>
    private static async Task<PixUrlInfoModel> ValidatePixDomainInEmv(string emvCode, PixValidationRequirementModel validationRequirement = null)
    {
        var field26Match = System.Text.RegularExpressions.Regex.Match(emvCode, @"26(\d{2})(.+?)(?=52\d{2}|$)");
        
        if (!field26Match.Success)
        {
            throw new ArgumentException("Field 26 (Merchant Account Information) not found in EMV code");
        }

        var field26Length = int.Parse(field26Match.Groups[1].Value);
        
        var field26Content = field26Match.Groups[2].Value;

        if (field26Content.Length != field26Length)
        {
            throw new ArgumentException($"Field 26 length mismatch: expected {field26Length}, got {field26Content.Length}");
        }

        if (!field26Content.Contains("0014br.gov.bcb.pix"))
        {
            throw new ArgumentException("PIX identifier (br.gov.bcb.pix) not found in field 26 - may not be a valid PIX code");
        }

        var field25Match = System.Text.RegularExpressions.Regex.Match(field26Content, @"25(\d{2})(.+)$");
        
        if (!field25Match.Success)
        {
            throw new ArgumentException("PIX URL (field 25) not found in Merchant Account Information");
        }

        var urlLength = int.Parse(field25Match.Groups[1].Value);
        
        var completeUrl = field25Match.Groups[2].Value;

        if (completeUrl.Length < urlLength)
        {
            throw new ArgumentException($"PIX URL length mismatch: expected {urlLength}, but available content is {completeUrl.Length}");
        }

        var extractedUrl = completeUrl.Substring(0, urlLength);

        if (validationRequirement?.JwtPublicKeyDomain != null)
        {
            if (!extractedUrl.Contains(validationRequirement.JwtPublicKeyDomain))
            {
                throw new ArgumentException($"PIX URL '{extractedUrl}' does not contain expected domain '{validationRequirement.JwtPublicKeyDomain}'");
            }
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(extractedUrl, @"^[a-zA-Z0-9\-\.\/]+$"))
        {
            throw new ArgumentException($"PIX URL '{extractedUrl}' contains invalid characters");
        }

        var jwtFetchResult = await PixJwtValidattionService.FetchJwtTokenAsync(extractedUrl);
        if (string.IsNullOrEmpty(jwtFetchResult))
        {
            throw new ArgumentException($"JWT fetch failed");            
        }
        
        return new PixUrlInfoModel
        {
            Url = extractedUrl,
            JwtToken = jwtFetchResult
        };
    }
    
    /// <summary>
    /// Validates the CRC-16 CCITT checksum of a PIX EMV code
    /// </summary>
    /// <param name="emvCode">The complete EMV code including CRC</param>
    /// <exception cref="ArgumentException">Thrown when CRC validation fails</exception>
    private static void ValidatePixEmvCrc(string emvCode)
    {
        if (string.IsNullOrWhiteSpace(emvCode))
        {
            throw new ArgumentException("EMV code is null or empty", nameof(emvCode));
        }

        var crcMatch = System.Text.RegularExpressions.Regex.Match(emvCode, @"63\d{2}([A-F0-9]{4})$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (!crcMatch.Success)
        {
            throw new ArgumentException("CRC field (63XX) not found at the end of EMV code", nameof(emvCode));            
        }

        var providedCrcHex = crcMatch.Groups[1].Value.ToUpperInvariant();
        
        var payload = emvCode.Substring(0, emvCode.Length - 4);

        try
        {
            var calculatedCrc = CalculateCrc16CcittFalse(payload);
            
            var calculatedCrcHex = calculatedCrc.ToString("X4");

            if (!providedCrcHex.Equals(calculatedCrcHex, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"CRC mismatch - Provided: {providedCrcHex}, Calculated: {calculatedCrcHex}. Payload: '{payload}'", nameof(emvCode));
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Error calculating CRC: {ex.Message}", nameof(emvCode), ex);
        }
    }    
    
    /// <summary>
    /// CRC-16 CCITT-FALSE: The CONFIRMED algorithm used by Cora PIX system
    /// Poly=0x1021, Init=0xFFFF, RefIn=false, RefOut=false, XorOut=0x0000
    /// This is the exact variant that matches Cora's EMV QR Code CRC calculation
    /// </summary>
    private static ushort CalculateCrc16CcittFalse(string data)
    {
        const ushort polynomial = 0x1021;
        
        ushort crc = 0xFFFF;
        
        var bytes = Encoding.ASCII.GetBytes(data);

        foreach (var b in bytes)
        {
            crc ^= (ushort)(b << 8);
            for (int i = 0; i < 8; i++)
            {
                if ((crc & 0x8000) != 0)
                    crc = (ushort)(((crc << 1) & 0xFFFF) ^ polynomial);
                else
                    crc = (ushort)((crc << 1) & 0xFFFF);
            }
        }
        return crc;
    }    

    #endregion Private Methods
}
