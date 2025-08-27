using System.ComponentModel;

namespace UtilService.Util.ApplicationEnum;

/// <summary>
/// Represents the time zones applicable in Brazil.
/// Includes mappings for both Linux/macOS and Windows time zone identifiers.
/// </summary>
public enum BrazilTimeZoneType
{
    /// <summary>
    /// Represents UTC-02:00 Fernando de Noronha Time.
    /// Linux/macOS-America/Noronha
    /// Windows-UTC-02 Standard Time
    /// </summary>
    [Description("UTC-02")] [DefaultValue("America/Noronha")]
    AmericaNoronha = 1,

    /// <summary>
    /// Represents UTC-03:00 Brasília Time.
    /// Linux/macOS-America/Sao_Paulo
    /// Windows-E. South America Standard Time.
    /// </summary>
    [Description("E. South America Standard Time")]
    [DefaultValue("America/Sao_Paulo")]
    AmericaSaoPaulo = 2,

    /// <summary>
    /// Represents Central Brazilian Standard Time UTC-04:00.
    /// Linux/macOS-America/Cuiabá
    /// Windows-Central Brazilian Standard Time.
    /// </summary>
    [Description("Central Brazilian Standard Time")]
    [DefaultValue("America/Cuiaba")]
    AmericaCuiaba = 3,

    /// <summary>
    /// Represents UTC-05:00 Acre Time.
    /// Linux/macOS-America/Rio_Branco
    /// Windows-SA Pacific Standard Time.
    /// </summary>
    [Description("SA Pacific Standard Time")]
    [DefaultValue("America/Rio_Branco")]
    AmericaRioBranco = 4
}