

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UtilService.Util;

/// <summary>
/// Class for files
/// </summary>
public static class FileOperations
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

}