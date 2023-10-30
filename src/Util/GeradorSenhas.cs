using System;
using System.Linq;

namespace UtilService.Util;

/// <summary>
/// Classe focada em geração de senhas
/// </summary>
public static class GeradorSenhas
{
    /// <summary>
    /// Método focado em geração de senhas
    /// </summary>
    /// <param name="tamanho"></param>
    /// <returns></returns>
    public static string Gerar(int tamanho)
    {
        var chars = $"{"ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower()}ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#$%¨&*()_-";
        var random = new Random();
        var result = new string(Enumerable.Repeat(chars, tamanho).Select(s => s[random.Next(s.Length)]).ToArray());
        return result;
    }
}