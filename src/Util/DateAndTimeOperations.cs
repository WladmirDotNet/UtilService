using System;
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
        return $"{value.ToShortDateString()}  {value.ToShortTimeString()}";
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
}