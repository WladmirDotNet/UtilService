using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace UtilService.Util;

/// <summary>
/// Classe de mátodos de extensão para objetos serem transformados em string formatada
/// </summary>
public static class TextOperations
{
    /// <summary>
    /// Trims the string properties of a data model
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    public static void TrimStringProperties<T>(this T obj)
    {
        // Obtém as propriedades públicas do objeto do tipo T
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Percorre todas as propriedades
        foreach (var property in properties)
        {
            // Verifica se a propriedade é do tipo string
            if (property.PropertyType == typeof(string))
            {
                // Obtém o valor atual da propriedade
                var value = (string)property.GetValue(obj);

                // Faz o Trim do valor e atribui de volta à propriedade
                if (value != null)
                {
                    var trimmedValue = value.Trim();
                    property.SetValue(obj, trimmedValue);
                }
            }
        }
    }

    /// <summary>
    /// Converts a string to null if empty
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string AsNullIfEmpty(this string value)
    {
        return !value.IsNullOrWhiteSpace() ? value : null;
    }

    /// <summary>
    /// Converts a string to null if it has empty space
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string AsNullIfWhiteSpace(this string value)
    {
        return !value.IsNullOrWhiteSpace() ? value : null;
    }

    /// <summary>
    /// Checks if it is null or empty space
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsNullOrWhiteSpace(this string text)
    {
        return string.IsNullOrWhiteSpace(text);
    }

    /// <summary>
    /// Format string for trim and uppercase
    /// </summary>
    /// <param name="texto"></param>
    /// <returns></returns>
    public static string ToTrimUpper(this string texto)
    {
        return !string.IsNullOrEmpty(texto) ? texto.Trim().ToUpper() : string.Empty;
    }

    /// <summary>
    /// Format string to trim or empty
    /// </summary>
    /// <param name="texto"></param>
    /// <returns></returns>
    public static string ToTrimOrEmpty(this string texto)
    {
        return !string.IsNullOrEmpty(texto) ? texto.Trim().ToUpper() : string.Empty;
    }

    /// <summary>
    /// Format string to trim and lowercase
    /// </summary>
    /// <param name="texto"></param>
    /// <returns></returns>
    public static string ToTrimLower(this string texto)
    {
        return !string.IsNullOrEmpty(texto) ? texto.Trim().ToLower() : string.Empty;
    }

    /// <summary>
    /// Returns only letters from a string
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string LettersOnly(this string text)
    {
        var input = text;
        const string expr = @"(?i)[^a-záéíóúàèìòùâêîôûãõç\s]";
        var rgx = new Regex(expr);
        return rgx.Replace(input, string.Empty).Trim();
    }

    /// <summary>
    /// Returns letters and numbers from a string, removing special characters
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string LettersAndNumbersOnly(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        var input = text;
        const string expr = @"(?i)[^a-z0-9áéíóúàèìòùâêîôûãõç\s]";
        var rgx = new Regex(expr);
        return rgx.Replace(input, string.Empty).Trim();
    }

    /// <summary>
    /// Returns numbers from a string 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string NumbersOnly(this string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        var ret = string.Join(string.Empty, Regex.Split(text, @"[^\d]")).Trim();
        return ret.IsNullOrWhiteSpace() ? string.Empty : ret;
    }

    /// <summary>
    /// Checks if the text is made up of numbers only
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsNumericOnly(this string text)
    {
        return text != null && Regex.IsMatch(text!, "^[0-9]+$");
    }

    /// <summary>
    /// Creates a join of all list items separated by char in the spliter parameter
    /// </summary>
    /// <param name="list"></param>
    /// <param name="spliter"></param>
    /// <returns></returns>
    public static string Join(this IEnumerable<string> list, char spliter)
    {
        return string.Join(spliter,list);
    }
}