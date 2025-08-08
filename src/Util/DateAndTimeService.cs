using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.InteropServices;
using UtilService.Util.ApplicationEnum;

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
            return ToFormatedTimeSpan(value, FormatTimeType.Hhmmssfff);
        }
        catch (Exception)
        {
            return "";
        }
    }
    
    /// <summary>
    /// Format TimeSpan String
    /// </summary>
    /// <param name="value"></param>
    /// <param name="formatTimeType"></param>
    /// <returns></returns>
    public static string ToFormatedTimeSpan(this TimeSpan value, FormatTimeType formatTimeType)
    {
        try
        {
            return formatTimeType switch
            {
                FormatTimeType.Hhmm => $"{value.Hours:00}:{value.Minutes:00}",
                FormatTimeType.Hhmmss => $"{value.Hours:00}:{value.Minutes:00}:{value.Seconds:00}",
                FormatTimeType.Hhmmssfff => $"{value.Hours:00}:{value.Minutes:00}:{value.Seconds:00}.{value.Milliseconds:000}",
                _ => throw new ArgumentOutOfRangeException(nameof(formatTimeType), formatTimeType, null)
            };
        }
        catch (Exception)
        {
            return "";
        }
    }    

    /// <summary>
    /// Attempts to extract a DateTime object from the specified string using the provided culture format.
    /// </summary>
    /// <param name="value">The date and time string to parse.</param>
    /// <param name="culture">The culture code used to format the date and time, with a default value of "pt-BR".</param>
    /// <returns>Returns a nullable DateTime object if parsing is successful; otherwise, null.</returns>
    public static DateTime? ExtractDateTime(this string value, string culture = "pt-BR") => DateTime.Parse(value, new CultureInfo(culture));

    /// <summary>
    /// Check if string can be extracted into TimeOnly
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool CanExtractTimeOnly(this string value)
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

        if (dt.Kind != DateTimeKind.Utc)
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);

        return TimeZoneInfo.ConvertTimeFromUtc(dt, brasilTimeZone);
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
    /// Formats the time component of a DateTime object into a string based on the specified format.
    /// </summary>
    /// <param name="dateTime">The DateTime instance to format.</param>
    /// <param name="formatTimeType">The format type specifying how the time should be represented.</param>
    /// <returns>A string representing the formatted time component of the DateTime instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the provided formatTimeType is not a valid value.</exception>
    public static string ToFormateTime(this DateTime dateTime, FormatTimeType formatTimeType)
    {
        return formatTimeType switch
        {
            FormatTimeType.Hhmm => dateTime.ToString("HH:mm"),
            FormatTimeType.Hhmmss => dateTime.ToString("HH:mm:ss"),
            FormatTimeType.Hhmmssfff => dateTime.ToString("HH:mm:ss.fff"),
            _ => throw new ArgumentOutOfRangeException(nameof(formatTimeType), formatTimeType, null)
        };
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

    /// <summary>
    /// Converts decimal to formatted time string HH:mm (e.g., 0.5 = "00:30", 99.5 = "99:30")
    /// </summary>
    /// <param name="decimalValue">Decimal value representing hours with fractions</param>
    /// <returns>Formatted time string in HH:mm format</returns>
    public static string ToTimeString(this decimal decimalValue)
    {
        if (decimalValue == 0) return "00:00";
        if (decimalValue < 0) decimalValue = 0;
        
        var totalMinutes = (int)(decimalValue * 60);
        var hours = totalMinutes / 60;
        var minutes = totalMinutes % 60;
        
        return $"{hours:D2}:{minutes:D2}";
    }
    
    /// <summary>
    /// Parses a time string in HH:mm format into decimal hours
    /// </summary>
    /// <param name="timeString">Time string to parse (HH:mm format)</param>
    /// <returns>Decimal representation of hours, or 0 if parsing fails</returns>
    public static decimal ToDecimal(string timeString)
    {
        if (string.IsNullOrEmpty(timeString))
            return 0;

        if (TimeSpan.TryParse(timeString, out var time))
            return (decimal)time.TotalHours;

        return 0;
    }    

    /// <summary>
    /// Formats month and year into a localized string using a customizable template
    /// </summary>
    /// <param name="month">Month number (1-12)</param>
    /// <param name="year">Year value</param>
    /// <param name="template">Template string with placeholders: {m} for month, {y} for year (default: "{m} {y}")</param>
    /// <param name="culture">Culture code for localization (default: "pt-BR")</param>
    /// <returns>Formatted month/year string based on template and culture</returns>
    /// <example>
    /// FormatMonthYear(1, 2025, "{m} {y}","pt-BR") → "janeiro 2025"
    /// FormatMonthYear(1, 2025, "{y}/{m}","en-US") → "2025/January"
    /// FormatMonthYear(1, 2025, "{m}","pt-BR") → "janeiro"
    /// </example>
    public static string FormatMonthYear(int month, int year, string template = "{m} {y}", string culture = "pt-BR")
    {
        var cultureInfo = new CultureInfo(culture);

        var monthName = cultureInfo.DateTimeFormat.GetMonthName(month);

        var result = template
            .Replace("{m}", monthName.ToPascalCase())
            .Replace("{y}", year.ToString());

        return result;
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
    /// Constructor
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
/// DatAnnotation for validate TimeOnly is greater than other TimeOnly  
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
        if (value is not TimeOnly endTime)
            return new ValidationResult(NullEndTimeErrorMessage);

        var startTimeProperty = validationContext.ObjectType.GetProperty(_startTimePropertyName);

        if (startTimeProperty is null)
            return new ValidationResult($"Property {_startTimePropertyName} not found.");

        var startTime = (TimeOnly?)startTimeProperty.GetValue(validationContext.ObjectInstance);

        if (startTime is null)
            return new ValidationResult($"Property {_startTimePropertyName} is null.");

        return endTime < startTime ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
    }
}