namespace UtilService.Util.Model;

internal class PublicKeyUrlModel
{
	public bool IsSuccess { get; set; }
	public string Url { get; set; } = string.Empty;
	public string ErrorMessage { get; set; } = string.Empty;

	public static PublicKeyUrlModel Success(string url) => new() { IsSuccess = true, Url = url };
	public static PublicKeyUrlModel Failure(string error) => new() { IsSuccess = false, ErrorMessage = error };
}
