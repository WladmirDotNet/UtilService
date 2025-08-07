using System.ComponentModel;

namespace UtilService.Util.ApplicationEnum;

/// <summary>
/// Specifies the format for representing time.
/// </summary>
public enum FormatTimeType
{
    /// <summary>
    /// HH:mm
    /// </summary>
    [Description ("HH:mm")]
    Hhmm = 0,
    
    /// <summary>
    /// HH:mm:ss
    /// </summary>
    [Description ("HH:mm:ss")]
    Hhmmss = 1,
    
    /// <summary>
    /// HH:mm:ss.fff
    /// </summary>
    [Description ("HH:mm:ss.fff")]
    Hhmmssfff = 2
}