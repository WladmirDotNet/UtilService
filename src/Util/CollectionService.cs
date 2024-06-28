using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace UtilService.Util;

/// <summary>
/// Class to check minimum list itens
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class MinimumItemsAttribute : ValidationAttribute
{
    /// <summary>
    /// Minimum item quantity
    /// </summary>
    public int Minimum { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="minimum"></param>
    public MinimumItemsAttribute(int minimum)
    {
        Minimum = minimum;
    }

    /// <summary>
    /// Validate method
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is ICollection collection)
        {
            return collection.Count < Minimum ? new ValidationResult(ErrorMessage ?? "It is mandatory to inform items in the collection") : ValidationResult.Success;
        }

        return ValidationResult.Success;
    }

    
}