namespace PracticeFA.App.Models;

/// <summary>P05b — client preferences persisted to %AppData%/PracticeFA/settings.json (no SQL).</summary>
public sealed class AppSettings
{
    public string PlantCode { get; set; } = "P01";
    public string DefaultPrinter { get; set; } = "";
    public string Theme { get; set; } = AppThemeNames.Light;

    public static AppSettings CreateDefault() => new();

    public AppSettings Clone() =>
        new()
        {
            PlantCode = PlantCode,
            DefaultPrinter = DefaultPrinter,
            Theme = Theme,
        };
}

public static class AppThemeNames
{
    public const string Light = "Light";
    public const string Dark = "Dark";

    public static IReadOnlyList<string> All { get; } = [Light, Dark];

    public static bool IsValid(string? theme) =>
        string.Equals(theme, Light, StringComparison.OrdinalIgnoreCase)
        || string.Equals(theme, Dark, StringComparison.OrdinalIgnoreCase);
}
