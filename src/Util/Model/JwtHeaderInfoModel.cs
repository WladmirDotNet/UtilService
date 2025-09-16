namespace UtilService.Util.Model;

/// <summary>
/// JWT header information
/// </summary>
internal class JwtHeaderInfoModel
{
	/// <summary>
	/// Algorithm used to sign the JWT
	/// </summary>
	public string Alg { get; set; } = string.Empty;

	/// <summary>
	/// Token type (usually "JWT")
	/// </summary>
	public string Typ { get; set; } = string.Empty;

	/// <summary>
	/// Key ID used to sign the JWT
	/// </summary>
	public string Kid { get; set; } = string.Empty;
}
