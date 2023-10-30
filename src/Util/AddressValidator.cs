using System;
using System.Text.RegularExpressions;

namespace UtilService.Util;
/// <summary>
/// Classe for address
/// </summary>
public static class AddressValidator
{

    /// <summary>
    /// Format string to CEP
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToFormatedCepString(this string value)
    {
        try
        {
            return !value.IsNullOrWhiteSpace() ? long.Parse(value).ToString(@"00\.000-000") : string.Empty;
        }
        catch (Exception)
        {
            return "";
        }
    }
    /// <summary>
    /// Check CEP is valid
    /// </summary>
    /// <param name="cep"></param>
    /// <returns></returns>
    public static bool IsValidCep(this string cep)
    {
        var cleanedCep = Regex.Replace(cep, @"[^\d]", "");

        return cleanedCep.Length == 8;
    }
}