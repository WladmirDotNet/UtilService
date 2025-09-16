using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace UtilService.Util.Model;

/// <summary>
/// Represents the result of a complete PIX JWT validation
/// Including URL extraction, JWT fetch, decode, and signature verification
/// </summary>
internal class JwtValidationModel
{
    /// <summary>
    /// Indicates if the complete JWT validation was successful
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Indicates if the JWT signature is valid
    /// </summary>
    public bool IsSignatureValid { get; set; }

    /// <summary>
    /// The URL extracted from the PIX EMV code
    /// </summary>
    public string ExtractedUrl { get; set; } = string.Empty;

    /// <summary>
    /// The raw JWT token fetched from the URL
    /// </summary>
    public string RawJwtToken { get; set; } = string.Empty;

    /// <summary>
    /// The decoded JWT token object
    /// </summary>
    public JwtSecurityToken DecodedToken { get; set; }

    /// <summary>
    /// JWT header information
    /// </summary>
    public JwtHeaderInfoModel JwtHeader { get; set; } = new();

    /// <summary>
    /// JWT payload information
    /// </summary>
    public JwtPayloadInfoModel JwtPayload { get; set; } = new();

    /// <summary>
    /// The URL where the public key was fetched from
    /// </summary>
    public string PublicKeyUrl { get; set; } = string.Empty;

    /// <summary>
    /// The public key data used for signature verification
    /// </summary>
    public string PublicKey { get; set; } = string.Empty;

    /// <summary>
    /// List of validation errors found
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// List of informational messages
    /// </summary>
    public List<string> Info { get; set; } = new();

    /// <summary>
    /// Indicates if there are any errors
    /// </summary>
    public bool HasErrors => Errors.Count > 0;

    /// <summary>
    /// Indicates if there are any informational messages
    /// </summary>
    public bool HasInfo => Info.Count > 0;

    /// <summary>
    /// Adds an error to the error list
    /// </summary>
    /// <param name="error">Error message</param>
    public void AddError(string error)
    {
        if (!string.IsNullOrWhiteSpace(error))
        {
            Errors.Add(error);
            IsValid = false;
        }
    }

    /// <summary>
    /// Adds an informational message to the result
    /// </summary>
    /// <param name="info">Information message</param>
    public void AddInfo(string info)
    {
        if (!string.IsNullOrWhiteSpace(info))
        {
            Info.Add(info);
        }
    }

    /// <summary>
    /// Gets a summary of the JWT validation result
    /// </summary>
    /// <returns>Summary string</returns>
    private string GetSummary()
    {
        if (IsValid && IsSignatureValid)
        {
            return $"✅ JWT validation successful - Signature verified";
        }
        else if (IsSignatureValid)
        {
            return $"⚠️ JWT signature valid but validation failed - {Errors.Count} error(s)";
        }
        else
        {
            return $"❌ JWT validation failed - {Errors.Count} error(s)";
        }
    }

    /// <summary>
    /// Gets all messages (errors and info) as a single list
    /// </summary>
    /// <returns>Combined list of all messages</returns>
    public List<string> GetAllMessages()
    {
        var messages = new List<string>();
        messages.AddRange(Errors.Select(e => $"ERROR: {e}"));
        messages.AddRange(Info.Select(i => $"INFO: {i}"));
        return messages;
    }

    /// <summary>
    /// Gets detailed validation information
    /// </summary>
    /// <returns>Detailed validation info</returns>
    public string GetDetailedInfo()
    {
        var sb = new System.Text.StringBuilder();
        
        sb.AppendLine($"JWT Validation Result: {GetSummary()}");
        sb.AppendLine($"Extracted URL: {ExtractedUrl}");
        sb.AppendLine($"JWT Token Length: {RawJwtToken.Length} characters");
        sb.AppendLine($"Public Key URL: {PublicKeyUrl}");
        
        if (DecodedToken != null)
        {
            sb.AppendLine($"JWT Algorithm: {JwtHeader.Alg}");
            sb.AppendLine($"JWT Key ID: {JwtHeader.Kid}");
            sb.AppendLine($"JWT Issuer: {JwtPayload.Iss}");
            sb.AppendLine($"JWT Audience: {JwtPayload.Aud}");
            sb.AppendLine($"JWT Subject: {JwtPayload.Sub}");
            
            if (JwtPayload.Exp.HasValue)
            {
                var expDate = DateTimeOffset.FromUnixTimeSeconds(JwtPayload.Exp.Value);
                sb.AppendLine($"JWT Expires: {expDate:yyyy-MM-dd HH:mm:ss} UTC");
            }
        }
        
        if (HasErrors)
        {
            sb.AppendLine("Errors:");
            foreach (var error in Errors)
            {
                sb.AppendLine($"  • {error}");
            }
        }
        
        if (HasInfo)
        {
            sb.AppendLine("Info:");
            foreach (var info in Info)
            {
                sb.AppendLine($"  • {info}");
            }
        }
        
        return sb.ToString();
    }
}

