using System;
using System.Linq;

namespace UtilService.Util;

/// <summary>
/// Classe for generate strings
/// </summary>
public static class StringGeneratorService
{
      
    /// <summary>
    /// Create ramdom strings
    /// </summary>
    /// <param name="tamanho"></param>
    /// <param name="incluirNumeros"></param>
    /// <param name="incluirCaracteresEspeciais"></param>
    /// <returns></returns>
    public static string Generate(int tamanho, bool incluirNumeros, bool incluirCaracteresEspeciais)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        chars += chars.ToLower();

        if(incluirNumeros)
            chars += "0123456789";

        if(incluirCaracteresEspeciais)
            chars += "@#$%¨&*()_-";

        var random = new Random();
        var result = new string(Enumerable.Repeat(chars, tamanho).Select(s => s[random.Next(s.Length)]).ToArray());
        return result;
    }
}