using System;
using System.ComponentModel.DataAnnotations;

namespace UtilService.Util;

/// <summary>
/// Classe for Id
/// </summary>
public static class DocumentService
{
    /// <summary>
    /// Check if CNPJ is valid
    /// </summary>
    /// <param name="cnpj"></param>
    /// <returns></returns>
    public static bool ValidateCnpj(this string cnpj)
    {
        cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");

        if (cnpj.Length != 14)
            return false;

        int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCnpj = cnpj.Substring(0, 12);
        var soma = 0;

        for (var i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores1[i];

        var resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        tempCnpj += resto;

        soma = 0;
        for (var i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores2[i];

        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        tempCnpj += resto;

        return cnpj.EndsWith(tempCnpj);
    }

    /// <summary>
    /// Check if CPF is valid
    /// </summary>
    /// <param name="cpf"></param>
    /// <returns></returns>
    public static bool ValidateCpf(this string cpf)
    {
        cpf = cpf.Replace(".", "").Replace("-", "");

        if (cpf.Length != 11)
            return false;

        int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCpf = cpf.Substring(0, 9);
        var soma = 0;

        for (var i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];

        var resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        tempCpf += resto;

        soma = 0;
        for (var i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];

        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        tempCpf += resto;

        return cpf.EndsWith(tempCpf);
    }

    /// <summary>
    /// Format string to CPF oru CNPJ
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToFormatedCpfCnpjString(this string value)
    {
        try
        {
            if (!value.IsNullOrWhiteSpace())
            {
                return long.Parse(value).ToString(value.Length < 12 ? @"000\.000\.000\-00" : @"00\.000\.000\/0000\-00");
            }

            return string.Empty;
        }
        catch (Exception)
        {
            return "";
        }
    }

    /// <summary>
    /// Format CrmDocument to seven digits
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string FormatCrmDocument(this string value)
    {
        try
        {
            if (!value.IsNullOrWhiteSpace())
            {
                return value.PadLeft(7, '0');
            }

            return value;
        }
        catch (Exception)
        {
            return "";
        }
    }
}

#region DataAnnotations

/// <summary>
/// Provides validation for Brazilian CNPJ numbers.
/// </summary>
/// <remarks>
/// This attribute is used to validate properties, fields, or parameters to ensure their values are valid CNPJ numbers.
/// Optionally allows formatting delimited by periods, hyphens, and slashes.
/// </remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class ValidCnpjAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets a value indicating whether the CNPJ validation will accept formatted input (e.g., with dots, slashes, and dashes).
    /// If set to true, formatted input is accepted and will be cleaned before validation.
    /// If set to false, only unformatted numeric input is considered valid.
    /// </summary>
    public bool AcceptsFormatted { get; set; } = false;

    /// <summary>
    /// Attribute to validate the format and correctness of a Brazilian CNPJ (Cadastro Nacional da Pessoa Jurídica).
    /// </summary>
    /// <remarks>
    /// The attribute ensures that a CNPJ follows the expected format and validity rules. It optionally accepts formatted CNPJ values (with dots, slashes, and hyphens).
    /// </remarks>
    public ValidCnpjAttribute()
    {
        ErrorMessage ??= "Invalid CNPJ.";
    }

    /// <summary>
    /// Determines whether the input is valid.
    /// </summary>
    /// <param name="value">The value to be validated.</param>
    /// <param name="validationContext"></param>
    /// <returns>Returns true if the input is valid; otherwise, false.</returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var cnpj = value.ToString();

        if (cnpj.IsNullOrWhiteSpace())
        {
            return ValidationResult.Success;
        }
     
        if (!AcceptsFormatted)
        {
            if (!cnpj.IsNumericOnly())
            {
                return new ValidationResult(ErrorMessage ?? "CNPJ must be numbers only.");
            }
        }
    
        var isValid = cnpj.ValidateCnpj();

        return isValid ? ValidationResult.Success : new ValidationResult(ErrorMessage ?? "Invalid CNPJ.");
    }
}

#endregion DataAnnotations