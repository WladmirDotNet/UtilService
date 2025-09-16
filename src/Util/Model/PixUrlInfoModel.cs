namespace UtilService.Util.Model;

/// <summary>
/// Contains PIX URL information extracted from EMV code
/// </summary>
public class PixUrlInfoModel
{
    /// <summary>
    /// Gets or sets the complete PIX URL
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the JWT token extracted from the URL
    /// </summary>
    public string JwtToken { get; set; }
}
