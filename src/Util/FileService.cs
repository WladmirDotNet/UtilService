using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UtilService.Util;

/// <summary>
/// Class for files
/// </summary>
public static class FileService
{
    /// <summary>
    /// Open a text file and convert each line into a list item
    /// </summary>
    /// <param name="arquivo"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<List<string>> OpenToList(string arquivo)
    {
        if (!File.Exists(arquivo))
        {
            throw new Exception("O arquivo " + arquivo + " não existe.");
        }

        var lista = new List<string>();

        var fluxoTexto = new StreamReader(arquivo);

        while (!fluxoTexto.EndOfStream)
        {
            lista.Add(await fluxoTexto.ReadLineAsync());
        }

        fluxoTexto.Close();
        fluxoTexto.Dispose();

        return lista;
    }

    /// <summary>
    /// Checks if the memoryStream has BOM
    /// </summary>
    /// <param name="memoryStream"></param>
    /// <returns></returns>
    public static bool HasBom(this MemoryStream memoryStream)
    {
        if (memoryStream.Length < 2)
            return false;

        var bom = new byte[4]; 
        memoryStream.Seek(0, SeekOrigin.Begin);
        memoryStream.Read(bom, 0, bom.Length);

        if (bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF)
            return true;

        if (bom[0] == 0xFE && bom[1] == 0xFF)
            return true;

        if (bom[0] == 0xFF && bom[1] == 0xFE)
            return true;

        if (bom[0] == 0x00 && bom[1] == 0x00 && bom[2] == 0xFE && bom[3] == 0xFF)
            return true;

        if (bom[0] == 0xFF && bom[1] == 0xFE && bom[2] == 0x00 && bom[3] == 0x00)
            return true;

        return false;
    }
    
    /// <summary>
    /// Checks if the file has a valid extension
    /// </summary>
    public class FileExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        /// <inheritdoc />
        public FileExtensionAttribute(string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
            ErrorMessage = $"Extensões permitidas: {string.Join(", ", _allowedExtensions)}";
        }

        /// <summary>
        /// Checks if the file has a valid extension
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var fileName = value.ToString();

            if (string.IsNullOrEmpty(fileName))
                return ValidationResult.Success;

            var extension = Path.GetExtension(fileName)?.ToLower();

            if (!_allowedExtensions.Contains(extension))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }

}