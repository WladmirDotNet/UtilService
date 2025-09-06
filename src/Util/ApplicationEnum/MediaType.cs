using System.ComponentModel;

namespace UtilService.Util.ApplicationEnum;

/// <summary>
/// Media types for content classification
/// </summary>
public enum MediaType
{
    /// <summary>
    /// Image media type
    /// </summary>
    [Description("Image")]
    Image = 1,
    
    /// <summary>
    /// Video media type
    /// </summary>
    [Description("Video")]
    Video = 2,
    
    /// <summary>
    /// Audio media type
    /// </summary>
    [Description("Audio")]
    Audio = 3    
    
}
