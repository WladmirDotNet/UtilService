using System;
using System.Collections.Generic;
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
    
    /// <summary>
    /// Generate a verification code
    /// </summary>
    /// <param name="segmentCount"></param>
    /// <param name="lettersPerSegment"></param>
    /// <returns></returns>
    public static string GenerateVerificationCode(int segmentCount, int lettersPerSegment)
    {
        var segments = new List<string>();

        for (int i = 0; i < segmentCount; i++)
        {
            var segment = Generate(lettersPerSegment, false, false).ToUpper();
            segments.Add(segment);
        }

        return string.Join("-", segments);
    }
}