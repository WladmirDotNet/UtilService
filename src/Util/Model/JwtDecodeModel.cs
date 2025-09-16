using System.IdentityModel.Tokens.Jwt;

namespace UtilService.Util.Model;

internal class JwtDecodeModel
{
	public bool IsSuccess { get; set; }
	public JwtSecurityToken Token { get; set; }
	public JwtHeaderInfoModel Header { get; set; } = new();
	public JwtPayloadInfoModel Payload { get; set; } = new();
	public string ErrorMessage { get; set; } = string.Empty;

	public static JwtDecodeModel Success(JwtSecurityToken token, JwtHeaderInfoModel header, JwtPayloadInfoModel payload) => new()
	{
		IsSuccess = true, 
		Token = token, 
		Header = header, 
		Payload = payload
	};
	public static JwtDecodeModel Failure(string error) => new() { IsSuccess = false, ErrorMessage = error };
}
