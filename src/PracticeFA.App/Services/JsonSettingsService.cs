using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using PracticeFA.App.Models;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>P39 — ISettingsService implementation (%AppData%/PracticeFA/settings.json).</summary>
public sealed class JsonSettingsService : ISettingsService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public string SettingsFilePath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "PracticeFA",
            "settings.json");

    public AppSettings Current { get; private set; } = AppSettings.CreateDefault();

    public string? LastLoadWarning { get; private set; }

    public AppSettings Load()
    {
        LastLoadWarning = null;

        try
        {
            if (!File.Exists(SettingsFilePath))
            {
                Current = AppSettings.CreateDefault();
                return Current;
            }

            var json = File.ReadAllText(SettingsFilePath);
            var settings = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions);
            if (settings is null)
            {
                LastLoadWarning = "Settings file was empty — using defaults.";
                Current = AppSettings.CreateDefault();
                return Current;
            }

            Normalize(settings);
            Current = settings;
            return Current;
        }
        catch (JsonException ex)
        {
            LastLoadWarning = $"Invalid JSON — using defaults. ({ex.Message})";
            Current = AppSettings.CreateDefault();
            return Current;
        }
        catch (IOException ex)
        {
            LastLoadWarning = $"Could not read settings — using defaults. ({ex.Message})";
            Current = AppSettings.CreateDefault();
            return Current;
        }
    }

    public (bool Success, string Message) Save(AppSettings settings)
    {
        try
        {
            Normalize(settings);
            var directory = Path.GetDirectoryName(SettingsFilePath)!;
            Directory.CreateDirectory(directory);

            var json = JsonSerializer.Serialize(settings, JsonOptions);
            File.WriteAllText(SettingsFilePath, json);
            Current = settings.Clone();
            ApplyTheme(Application.Current, Current.Theme);

            return (true, $"Settings saved to {SettingsFilePath}");
        }
        catch (Exception ex)
        {
            return (false, $"Could not save settings: {ex.Message}");
        }
    }

    public void ApplyTheme(Application application, string theme)
    {
        if (!AppThemeNames.IsValid(theme))
            theme = AppThemeNames.Light;

        if (string.Equals(theme, AppThemeNames.Dark, StringComparison.OrdinalIgnoreCase))
        {
            application.Resources["AppBackgroundBrush"] = new SolidColorBrush(Color.FromRgb(0x2A, 0x22, 0x1E));
            application.Resources["SurfaceBrush"] = new SolidColorBrush(Color.FromRgb(0x3E, 0x34, 0x30));
            application.Resources["SidebarBrush"] = new SolidColorBrush(Color.FromRgb(0x33, 0x2A, 0x26));
            return;
        }

        application.Resources.Remove("AppBackgroundBrush");
        application.Resources.Remove("SurfaceBrush");
        application.Resources.Remove("SidebarBrush");
    }

    private static void Normalize(AppSettings settings)
    {
        settings.PlantCode = string.IsNullOrWhiteSpace(settings.PlantCode)
            ? "P01"
            : settings.PlantCode.Trim();

        settings.DefaultPrinter = settings.DefaultPrinter?.Trim() ?? "";

        if (!AppThemeNames.IsValid(settings.Theme))
            settings.Theme = AppThemeNames.Light;
    }
}
