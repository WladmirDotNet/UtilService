namespace UtilService.Util.Model;

internal class SignatureValidationModel
{
	public bool IsValid { get; set; }
	public string ErrorMessage { get; set; } = string.Empty;

	public static SignatureValidationModel Success() => new() { IsValid = true };
	public static SignatureValidationModel Failure(string error) => new() { IsValid = false, ErrorMessage = error };
}
