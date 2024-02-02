using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace UtilService.Util;

/// <summary>
/// Classe to time
/// </summary>
public static class DateAndTimeOperations
{
    /// <summary>
    /// Formated Datetime String
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToFormatedDateTime(this DateTime value)
    {
        return $"{value:dd/MM/yyyy}  {value.ToShortTimeString()}";
    }

    /// <summary>
    /// Formated Datetime String
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToFormatedDateTime(this DateTime? value)
    {
        return value.HasValue ? $"{value:dd/MM/yyyy}  {value.Value.ToShortTimeString()}" : string.Empty;
    }

    /// <summary>
    /// Format TimeSpan String
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToFormatedTimeSpan(this TimeSpan value)
    {
        try
        {
            return $"{value.Hours:00}:{value.Minutes:00}:{value.Seconds:00}.{value.Milliseconds:000}";
        }
        catch (Exception)
        {
            return "";
        }
    }

    /// <summary>
    /// Check if string can be extracted into TimeOnly
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool CanExtactTimeOnly(this string value)
    {
        var canConvert = TimeOnly.TryParseExact(value, "MM/dd/yyyy HH:mm:ss", out _);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "dd/MM/yyyy HH:mm:ss", out _);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "yyyy/MM/dd HH:mm:ss", out _);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "MM-dd-yyy HH:mm:ss", out _);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "dd-MM-yyyy HH:mm:ss", out _);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "yyyy-MM-dd HH:mm:ss", out _);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "dd/MM/yyyy HH:mm", out _);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "yyyy/MM/dd HH:mm", out _);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "MM-dd-yyy HH:mm", out _);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "dd-MM-yyyy HH:mm", out _);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "yyyy-MM-dd HH:mm", out _);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "HH:mm:ss", out _);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "HH:mm", out _);

        return canConvert;

    }

    /// <summary>
    /// Extract TimeOnly from string object
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TimeOnly? ExtractTimeOnly(this string value)
    {
        var canConvert = TimeOnly.TryParseExact(value, "MM/dd/yyyy HH:mm:ss", out var result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "dd/MM/yyyy HH:mm:ss", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "yyyy/MM/dd HH:mm:ss", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "MM-dd-yyy HH:mm:ss", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "dd-MM-yyyy HH:mm:ss", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "yyyy-MM-dd HH:mm:ss", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "MM/dd/yyyy HH:mm", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "dd/MM/yyyy HH:mm", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "yyyy/MM/dd HH:mm", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "MM-dd-yyy HH:mm", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "dd-MM-yyyy HH:mm", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "yyyy-MM-dd HH:mm", out result);


        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "HH:mm:ss", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "HH:mm", out result);

        return !canConvert ? null : result;

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TimeOnly ExtractTimeOnlyValue(this string value)
    {
        var canConvert = TimeOnly.TryParseExact(value, "MM/dd/yyyy HH:mm:ss", out var result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "dd/MM/yyyy HH:mm:ss", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "yyyy/MM/dd HH:mm:ss", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "MM-dd-yyy HH:mm:ss", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "dd-MM-yyyy HH:mm:ss", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "yyyy-MM-dd HH:mm:ss", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "MM/dd/yyyy HH:mm", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "dd/MM/yyyy HH:mm", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "yyyy/MM/dd HH:mm", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "MM-dd-yyy HH:mm", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "dd-MM-yyyy HH:mm", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "yyyy-MM-dd HH:mm", out result);


        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "HH:mm:ss", out result);

        if (!canConvert)
            canConvert = TimeOnly.TryParseExact(value, "HH:mm", out result);

        return !canConvert ? throw new Exception($"The value {value} can't convert to TimeOnly") : result;

    }

    /// <summary>
    /// Convert datetime to Brazilian local datetime
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime ToBrasil(this DateTime dt)
    {
        var brasilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        var dateTime = TimeZoneInfo.ConvertTime(dt, brasilTimeZone);

        return dateTime;
    }

    /// <summary>
    /// Convert datetime to Brazilian local datetime -3 hours
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime ToBrasilDateTime(this DateTime dt)
    {
        var dateTime = dt.AddHours(-3);

        return dateTime;
    }

    /// <summary>
    /// Get week day from int value
    /// </summary>
    /// <param name="weekDay"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static string GetWeekDay(this int weekDay, string culture = "pt-BR")
    {
        var cultureInfo = new CultureInfo(culture);
        var weekDayEnum = (DayOfWeek)Enum.ToObject(typeof(DayOfWeek), weekDay);
        return cultureInfo.DateTimeFormat.GetDayName(weekDayEnum);
    }
}

/// <summary>
/// Dataannotation for validate TimeOnly range
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class TimeOnlyRangeAttribute : ValidationAttribute
{
    private readonly TimeOnly _minTime;
    private readonly TimeOnly _maxTime;

    /// <summary>
    /// Contructor
    /// </summary>
    /// <param name="minTime"></param>
    /// <param name="maxTime"></param>
    public TimeOnlyRangeAttribute(string minTime, string maxTime)
    {
        _minTime = TimeOnly.Parse(minTime);
        _maxTime = TimeOnly.Parse(maxTime);

        ErrorMessage = $"O valor deve estar no intervalo de {minTime} a {maxTime}.";
    }

    /// <summary>
    /// Validation method
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not null && TimeOnly.TryParse(value.ToString(), out var timeOnlyValue))
        {
            if (timeOnlyValue >= _minTime && timeOnlyValue <= _maxTime)
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult(ErrorMessage);
    }
}