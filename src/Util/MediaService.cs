using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UtilService.Util.ApplicationEnum;

namespace UtilService.Util;

/// <summary>
/// Helper methods for media and content type operations
/// </summary>
public static class MediaService
{
    /// <summary>
    /// Gets the ContentType enum value from a file name based on its extension
    /// </summary>
    /// <param name="fileName">The file name to analyze</param>
    /// <returns>The corresponding ContentType enum value</returns>
    /// <exception cref="ArgumentException">Thrown when the file extension is not supported</exception>
    public static ContentType GetContentTypeFromFileName(this string fileName)
    {
        var extension = Path.GetExtension(fileName)?.ToLower();
        
        return extension switch
        {
            ".jpg" or ".jpeg" => ContentType.ImageJpeg,
            ".png" => ContentType.ImagePng,
            ".gif" => ContentType.ImageGif,
            ".webp" => ContentType.ImageWebp,
            ".bmp" => ContentType.ImageBmp,
            ".tiff" or ".tif" => ContentType.ImageTiff,
            ".svg" => ContentType.ImageSvg,
            ".mp4" => ContentType.VideoMp4,
            ".webm" => ContentType.VideoWebm,
            ".mov" => ContentType.VideoQuicktime,
            ".avi" => ContentType.VideoAvi,
            ".mpeg" or ".mpg" => ContentType.VideoMpeg,
            ".ts" => ContentType.VideoMp2T,
            ".wmv" => ContentType.VideoXMsWmv,
            ".flv" => ContentType.VideoXFlv,
            ".f4v" => ContentType.VideoF4V,
            ".3gp" => ContentType.Video3Gpp,
            ".3g2" => ContentType.Video3Gpp2,
            ".mkv" => ContentType.VideoXMatroska,
            ".ogv" => ContentType.VideoOgg,
            ".m4v" => ContentType.VideoXm4V,
            _ => throw new ArgumentException($"Unsupported file extension: {extension}")
        };
    }
    
    /// <summary>
    /// Gets the MIME type string from a ContentType enum value
    /// </summary>
    /// <param name="contentType">The ContentType enum value</param>
    /// <returns>The MIME type string</returns>
    public static string GetMimeTypeString(this ContentType contentType)
    {
        var field = contentType.GetType().GetField(contentType.ToString());
        var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
        
        return attribute?.Description ?? throw new ArgumentException($"No description found for ContentType: {contentType}");
    }
    
    /// <summary>
    /// Checks if a file name has a valid image extension
    /// </summary>
    /// <param name="fileName">The file name to validate</param>
    /// <returns>True if the file has a valid image extension, false otherwise</returns>
    public static bool IsValidImageExtension(this string fileName)
    {
        try
        {
            return GetAllowedContentTypes(MediaType.Image).Contains(fileName.GetContentTypeFromFileName());
        }
        catch
        {
            return false;
        }
    }
    
    /// <summary>
    /// Checks if a file name has a valid video extension
    /// </summary>
    /// <param name="fileName">The file name to validate</param>
    /// <returns>True if the file has a valid video extension, false otherwise</returns>
    public static bool IsValidVideoExtension(this string fileName)
    {
        try
        {
            return GetAllowedContentTypes(MediaType.Video).Contains(fileName.GetContentTypeFromFileName());
        }
        catch
        {
            return false;
        }
    }
    
    /// <summary>
    /// Gets allowed content types based on media type
    /// </summary>
    /// <param name="mediaType">The media type filter (null returns all types)</param>
    /// <returns>Array of allowed ContentType values</returns>
    public static ContentType[] GetAllowedContentTypes(MediaType? mediaType = null)
    {
        var allContentTypes = Enum.GetValues<ContentType>();

        if (!mediaType.HasValue)
            return allContentTypes;

        return mediaType.Value switch
        {
            MediaType.Image => allContentTypes.Where(ct => IsImageContentType(ct)).ToArray(),
            MediaType.Video => allContentTypes.Where(ct => IsVideoContentType(ct)).ToArray(),
            _ => throw new ArgumentException($"Unsupported MediaType: {mediaType}")
        };
    }
    
    /// <summary>
    /// Gets allowed image content types 
    /// </summary>
    /// <returns>Array of allowed Image ContentType values</returns>
    public static ContentType[] GetAllowedImageContentTypes()
    {
        return GetAllowedContentTypes(MediaType.Image);
    } 
    
    /// <summary>
    /// Gets allowed video content types 
    /// </summary>
    /// <returns>Array of allowed Video ContentType values</returns>
    public static ContentType[] GetAllowedVideoContentTypes()
    {
        return GetAllowedContentTypes(MediaType.Video);
    } 
    
    #region Private Methods

    /// <summary>
    /// Checks if a ContentType is an image type based on MediaType.Image description
    /// </summary>
    /// <param name="contentType">The ContentType to check</param>
    /// <returns>True if it's an image type, false otherwise</returns>
    private static bool IsImageContentType(ContentType contentType)
    {
        return contentType.ToString().StartsWith(MediaType.Image.GetDescription(), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if a ContentType is a video type based on MediaType.Video description
    /// </summary>
    /// <param name="contentType">The ContentType to check</param>
    /// <returns>True if it's a video type, false otherwise</returns>
    private static bool IsVideoContentType(ContentType contentType)
    {
        return contentType.ToString().StartsWith(MediaType.Video.GetDescription(), StringComparison.OrdinalIgnoreCase);
    }
    
    #endregion Private Methods
}
