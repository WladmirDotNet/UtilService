using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using UtilService.Util.Model;

namespace UtilService.Util;

/// <summary>
/// Service for validating PIX JWT tokens with signature verification
/// Performs complete JWT validation including public key verification
/// </summary>
internal abstract class PixJwtValidattionService
{
    private static readonly HttpClient HttpClient = new();
    private static readonly JwtSecurityTokenHandler TokenHandler = new();

    /// <summary>
    /// Extracts public key URL from JWT token
    /// </summary>
    internal static PublicKeyUrlModel ExtractPublicKeyUrl(JwtSecurityToken token)
    {
        try
        {
            var possibleClaims = new[] { "jku", "jwks_uri", "x5u", "key_url", "public_key_url" };

            foreach (var claimName in possibleClaims)
            {
                var claim = token.Claims.FirstOrDefault(c => c.Type.Equals(claimName, StringComparison.OrdinalIgnoreCase));
                if (claim != null && !string.IsNullOrWhiteSpace(claim.Value))
                {
                    return PublicKeyUrlModel.Success(claim.Value);
                }
            }

            if (token.Header.TryGetValue("jku", out var jkuValue) && jkuValue != null)
            {
                return PublicKeyUrlModel.Success(jkuValue.ToString());
            }

            if (token.Header.TryGetValue("x5u", out var x5UValue) && x5UValue != null)
            {
                return PublicKeyUrlModel.Success(x5UValue.ToString());
            }

            var issuer = token.Payload.Iss;
            if (!string.IsNullOrWhiteSpace(issuer))
            {
                var wellKnownUrl = $"{issuer.TrimEnd('/')}/.well-known/jwks.json";
                return PublicKeyUrlModel.Success(wellKnownUrl);
            }

            return PublicKeyUrlModel.Failure("No public key URL found in JWT token");
        }
        catch (Exception ex)
        {
            return PublicKeyUrlModel.Failure($"Error extracting public key URL: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Fetches JWT token from the specified URL
    /// </summary>
    internal static async Task<string> FetchJwtTokenAsync(string url)
    {
        try
        {
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "PaymentService/1.0");
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");

            var response = await HttpClient.GetAsync("https://"+url);

            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException($"HTTP {response.StatusCode}: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Empty response from server");
            }

            string jwtToken = content;

            if (content.TrimStart().StartsWith("{"))
            {
                try
                {
                    var jsonDoc = JsonDocument.Parse(content);
                    var root = jsonDoc.RootElement;

                    var jwtFields = new[] { "token", "jwt", "access_token", "id_token", "data", "payload" };

                    foreach (var field in jwtFields)
                    {
                        if (root.TryGetProperty(field, out var tokenElement))
                        {
                            jwtToken = tokenElement.GetString() ?? content;
                            break;
                        }
                    }
                }
                catch
                {
                    throw new ArgumentException("Incorrect JSON format");                    
                }
            }

            return jwtToken;
        }
        catch (HttpRequestException ex)
        {
            throw new ArgumentException($"HTTP request failed: {ex.Message}");
        }
        catch (TaskCanceledException ex)
        {
            throw new ArgumentException($"Request timeout: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Unexpected error: {ex.Message}");
        }
    }    

    /// <summary>
    /// Decodes JWT token and extracts header and payload
    /// </summary>
    internal static JwtDecodeModel DecodeJwtToken(string jwtToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(jwtToken))
            {
                return JwtDecodeModel.Failure("JWT token is null or empty");
            }

            var parts = jwtToken.Split('.');
            if (parts.Length != 3)
            {
                return JwtDecodeModel.Failure($"Invalid JWT format. Expected 3 parts, got {parts.Length}");
            }

            var token = TokenHandler.ReadJwtToken(jwtToken);

            var header = new JwtHeaderInfoModel
            {
                Alg = token.Header.Alg,
                Typ = token.Header.Typ,
                Kid = token.Header.Kid
            };

            var payload = new JwtPayloadInfoModel
            {
                Iss = token.Payload.Iss,
                Aud = token.Payload.Aud?.FirstOrDefault(),
                Sub = token.Payload.Sub,
                Claims = token.Payload.Claims.ToDictionary(c => c.Type, c => c.Value)
            };

            return JwtDecodeModel.Success(token, header, payload);
        }
        catch (ArgumentException ex)
        {
            return JwtDecodeModel.Failure($"Invalid JWT format: {ex.Message}");
        }
        catch (Exception ex)
        {
            return JwtDecodeModel.Failure($"JWT decode error: {ex.Message}");
        }
    }

    /// <summary>
    /// Fetches public key from the specified URL
    /// </summary>
    internal static async Task<PublicKeyFetchModel> FetchPublicKeyAsync(string jwksUrl, string kid)
    {
        try
        {
            var response = await HttpClient.GetAsync(jwksUrl);

            if (!response.IsSuccessStatusCode)
            {
                return PublicKeyFetchModel.Failure($"Failed to fetch JWKS: HTTP {response.StatusCode}");
            }

            var jwksContent = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(jwksContent))
            {
                return PublicKeyFetchModel.Failure("JWKS response is empty");
            }

            using var jwksDoc = JsonDocument.Parse(jwksContent);

            if (!jwksDoc.RootElement.TryGetProperty("keys", out var keysElement))
            {
                return PublicKeyFetchModel.Failure("JWKS does not contain 'keys' property");
            }

            foreach (var keyElement in keysElement.EnumerateArray())
            {
                if (keyElement.TryGetProperty("kid", out var keyIdElement))
                {
                    var keyId = keyIdElement.GetString();

                    if (keyId == kid)
                    {
                        if (keyElement.TryGetProperty("n", out var nElement) &&
                            keyElement.TryGetProperty("e", out var eElement))
                        {
                            var jwkKey = JsonSerializer.Serialize(keyElement);
                            return PublicKeyFetchModel.Success(jwkKey, nElement.GetString() ?? string.Empty, eElement.GetString() ?? string.Empty);
                        }
                        else
                        {
                            return PublicKeyFetchModel.Failure($"Key with kid '{kid}' does not contain required RSA components (n, e)");
                        }
                    }
                }
            }

            return PublicKeyFetchModel.Failure($"No key found with kid '{kid}' in JWKS");
        }
        catch (Exception ex)
        {
            return PublicKeyFetchModel.Failure($"Error fetching public key from JWKS: {ex.Message}");
        }
    }

    /// <summary>
    /// Validates JWT signature using the provided public key
    /// </summary>
    internal static SignatureValidationModel ValidateJwtSignature(string jwtToken, string publicKeyData)
    {
        try
        {
            SecurityKey securityKey;

            var keyJson = JsonDocument.Parse(publicKeyData);
                
            var root = keyJson.RootElement;

            if (root.TryGetProperty("kty", out var ktyElement))
            {
                var kty = ktyElement.GetString();

                if (kty == "RSA")
                {
                    securityKey = CreateRsaSecurityKey(root);
                }
                else
                {
                    return SignatureValidationModel.Failure($"Unsupported key type: {kty}");
                }
            }
            else
            {
                return SignatureValidationModel.Failure("Invalid public key format");
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false, 
                ValidateAudience = false, 
                ValidateLifetime = false, 
                ClockSkew = TimeSpan.Zero
            };

            TokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var validatedToken);

            return SignatureValidationModel.Success();
        }
        catch (SecurityTokenValidationException ex)
        {
            return SignatureValidationModel.Failure($"Token validation failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return SignatureValidationModel.Failure($"Signature validation error: {ex.Message}");
        }
    }

    /// <summary>
    /// Validates if the PIX key in JWT payload matches our expected key
    /// </summary>
    /// <param name="payload">JWT payload containing the PIX key</param>
    /// <param name="pixKey"></param>
    /// <returns>Validation result</returns>
    internal static PixKeyValidationModel ValidatePixKey(JwtPayload payload,string pixKey)
    {
        try
        {
            if (payload == null)
            {
                return PixKeyValidationModel.Failure("JWT payload is null");
            }

            if (!payload.TryGetValue("chave", out var chaveValue))
            {
                return PixKeyValidationModel.Failure("PIX key 'chave' field not found in JWT payload");
            }

            var pixKeyFromToken = chaveValue?.ToString();

            if (string.IsNullOrWhiteSpace(pixKeyFromToken))
            {
                return PixKeyValidationModel.Failure("PIX key 'chave' field is empty or null");
            }

            if (!pixKeyFromToken.Equals(pixKey, StringComparison.OrdinalIgnoreCase))
            {
                return PixKeyValidationModel.Failure($"PIX key mismatch. Expected: {pixKey}, Found: {pixKeyFromToken}");
            }
            else
            {
                return PixKeyValidationModel.Success($"Pix key {pixKey} found");
            }
        }
        catch (Exception ex)
        {
            return PixKeyValidationModel.Failure($"Error validating PIX key: {ex.Message}");
        }
    }    

    /// <summary>
    /// Creates RSA security key from JWK format
    /// </summary>
    private static SecurityKey CreateRsaSecurityKey(JsonElement keyElement)
    {
        var n = keyElement.GetProperty("n").GetString();
        
        var e = keyElement.GetProperty("e").GetString();

        var rsa = RSA.Create();
        
        rsa.ImportParameters(new RSAParameters
        {
            Modulus = Base64UrlEncoder.DecodeBytes(n),
            Exponent = Base64UrlEncoder.DecodeBytes(e)
        });

        return new RsaSecurityKey(rsa);
    }
}
