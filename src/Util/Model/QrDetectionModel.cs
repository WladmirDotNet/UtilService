using ZXing;

namespace UtilService.Util.Model;

/// <summary>
/// QR Code detection result with text and position points
/// </summary>
public class QrDetectionModel
{
    /// <summary>
    /// Text content of the detected QR Code
    /// </summary>
    public string Text { get; set; } = string.Empty;
    
    /// <summary>
    /// Corner points that define the QR Code position in the image
    /// </summary>
    public ResultPoint[] CornerPoints { get; set; } = [];
}
