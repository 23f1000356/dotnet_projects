using System.Windows;
using PracticeFA.App.Models;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>
/// P05b facade — delegates to ISettingsService from DI (P39).
/// </summary>
public static class SettingsService
{
    public static string SettingsFilePath => Get().SettingsFilePath;

    public static AppSettings Current => Get().Current;

    public static string? LastLoadWarning => Get().LastLoadWarning;

    public static AppSettings Load() => Get().Load();

    public static (bool Success, string Message) Save(AppSettings settings) => Get().Save(settings);

    public static void ApplyTheme(Application application, string theme) =>
        Get().ApplyTheme(application, theme);

    private static ISettingsService Get() => App.GetRequiredService<ISettingsService>();
}
