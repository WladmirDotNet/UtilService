using System;
using System.ComponentModel.DataAnnotations;

namespace UtilService.Util;

/// <summary>
/// Validate guid null
/// </summary>
public class ValidateGuidNullAttribute : ValidationAttribute
{
    /// <summary>
    /// Constructor
    /// </summary>
    public ValidateGuidNullAttribute()
    {
        ErrorMessage = ErrorMessage.IsNullOrWhiteSpace() ? "Guid can not be null." : ErrorMessage;
    }

    /// <summary>
    /// Validade if guid is null
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool IsValid(object value)
    {
        if (value is Guid guidValue)
        {
            return guidValue != Guid.Empty;
        }

        return true;
    }
}