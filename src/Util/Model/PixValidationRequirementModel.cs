namespace UtilService.Util.Model;

/// <summary>
/// Represents the requirements for PIX validation operations
/// </summary>
public class PixValidationRequirementModel
{
    /// <summary>
    /// Gets or sets the JWT public key domain used for PIX validation
    /// </summary>
    public string JwtPublicKeyDomain { get; set; }

    /// <summary>
    /// Gets or sets the PIX key to be validated
    /// </summary>
    public string PixKey { get; set; }
}
