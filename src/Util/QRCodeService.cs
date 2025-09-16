using System;
using System.Collections.Generic;
using System.IO;
using QRCoder;
using ZXing;
using ZXing.Common;
using ZXing.Multi;
using SixLabors.ImageSharp.PixelFormats;
using UtilService.Util.Model;

namespace UtilService.Util;

/// <summary>
/// Service for reading and generating QR Codes with security validations
/// </summary>
public static class QrCodeService
{
    /// <summary>
    /// Maximum allowed image size in bytes (1MB)
    /// </summary>
    private const long MaxImageSizeBytes = 10 * 1024 * 1024;
    
    /// <summary>
    /// Maximum allowed image dimensions (1920 1920)
    /// </summary>
    private const int MaxImageDimension = 1920;
    
    /// <summary>
    /// Maximum allowed QR Code text length
    /// </summary>
    private const int MaxQrTextLength = 5000;
    
    /// <summary>
    /// Maximum number of QR Codes to process from a single image
    /// </summary>
    private const int MaxQrCodesPerImage = 10;

    /// <summary>
    /// Reads QR Codes from an image and returns a list with the found texts
    /// Can detect multiple QR Codes positioned randomly in the image using ImageSharp
    /// Includes security validations to prevent memory exhaustion and malicious content
    /// </summary>
    /// <param name="imageStream">MemoryStream containing the image</param>
    /// <returns>List of strings with QR Code texts found (empty list if none found)</returns>
    /// <exception cref="ArgumentNullException">Thrown when imageStream is null</exception>
    /// <exception cref="ArgumentException">Thrown when image is too large or invalid</exception>
    public static List<string> ReadQrCodesFromImage(this MemoryStream imageStream)
    {
        if (imageStream == null)
            throw new ArgumentNullException(nameof(imageStream));
            
        if (imageStream.Length == 0)
            throw new ArgumentException("Image stream is empty", nameof(imageStream));
            
        if (imageStream.Length > MaxImageSizeBytes)
            throw new ArgumentException($"Image size ({imageStream.Length} bytes) exceeds maximum allowed size ({MaxImageSizeBytes} bytes)", nameof(imageStream));
        
        if (imageStream.HasBom())
            throw new ArgumentException("Image stream contains BOM (Byte Order Mark), which indicates a text file rather than an image", nameof(imageStream));
        
        var qrCodeTexts = new List<string>();
        
        try
        {
            imageStream.Position = 0;
            
            using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(imageStream);
            
            if (image.Width > MaxImageDimension || image.Height > MaxImageDimension)
                throw new ArgumentException($"Image dimensions ({image.Width}x{image.Height}) exceed maximum allowed size ({MaxImageDimension}x{MaxImageDimension})");
            
            var detectionResults = DetectQrCodesWithImageSharp(image);
            
            var processedCount = 0;
            for (int i = 0; i < detectionResults.Length && processedCount < MaxQrCodesPerImage; i++)
            {
                var sanitizedText = SanitizeQrCodeText(detectionResults[i].Text);
                if (!string.IsNullOrEmpty(sanitizedText))
                {
                    qrCodeTexts.Add(sanitizedText);
                    processedCount++;
                }
            }
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error reading QR Code from image: {ex.Message}", ex);
        }
        
        return qrCodeTexts;
    }
    
    /// <summary>
    /// Detects QR Codes from ImageSharp image using the latest approach
    /// </summary>
    /// <param name="image">ImageSharp image to scan</param>
    /// <returns>Array of QR detection results</returns>
    public static QrDetectionModel[] DetectQrCodesWithImageSharp(SixLabors.ImageSharp.Image<Rgba32> image)
    {
        if (image == null)
            throw new ArgumentNullException(nameof(image));
            
        if (image.Width > MaxImageDimension || image.Height > MaxImageDimension)
            throw new ArgumentException($"Image dimensions ({image.Width}x{image.Height}) exceed maximum allowed size ({MaxImageDimension}x{MaxImageDimension})");
        
        try
        {
            int width = image.Width;
            int height = image.Height;
            
            long expectedPixelDataSize = (long)width * height * 4;
            if (expectedPixelDataSize > MaxImageSizeBytes)
                throw new ArgumentException($"Image pixel data size ({expectedPixelDataSize} bytes) would exceed memory limit");

            var pixelData = new byte[expectedPixelDataSize];
            image.CopyPixelDataTo(pixelData);

            var luminanceSource = new RGBLuminanceSource(pixelData, width, height, RGBLuminanceSource.BitmapFormat.RGBA32);
            var binarizer = new HybridBinarizer(luminanceSource);
            var binaryBitmap = new BinaryBitmap(binarizer);

            var multiReader = new MultiFormatReader();
            var reader = new GenericMultipleBarcodeReader(multiReader);
            var hints = new Dictionary<DecodeHintType, object>
            {
                { DecodeHintType.TRY_HARDER, true },
                { DecodeHintType.POSSIBLE_FORMATS, new [] { BarcodeFormat.QR_CODE } }
            };

            var results = reader.decodeMultiple(binaryBitmap, hints);

            if (results == null || results.Length == 0)
                return Array.Empty<QrDetectionModel>();

            var maxResults = Math.Min(results.Length, MaxQrCodesPerImage);
            var output = new QrDetectionModel[maxResults];
            
            for (int i = 0; i < maxResults; i++)
            {
                var sanitizedText = SanitizeQrCodeText(results[i].Text);
                output[i] = new QrDetectionModel
                {
                    Text = sanitizedText,
                    CornerPoints = results[i].ResultPoints
                };
            }

            return output;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error in QR detection: {ex.Message}", ex);
        }
    }
    
    /// <summary>
    /// Generates a QR Code PNG from text 
    /// </summary>
    /// <param name="text">Text to be encoded in the QR Code</param>
    /// <param name="size">Image size in pixels (default: 300x300)</param>
    /// <param name="eccLevel">Error correction level (default: Q - 25%)</param>
    /// <returns>MemoryStream containing the QR Code PNG image</returns>
    public static MemoryStream GenerateQrCodePngImageStream(this string text, int size = 300, QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be null or empty", nameof(text));
            
        if (text.Length > MaxQrTextLength)
            throw new ArgumentException($"Text length ({text.Length}) exceeds maximum allowed length ({MaxQrTextLength})", nameof(text));
            
        if (size < 50 || size > 600)
            throw new ArgumentException($"Size ({size}) must be between 50 and 600 pixels", nameof(size));
        
        try
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(text, eccLevel);
            
            var estimatedModules = 25;
            var pixelsPerModule = Math.Max(1, Math.Min(size / estimatedModules, 20));
            
            var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeBytes = qrCode.GetGraphic(pixelsPerModule);
            
            if (qrCodeBytes.Length > MaxImageSizeBytes)
                throw new InvalidOperationException($"Generated QR Code size ({qrCodeBytes.Length} bytes) exceeds memory limit");
            
            var memoryStream = new MemoryStream(qrCodeBytes);
            memoryStream.Position = 0;
            
            return memoryStream;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error generating QR Code: {ex.Message}", ex);
        }
    }

    #region Private methods
    
    /// <summary>
    /// Sanitizes QR Code text to prevent malicious content using UtilService TextService methods
    /// </summary>
    /// <param name="text">Raw QR Code text</param>
    /// <returns>Sanitized text or empty string if invalid</returns>
    private static string SanitizeQrCodeText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;
            
        if (text.Length > MaxQrTextLength)
            text = text.Substring(0, MaxQrTextLength);
        
        var sanitized = text.ToTrimOrEmpty();
        
        if (sanitized.IsNullOrWhiteSpace())
            return string.Empty;
        
        if (ContainsSuspiciousContent(sanitized))
            return string.Empty;
            
        return sanitized.AsNullIfWhiteSpace() ?? string.Empty;
    }
    
    /// <summary>
    /// Checks for suspicious content in QR Code text
    /// </summary>
    /// <param name="text">Text to check</param>
    /// <returns>True if content is suspicious</returns>
    private static bool ContainsSuspiciousContent(string text)
    {
        if (string.IsNullOrEmpty(text))
            return false;
        
        var suspiciousPatterns = new[]
        {
            "<script",
            "javascript:",
            "data:text/html",
            "vbscript:",
            "file://",
            "\\x00",
            "\0"
        };
        
        var lowerText = text.ToLowerInvariant();
        
        foreach (var pattern in suspiciousPatterns)
        {
            if (lowerText.Contains(pattern))
                return true;
        }
        
        return false;
    }
    
    #endregion Private methods
    
}