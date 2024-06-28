using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UtilService.Util;

/// <summary>
/// Classe for valid model
/// </summary>
public static class ValidationExtensions
{

    /// <summary>
    /// Check valid model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static void CheckIfModelIsValid<T>(this T obj)
    {
        var context = new ValidationContext(obj);
        var results = new System.Collections.Generic.List<ValidationResult>();
        if (!Validator.TryValidateObject(obj, context, results, true)) 
        {
            throw new Exception(results.FirstOrDefault()?.ErrorMessage);
        }            
    }
}