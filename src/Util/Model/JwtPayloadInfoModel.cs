using System;
using System.Collections.Generic;

namespace UtilService.Util.Model;

/// <summary>
/// JWT payload information
/// </summary>
internal class JwtPayloadInfoModel
{
	/// <summary>
	/// Issuer of the JWT
	/// </summary>
	public string Iss { get; set; } = string.Empty;

	/// <summary>
	/// Audience of the JWT
	/// </summary>
	public string Aud { get; set; } = string.Empty;

	/// <summary>
	/// Subject of the JWT
	/// </summary>
	public string Sub { get; set; } = string.Empty;

	/// <summary>
	/// Expiration time (Unix timestamp)
	/// </summary>
	public long? Exp { get; set; }

	/// <summary>
	/// Issued at time (Unix timestamp)
	/// </summary>
	public long? Iat { get; set; }

	/// <summary>
	/// Not before time (Unix timestamp)
	/// </summary>
	public long? Nbf { get; set; }

	/// <summary>
	/// All claims in the JWT payload
	/// </summary>
	public Dictionary<string, string> Claims { get; set; } = new();

	/// <summary>
	/// Gets expiration date as DateTimeOffset
	/// </summary>
	public DateTimeOffset? ExpirationDate => Exp.HasValue ? DateTimeOffset.FromUnixTimeSeconds(Exp.Value) : null;

	/// <summary>
	/// Gets issued at date as DateTimeOffset
	/// </summary>
	public DateTimeOffset? IssuedAtDate => Iat.HasValue ? DateTimeOffset.FromUnixTimeSeconds(Iat.Value) : null;

	/// <summary>
	/// Gets not before date as DateTimeOffset
	/// </summary>
	public DateTimeOffset? NotBeforeDate => Nbf.HasValue ? DateTimeOffset.FromUnixTimeSeconds(Nbf.Value) : null;

	/// <summary>
	/// Checks if the JWT is expired
	/// </summary>
	public bool IsExpired => ExpirationDate.HasValue && DateTimeOffset.UtcNow > ExpirationDate.Value;

	/// <summary>
	/// Checks if the JWT is not yet valid
	/// </summary>
	public bool IsNotYetValid => NotBeforeDate.HasValue && DateTimeOffset.UtcNow < NotBeforeDate.Value;
}
