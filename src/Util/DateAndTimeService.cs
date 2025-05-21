using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.InteropServices;

namespace UtilService.Util;

/// <summary>
/// Classe to time
/// </summary>
public static class DateAndTimeService
{
    
    /// <summary>
    /// Format string time with zero in minutes and hours ex: 7:5 -> 07:05 
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string FormatTimeInString(this string time)
    {
        if (time.IsNullOrWhiteSpace() || !time.Contains(':'))
            return string.Empty;
        
        string[] partes = time.Split(':');
        string formatedTime = partes[0].PadLeft(2, '0');
        string formatedMinutes = partes[1].PadLeft(2, '0');
        string newTime = formatedTime + ":" + formatedMinutes;

        return newTime;
    }
    
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
    public static DateTime ToBrazilTimeZone(this DateTime dt)
    {
        var brasilTimeZone = GetBrazilTimeZone();
        return TimeZoneInfo.ConvertTimeFromUtc(dt, brasilTimeZone);
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

    /// <summary>
    /// Check if date is weekend
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>

    public static bool IsWeekend(this DateTime date)
    {
        return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }
    
    #region Private Methods
    
    private static TimeZoneInfo GetBrazilTimeZone()
    {
        return TimeZoneInfo.FindSystemTimeZoneById(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "E. South America Standard Time" : "America/Sao_Paulo");
    }
    
    #endregion private Methods
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

/// <summary>
/// Block if weekend
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class BlockIfWeekendAttribute : ValidationAttribute
{
    /// <summary>
    /// Constructor 
    /// </summary>
    public BlockIfWeekendAttribute()
    {
        ErrorMessage = "The date is can't be weekend.";
    }

    /// <summary>
    /// Validation method
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dateTime)
        {
            return !dateTime.IsWeekend() ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }

        return new ValidationResult(ErrorMessage);
    }
}

/// <summary>
/// Datannotation for validate TimeOnly is greater than other TimeOnly  
/// </summary>
public class TimeGreaterThanAttribute : ValidationAttribute
{
    private readonly string _startTimePropertyName;
    
    private const string NullEndTimeErrorMessage = "The start time is null.";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="startTimePropertyName"></param>
    public TimeGreaterThanAttribute(string startTimePropertyName)
    {
        _startTimePropertyName = startTimePropertyName;
        ErrorMessage = $"The field {startTimePropertyName} is greater than this field.";
    }

    /// <summary>
    /// Validation method
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if(value is not TimeOnly endTime)
            return new ValidationResult(NullEndTimeErrorMessage);
        
        var startTimeProperty = validationContext.ObjectType.GetProperty(_startTimePropertyName);
        
        if (startTimeProperty is null)
            return new ValidationResult($"Property {_startTimePropertyName} not found.");

        var startTime = (TimeOnly?) startTimeProperty.GetValue(validationContext.ObjectInstance);

        if(startTime is null)
            return new ValidationResult($"Property {_startTimePropertyName} is null.");
        
        return endTime < startTime ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
    }
}