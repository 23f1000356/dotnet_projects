using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PracticeFA.App.Models;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.ViewModels;

/// <summary>P05b + P39 — ISettingsService injected from composition root.</summary>
public partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        ThemeOptions = new ObservableCollection<string>(AppThemeNames.All);
        LoadFromService();
    }

    public ObservableCollection<string> ThemeOptions { get; }

    public string SettingsFilePath => _settingsService.SettingsFilePath;

    [ObservableProperty]
    private string _plantCode = "P01";

    [ObservableProperty]
    private string _defaultPrinter = "";

    [ObservableProperty]
    private string _selectedTheme = AppThemeNames.Light;

    [ObservableProperty]
    private string _statusMessage = "";

    [ObservableProperty]
    private bool _isBusy;

    partial void OnIsBusyChanged(bool value)
    {
        SaveCommand.NotifyCanExecuteChanged();
        ReloadCommand.NotifyCanExecuteChanged();
    }

    private bool CanRun() => !IsBusy;

    [RelayCommand(CanExecute = nameof(CanRun))]
    private void Reload()
    {
        LoadFromService();
        StatusMessage = string.IsNullOrEmpty(_settingsService.LastLoadWarning)
            ? "Reloaded settings from disk."
            : _settingsService.LastLoadWarning;
    }

    [RelayCommand(CanExecute = nameof(CanRun))]
    private void Save()
    {
        IsBusy = true;
        try
        {
            var settings = new AppSettings
            {
                PlantCode = PlantCode,
                DefaultPrinter = DefaultPrinter,
                Theme = SelectedTheme,
            };

            var (success, message) = _settingsService.Save(settings);
            StatusMessage = message;

            if (success)
                StatusMessage += "\n\nRestart the app to confirm plant and theme load from JSON on startup.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void LoadFromService()
    {
        var settings = _settingsService.Current;
        PlantCode = settings.PlantCode;
        DefaultPrinter = settings.DefaultPrinter;
        SelectedTheme = settings.Theme;

        StatusMessage = string.IsNullOrEmpty(_settingsService.LastLoadWarning)
            ? $"Loaded from {SettingsFilePath}"
            : _settingsService.LastLoadWarning;
    }
}
