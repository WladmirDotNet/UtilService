using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UtilService.Util;

/// <summary>
/// Class for telephone and email operations
/// </summary>
public static class PhoneAndEmailService
{
    /// <summary>
    /// Validates whether a phone number is valid
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    public static bool IsValidPhone(this string phoneNumber)
    {

        var cleanedPhoneNumber = Regex.Replace(phoneNumber, @"[^\d]", "");


        if (cleanedPhoneNumber.Length != 11 && cleanedPhoneNumber.Length != 10)
            return false;


        var ddd = cleanedPhoneNumber.Substring(0, 2);


        if (!int.TryParse(ddd, out var dddValue) || (dddValue < 11 || dddValue > 99))
            return false;


        return cleanedPhoneNumber.Length is 10 or 11;
    }

    /// <summary>
    /// Format phone 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToFormatedPhoneString(this string value)
    {
        try
        {

            if (!value.IsNullOrWhiteSpace() && value.Length > 10)
            {
                return long.Parse(value).ToString("(##)#####-####");
            }

            return long.Parse(value).ToString("(##)####-####");

        }
        catch (Exception)
        {
            return value ?? "";
        }
    }

    /// <summary>
    /// Checks if the email is valid
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool IsEmailValid(this string email)
    {
        // Define a expressão regular para validar emails
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // Verifica se o email corresponde ao padrão
        return Regex.IsMatch(email, pattern);
    }

}
    
/// <inheritdoc />
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
public class ValidateEmailAddressAttribute : ValidationAttribute
{
    /// <inheritdoc />
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(value?.ToString()))
        {
            return ValidationResult.Success;
        }

        var emailAttribute = new EmailAddressAttribute();
        return !emailAttribute.IsValid(value)
            ? new ValidationResult(ErrorMessage ?? "Invalid e-mail")
            : ValidationResult.Success;
    }
}