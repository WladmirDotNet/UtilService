using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace UtilService.Util;

/// <summary>
/// Class for Numeric Operations.
/// </summary>
public static class NumericService
{
    /// <summary>
    /// Check if the value is in the valid range.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validValues"></param>
    /// <returns></returns>
    public static bool IsInValidRange(this double value, double[] validValues)
    {
        return validValues.Contains(value);
    }
}

/// <summary>
/// Is invalid range attribute
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class IsInvalidRangeAttribute : ValidationAttribute
{
    private readonly double[] _allowedValues;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="allowedValues"></param>
    public IsInvalidRangeAttribute(params double[] allowedValues)
    {
        _allowedValues = allowedValues;
        ErrorMessage = $"The values entered are invalid, the allowed values are: {allowedValues.Select(o => o.ToString(CultureInfo.InvariantCulture)).Join(',')}";
    }

    /// <summary>
    /// Check if the value is in the valid range.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is double doubleValue)
            return doubleValue.IsInValidRange(_allowedValues) ? ValidationResult.Success : new ValidationResult(ErrorMessage);

        return new ValidationResult(ErrorMessage);
    }

}