using System.ComponentModel;

namespace UtilService.Util.ApplicationEnum;

/// <summary>
/// Content types for files
/// </summary>
public enum ContentType
{
    #region Image Content Types (1 - 50 Reserved)
    
    /// <summary>
    /// JPEG image format (image/jpeg)
    /// </summary>
    [Description("image/jpeg")]
    ImageJpeg = 1,
    
    /// <summary>
    /// PNG image format (image/png)
    /// </summary>
    [Description("image/png")]
    ImagePng = 2,
    
    /// <summary>
    /// GIF image format (image/gif)
    /// </summary>
    [Description("image/gif")]
    ImageGif = 3,
    
    /// <summary>
    /// WebP image format (image/webp)
    /// </summary>
    [Description("image/webp")]
    ImageWebp = 4,
    
    /// <summary>
    /// BMP image format (image/bmp)
    /// </summary>
    [Description("image/bmp")]
    ImageBmp = 5,
    
    /// <summary>
    /// TIFF image format (image/tiff)
    /// </summary>
    [Description("image/tiff")]
    ImageTiff = 6,
    
    /// <summary>
    /// SVG image format (image/svg+xml)
    /// </summary>
    [Description("image/svg+xml")]
    ImageSvg = 7,
    
    #endregion
    
    #region Video Content Types (51 - 100 Reserved)
    
    /// <summary>
    /// MP4 video format (video/mp4)
    /// </summary>
    [Description("video/mp4")]
    VideoMp4 = 51,
    
    /// <summary>
    /// MP4V-ES video format (video/mp4v-es)
    /// </summary>
    [Description("video/mp4v-es")]
    VideoMp4VEs = 52,
    
    /// <summary>
    /// MPEG-4 Generic video format (video/mpeg4-generic)
    /// </summary>
    [Description("video/mpeg4-generic")]
    VideoMpeg4Generic = 53,
    
    /// <summary>
    /// WebM video format (video/webm)
    /// </summary>
    [Description("video/webm")]
    VideoWebm = 54,
    
    /// <summary>
    /// QuickTime video format (video/quicktime)
    /// </summary>
    [Description("video/quicktime")]
    VideoQuicktime = 55,
    
    /// <summary>
    /// QuickTime X video format (video/x-quicktime)
    /// </summary>
    [Description("video/x-quicktime")]
    VideoXQuicktime = 56,
    
    /// <summary>
    /// AVI video format (video/avi)
    /// </summary>
    [Description("video/avi")]
    VideoAvi = 57,
    
    /// <summary>
    /// MS Video format (video/msvideo)
    /// </summary>
    [Description("video/msvideo")]
    VideoMsVideo = 58,
    
    /// <summary>
    /// MS Video X format (video/x-msvideo)
    /// </summary>
    [Description("video/x-msvideo")]
    VideoXMsVideo = 59,
    
    /// <summary>
    /// MPEG video format (video/mpeg)
    /// </summary>
    [Description("video/mpeg")]
    VideoMpeg = 60,
    
    /// <summary>
    /// MPEG-2 Transport Stream format (video/mp2t)
    /// </summary>
    [Description("video/mp2t")]
    VideoMp2T = 61,
    
    /// <summary>
    /// H.264 video format (video/h264)
    /// </summary>
    [Description("video/h264")]
    VideoH264 = 62,
    
    /// <summary>
    /// H.265 video format (video/h265)
    /// </summary>
    [Description("video/h265")]
    VideoH265 = 63,
    
    /// <summary>
    /// Windows Media Video format (video/x-ms-wmv)
    /// </summary>
    [Description("video/x-ms-wmv")]
    VideoXMsWmv = 64,
    
    /// <summary>
    /// Advanced Systems Format (video/x-ms-asf)
    /// </summary>
    [Description("video/x-ms-asf")]
    VideoXMsAsf = 65,
    
    /// <summary>
    /// Flash Video format (video/x-flv)
    /// </summary>
    [Description("video/x-flv")]
    VideoXFlv = 66,
    
    /// <summary>
    /// Flash Video F4V format (video/f4v)
    /// </summary>
    [Description("video/f4v")]
    VideoF4V = 67,
    
    /// <summary>
    /// 3GPP video format (video/3gpp)
    /// </summary>
    [Description("video/3gpp")]
    Video3Gpp = 68,
    
    /// <summary>
    /// 3GPP2 video format (video/3gpp2)
    /// </summary>
    [Description("video/3gpp2")]
    Video3Gpp2 = 69,
    
    /// <summary>
    /// Matroska video format (video/x-matroska)
    /// </summary>
    [Description("video/x-matroska")]
    VideoXMatroska = 70,
    
    /// <summary>
    /// Ogg video format (video/ogg)
    /// </summary>
    [Description("video/ogg")]
    VideoOgg = 71,
    
    /// <summary>
    /// iTunes M4V video format (video/x-m4v)
    /// </summary>
    [Description("video/x-m4v")]
    VideoXm4V = 72
    
    #endregion
}
