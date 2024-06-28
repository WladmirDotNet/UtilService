using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UtilService.Util;

/// <summary>
/// Classe for address
/// </summary>
public static class AddressService
{

    /// <summary>
    /// Format string to CEP
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToFormatedCepString(this string value)
    {
        try
        {
            return !value.IsNullOrWhiteSpace() ? long.Parse(value).ToString(@"00\.000-000") : string.Empty;
        }
        catch (Exception)
        {
            return "";
        }
    }
    /// <summary>
    /// Check CEP is valid
    /// </summary>
    /// <param name="cep"></param>
    /// <returns></returns>
    public static bool IsValidCep(this string cep)
    {
        if (!cep.IsNumericOnly())
            return false;

        return cep.Length == 8;
    }
}

#region DataAnnotations

/// <summary>
/// CEP validator
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class ValidCepAttribute : ValidationAttribute
{
    /// <summary>
    /// Check if CEP is valid
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        string cep = value.ToString();

        return cep.IsValidCep() ? ValidationResult.Success : new ValidationResult(ErrorMessage ?? "Formato de CEP inválido.");
    }
}

/// <summary>
/// Validate Brazilian UF
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class ValidUfBrasilAttribute : ValidationAttribute
{
    /// <summary>
    /// Validate Brazilian UF
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var uf = value.ToString()?.ToUpper();

        return IsValidUf(uf) ? ValidationResult.Success : new ValidationResult(ErrorMessage ?? "UF inválida.");
    }

    private bool IsValidUf(string uf)
    {
        string[] ufsValidas =
        {
            "AC",
            "AL",
            "AP", 
            "AM", 
            "BA", 
            "CE", 
            "DF", 
            "ES", 
            "GO", 
            "MA", 
            "MT", 
            "MS", 
            "MG", 
            "PA", 
            "PB", 
            "PR", 
            "PE", 
            "PI",
            "RJ", 
            "RN", 
            "RS", 
            "RO", 
            "RR", 
            "SC", 
            "SP", 
            "SE", 
            "TO"
        };

        return ufsValidas.Contains(uf);
    }
}

#endregion DataAnnotations