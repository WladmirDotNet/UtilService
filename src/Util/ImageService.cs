using System;
using System.Linq;

namespace UtilService.Util;

/// <summary>
/// Cladd for images
/// </summary>
public static class ImageService
{
    /// <summary>
    /// Tag to add prefix data:image/png;base64, em imágens
    /// </summary>
    public const string PrefixBase64Png = "data:image/png;base64,";

    /// <summary>
    /// Tag to add prefix data:image/png;base64, em imágens
    /// </summary>
    public const string PrefixBase64Jpeg = "data:image/jpeg;base64,";

    /// <summary>
    /// Check if Base64 is image
    /// </summary>
    /// <param name="imagemBase64"></param>
    /// <returns></returns>
    public static bool IsImage(this string imagemBase64)
    {
        try
        {
            byte[] data = Convert.FromBase64String(imagemBase64);
            return IsImageFromByte(data);
        }
        catch (Exception)
        {
            return false;
        }
    }
        
    private static bool IsImageFromByte(byte[] bytes)
    {
        string jpgHeader = "FFD8FFE0";
        string pngHeader = "89504E47";
        string gifHeader = "47494638";
        string header = ByteArrayToHexString(bytes.Take(4).ToArray());

        return (header == jpgHeader || header == pngHeader || header == gifHeader);
    }

    /// <summary>
    /// Determines whether the provided byte array represents an image.
    /// </summary>
    /// <param name="bytes">The byte array to evaluate.</param>
    /// <returns>True if the byte array represents a supported image format; otherwise, false.</returns>
    public static bool IsImage(this byte[] bytes)
    {
        string jpgHeader = "FFD8FFE0";
        string pngHeader = "89504E47";
        string gifHeader = "47494638";
        string header = ByteArrayToHexString(bytes.Take(4).ToArray());

        return header == jpgHeader || header == pngHeader || header == gifHeader;
    }

    private static string ByteArrayToHexString(byte[] bytes)
    {
        return string.Concat(bytes.Select(b => b.ToString("X2")));
    }
}