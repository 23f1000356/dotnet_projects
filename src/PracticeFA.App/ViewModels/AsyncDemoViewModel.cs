using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PracticeFA.App.Models;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.ViewModels;

/// <summary>P20 — async SP load with IsBusy, disabled commands, and BusyMessage for overlay.</summary>
public partial class AsyncDemoViewModel : ObservableObject
{
    private readonly IAsyncDemoService _asyncDemoService;

    public AsyncDemoViewModel(IAsyncDemoService asyncDemoService)
    {
        _asyncDemoService = asyncDemoService;
        Rows = new ObservableCollection<AsyncDemoRow>();
    }

    public ObservableCollection<AsyncDemoRow> Rows { get; }

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _busyMessage = "Please wait…";

    [ObservableProperty]
    private string _statusMessage =
        "Press Load (slow) — 3 second simulated delay + dbo.spGetOrderHeaders on a background thread.";

    partial void OnIsBusyChanged(bool value) => LoadSlowCommand.NotifyCanExecuteChanged();

    private bool CanLoad() => !IsBusy;

    [RelayCommand(CanExecute = nameof(CanLoad))]
    private async Task LoadSlowAsync()
    {
        IsBusy = true;
        BusyMessage = $"Loading orders… (~{_asyncDemoService.SimulatedDelayMs / 1000}s simulated SP)";
        StatusMessage = "Working on background thread — UI should stay responsive.";
        var started = DateTime.UtcNow;

        try
        {
            var rows = await _asyncDemoService.LoadOrderHeadersSlowAsync();
            Rows.Clear();
            foreach (var row in rows)
                Rows.Add(row);

            var elapsed = (DateTime.UtcNow - started).TotalSeconds;
            StatusMessage =
                $"Loaded {Rows.Count} row(s) in {elapsed:F1}s — Task.Run + async/await (P20). " +
                "Buttons were disabled; overlay blocked input.";
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "Load cancelled.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Load failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
            BusyMessage = "Please wait…";
        }
    }
}
