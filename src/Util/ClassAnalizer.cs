using System.Collections.Generic;
using System.Linq;

namespace UtilService.Util;

/// <summary>
/// Classe for object diferences
/// </summary>
public static class ClassAnalizer
{
    /// <summary>
    /// Class comparer
    /// </summary>
    /// <param name="itemOriginal"></param>
    /// <param name="itemAtual"></param>
    /// <returns></returns>
    public static List<ClassCompareResult> Compare(object itemOriginal, object itemAtual)
    {
        var propsOriginal = itemOriginal.GetType().GetProperties();
        var propsAtual = itemAtual.GetType().GetProperties();

        return (from pi in propsOriginal
            let dataPropertieAtual = propsAtual.FirstOrDefault(o =>
                o.Name == pi.Name && o.GetValue(itemAtual, null) != pi.GetValue(itemOriginal, null))
            where dataPropertieAtual != null
            select new ClassCompareResult
            (
                pi.Name,
                pi.GetValue(itemOriginal, null)?.ToString(),
                dataPropertieAtual.GetValue(itemAtual, null)?.ToString()
            )).ToList();
    }


    /// <summary>
    /// Cladd diferences
    /// </summary>
    /// <param name="compare"></param>
    /// <returns></returns>
    public static List<ClassCompareResult> GetDiferences(List<ClassCompareResult> compare)
    {
        return compare.Where(o => o.ValorCampoAnterior != o.ValorCampoAtual).ToList();
    }

    /// <summary>
    /// List comparer
    /// </summary>
    /// <param name="list1"></param>
    /// <param name="list2"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool EspecialEqualsToList<T>(IEnumerable<T> list1, IEnumerable<T> list2)
    {
        var cnt = new Dictionary<T, int>();
        foreach (T s in list1)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]++;
            }
            else
            {
                cnt.Add(s, 1);
            }
        }
        foreach (T s in list2)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]--;
            }
            else
            {
                return false;
            }
        }
        return cnt.Values.All(c => c == 0);
    }

    /// <summary>
    /// DTO class for compare
    /// </summary>
    public class ClassCompareResult
    {
        /// <summary>
        /// Classe for compare result
        /// </summary>
        /// <param name="nomeCampo"></param>
        /// <param name="valorCampoAnterior"></param>
        /// <param name="valorCampoAtual"></param>
        public ClassCompareResult(string nomeCampo, string valorCampoAnterior, string valorCampoAtual)
        {
            NomeCampo = nomeCampo;
            ValorCampoAnterior = valorCampoAnterior;
            ValorCampoAtual = valorCampoAtual;
        }

        /// <summary>
        /// Field name
        /// </summary>
        public string NomeCampo { get; private set; }
        /// <summary>
        /// Field value
        /// </summary>
        public string ValorCampoAnterior { get; private set; }
        /// <summary>
        /// Current field value
        /// </summary>
        public string ValorCampoAtual { get; private set; }
    }
}