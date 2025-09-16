using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UtilService.Util.Model;

namespace UtilService.Util;

/// <summary>
/// Service class for PIX (Brazilian instant payment system) operations and validations
/// </summary>
public static class PixService
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
                
                if (pixUrlInfo != null)
                {
                    var decodeResult = PixJwtValidattionService.DecodeJwtToken(pixUrlInfo.JwtToken);
                    
                    if (!decodeResult.IsSuccess)
                    {
                        throw new ArgumentException($"JWT decode failed: {decodeResult.ErrorMessage}");
                    }

                    if (decodeResult?.Token != null)
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

                        var kid = decodeResult.Header.Kid ?? "";

                        var publicKeyResult = await PixJwtValidattionService.FetchPublicKeyAsync(publicKeyUrlResult.Url, kid);
                        
                        if (!publicKeyResult.IsSuccess)
                        {
                            throw new ArgumentException($"Public key fetch failed: {publicKeyResult.ErrorMessage}");
                        }                      

                        var signatureValidationResult = PixJwtValidattionService.ValidateJwtSignature(pixUrlInfo.JwtToken, publicKeyResult.PublicKey);
                        
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
    private static PixUrlInfoModel ValidatePixDomainInEmv(string emvCode, PixValidationRequirementModel validationRequirement = null)
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

        var jwtToken = ExtractJwtTokenFromPixUrl(extractedUrl);
        
        return new PixUrlInfoModel
        {
            Url = extractedUrl,
            JwtToken = jwtToken
        };
    }

    /// <summary>
    /// Extracts JWT token from PIX URL
    /// </summary>
    /// <param name="pixUrl">The PIX URL containing the JWT token</param>
    /// <returns>The JWT token extracted from the URL</returns>
    /// <exception cref="ArgumentException">Thrown when JWT token cannot be extracted</exception>
    private static string ExtractJwtTokenFromPixUrl(string pixUrl)
    {
        if (string.IsNullOrWhiteSpace(pixUrl))
        {
            throw new ArgumentException("PIX URL is null or empty", nameof(pixUrl));
        }
        
        var urlParts = pixUrl.Split('/');
        
        if (urlParts.Length == 0)
        {
            throw new ArgumentException("Invalid PIX URL format - no path segments found", nameof(pixUrl));
        }

        var potentialToken = urlParts[urlParts.Length - 1];

        if (string.IsNullOrWhiteSpace(potentialToken))
        {
            throw new ArgumentException("JWT token not found in PIX URL - last path segment is empty", nameof(pixUrl));
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(potentialToken, @"^[a-zA-Z0-9\-_]+$"))
        {
            throw new ArgumentException($"Invalid JWT token format: '{potentialToken}' - should contain only alphanumeric characters, hyphens and underscores", nameof(pixUrl));
        }

        if (potentialToken.Length < 10)
        {
            throw new ArgumentException($"JWT token too short: '{potentialToken}' - expected at least 10 characters", nameof(pixUrl));
        }

        return potentialToken;
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
        
        var crcFieldStart = crcMatch.Index;
        
        var payload = emvCode[..crcFieldStart];

        try
        {
            var calculatedCrc = CalculateCrc16Ccitt(payload);
            
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
    /// Calculates CRC-16 CCITT-FALSE checksum for PIX EMV codes
    /// This is the CONFIRMED variant used by Cora PIX system
    /// Polynomial: 0x1021 (x^16 + x^12 + x^5 + 1)
    /// Initial value: 0xFFFF
    /// RefIn: false, RefOut: false, XorOut: 0x0000
    /// Uses ASCII encoding as per PIX specification
    /// </summary>
    /// <param name="data">Input string data (EMV payload without CRC)</param>
    /// <returns>CRC-16 value</returns>
    private static ushort CalculateCrc16Ccitt(string data)
    {
        const ushort polynomial = 0x1021;
        
        ushort crc = 0xFFFF; 

        var bytes = System.Text.Encoding.ASCII.GetBytes(data);

        foreach (var b in bytes)
        {
            crc ^= (ushort)(b << 8);

            for (int i = 0; i < 8; i++)
            {
                if ((crc & 0x8000) != 0)
                {
                    crc = (ushort)(((crc << 1) & 0xFFFF) ^ polynomial);
                }
                else
                {
                    crc = (ushort)((crc << 1) & 0xFFFF);
                }
            }
        }

        return crc; 
    }        

    #endregion Private Methods
}
