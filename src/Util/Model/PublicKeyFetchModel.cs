namespace UtilService.Util.Model;

internal class PublicKeyFetchModel
{
	public bool IsSuccess { get; set; }
	public string PublicKey { get; set; } = string.Empty;
	public string ErrorMessage { get; set; } = string.Empty;
	public string N { get; set; } = string.Empty;
	public string E { get; set; } = string.Empty;

	public static PublicKeyFetchModel Success(string key, string n, string e) => new()
	{
		N = n,
		E = e,
		IsSuccess = true,
		PublicKey = key
	};
	public static PublicKeyFetchModel Failure(string error) => new() { IsSuccess = false, ErrorMessage = error };
}
