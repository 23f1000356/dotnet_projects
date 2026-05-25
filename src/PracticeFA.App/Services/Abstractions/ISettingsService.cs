using System.Windows;
using PracticeFA.App.Models;

namespace PracticeFA.App.Services.Abstractions;

/// <summary>P39 — JSON user preferences (no SQL).</summary>
public interface ISettingsService
{
    string SettingsFilePath { get; }
    AppSettings Current { get; }
    string? LastLoadWarning { get; }

    AppSettings Load();
    (bool Success, string Message) Save(AppSettings settings);
    void ApplyTheme(Application application, string theme);
}
