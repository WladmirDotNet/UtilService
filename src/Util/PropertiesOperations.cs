using System;
using System.ComponentModel.DataAnnotations;

namespace UtilService.Util;

/// <summary>
/// Checks whether a specific property has been reported
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class DependentOfAttribute : ValidationAttribute
{
    /// <summary>
    /// Dependent property
    /// </summary>
    public string DependentProperty { get; private set; }

    /// <summary>
    /// Checks whether a specific property has been reported
    /// </summary>
    /// <param name="dependentProperty"></param>
    /// <param name="errorMessage"></param>
    public DependentOfAttribute(string dependentProperty, string errorMessage = "")
        : base(errorMessage)
    {
        DependentProperty = dependentProperty;
    }

    /// <summary>
    /// Checks whether a specific property has been reported
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var dependentValue = validationContext.ObjectType.GetProperty(DependentProperty)?.GetValue(validationContext.ObjectInstance, null);

        if (dependentValue != null && string.IsNullOrEmpty(value?.ToString()))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}

