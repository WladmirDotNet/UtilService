using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UtilService.Util;

/// <summary>
/// Class of operations with enum
/// </summary>
public static class EnumOperations
{
    /// <summary>
    /// Converts the enum to an integer array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int[] ToIntArray<T>()
    {
        return (int[])Enum.GetValues(typeof(T));
    }

    /// <summary>
    /// Returns the value of an enum by its default value (DefaultValue)
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static TEnum GetEnumValueFromDefaultValue<TEnum>(this string defaultValue)
    {
        foreach (TEnum enumValue in Enum.GetValues(typeof(TEnum)))
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString()!);

            if (fieldInfo == null) continue;
            if (fieldInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false) is DefaultValueAttribute[]
                {
                    Length: > 0
                } attributes && attributes[0].Value!.ToString() == defaultValue)
            {
                return enumValue;
            }
        }

        throw new ArgumentException($"No enum value found with the default value: {defaultValue}");
    }

    /// <summary>
    /// Returns whether enum exists by default value (DefaultValue)
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static bool ExistsEnumValueFromDefaultValue<TEnum>(this string defaultValue)
    {
        foreach (TEnum enumValue in Enum.GetValues(typeof(TEnum)))
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString()!);

            if (fieldInfo == null) continue;
            if (fieldInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false) is DefaultValueAttribute[]
                {
                    Length: > 0
                } attributes && attributes[0].Value!.ToString() == defaultValue)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns whether a string value exists in the enum
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static bool ExistsInEnum<TEnum>(this string value)
    {
        return Enum.TryParse(typeof(TEnum), value, true, out _);
    }

    /// <summary>
    /// Converte string em Enum
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToEnum<T>(this string value) where T : struct
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    /// <summary>
    /// Loads a string collection with data from an enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lista"></param>
    /// <returns></returns>
    public static IEnumerable<string> LoadDataFromEnum<T>(this IEnumerable<string> lista) where T : Enum
    {
        var valoresEnum = Enum.GetValues(typeof(T));
        var listaDeStrings = valoresEnum.Cast<T>().Select(e => e.ToString()).ToList();
        return listaDeStrings;
    }

    /// <summary>
    /// Loads a string collection with DefaultValue data from an enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lista"></param>
    /// <returns></returns>
    public static IEnumerable<string> LoadDataFromEnumDefaultValue<T>(this IEnumerable<string> lista) where T : Enum
    {
        var valoresEnum = Enum.GetValues(typeof(T));
        var listaDeStrings = valoresEnum.Cast<T>().Where(o => o.GetDefaultValue() != null).Select(e => e.GetDefaultValue()).ToList();
        return listaDeStrings!;
    }

    /// <summary>
    /// Loads a string collection with DefaultValue data from an enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lista"></param>
    /// <returns></returns>
    public static IEnumerable<string> LoadDataFromEnumDescription<T>(this IEnumerable<string> lista) where T : Enum
    {
        var valoresEnum = Enum.GetValues(typeof(T));
        var listaDeStrings = valoresEnum.Cast<T>().Where(o => o.GetDescription() != null).Select(e => e.GetDescription()).ToList();
        return listaDeStrings!;
    }
    
    /// <summary>
    /// Retrieves the description of the enum
    /// </summary>
    /// <param name="enum"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum @enum)
    {
        var e = @enum.GetType().GetField(@enum.ToString());
        var attributes = (DescriptionAttribute[])e?.GetCustomAttributes(typeof(DescriptionAttribute), false)!;
        return attributes != null && attributes.Length > 0 ? attributes[0].Description : @enum.ToString();
    }

    /// <summary>
    /// Retrieves the default value of the enum
    /// </summary>
    /// <param name="enum"></param>
    /// <returns></returns>
    public static string GetDefaultValue(this Enum @enum)
    {
        var e = @enum.GetType().GetField(@enum.ToString());
        var attributes = (DefaultValueAttribute[])e?.GetCustomAttributes(typeof(DefaultValueAttribute), false)!;
        return attributes is { Length: > 0 } ? attributes[0].Value?.ToString() : @enum.ToString();
    }

    /// <summary>
    /// Retrieves an integer value from the enum
    /// </summary>
    /// <param name="enum"></param>
    /// <returns></returns>

    public static int GetIntValue(this Enum @enum)
    {
        return Convert.ToInt32(@enum);
    }

    /// <summary>
    /// Convert int to specific enum
    /// </summary>
    /// <param name="valorInteiro"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static TEnum IntToEnum<TEnum>(this int valorInteiro) where TEnum : Enum
    {
        if (Enum.IsDefined(typeof(TEnum), valorInteiro))
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), valorInteiro);
        }
        else
        {
            throw new ArgumentException($"The value {valorInteiro} cant convert to enum {typeof(TEnum).Name}.");
        }
    }

}

/// <summary>
/// Validator for valid enums in DataAnnotation format
/// </summary>
public class EnumDataAnnotations
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class AllowedEnumValuesAttribute : ValidationAttribute
    {
        private readonly object[] _allowedValues;

        /// <inheritdoc />
        public AllowedEnumValuesAttribute(Type enumType, params object[] allowedValues)
        {
            if (enumType is not { IsEnum: true })
            {
                throw new ArgumentException("The type needs to be enum.");
            }

            _allowedValues = allowedValues;
        }

        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (_allowedValues.Contains(value))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.IsNullOrWhiteSpace(ErrorMessage) ? $"O valor '{value}' não é um valor permitido." : ErrorMessage);
        }
    }
}
