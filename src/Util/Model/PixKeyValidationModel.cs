namespace UtilService.Util.Model;

/// <summary>
/// Result of PIX key validation
/// </summary>
internal class PixKeyValidationModel
{
	public bool IsValid { get; set; }
	public string Message { get; set; }
	public string ErrorMessage { get; set; }

	public static PixKeyValidationModel Success(string message) => new() { IsValid = true, Message = message };
	public static PixKeyValidationModel Failure(string errorMessage) => new() { IsValid = false, ErrorMessage = errorMessage };
}
