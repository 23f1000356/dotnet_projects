using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PracticeFA.App.Models;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.ViewModels;

/// <summary>P45 — long-running SAP-style call with IProgress, CancellationTokenSource, and busy overlay.</summary>
public partial class ErpStockViewModel : ObservableObject, IDisposable
{
    private readonly IErpService _erpService;
    private CancellationTokenSource? _cts;

    public ErpStockViewModel(IErpService erpService)
    {
        _erpService = erpService;
        StockLines = new ObservableCollection<ErpStockLine>();
    }

    public ObservableCollection<ErpStockLine> StockLines { get; }

    [ObservableProperty]
    private string _sku = "STYLE-A";

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _busyMessage = "Please wait…";

    [ObservableProperty]
    private string _statusMessage =
        "Enter SKU and Verify — mock SAP call takes ~5s. Cancel stops the background task.";

    partial void OnIsBusyChanged(bool value)
    {
        VerifyStockCommand.NotifyCanExecuteChanged();
        CancelCommand.NotifyCanExecuteChanged();
    }

    private bool CanVerify() => !IsBusy;

    private bool CanCancel() => IsBusy;

    [RelayCommand(CanExecute = nameof(CanVerify))]
    private async Task VerifyStockAsync()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        IsBusy = true;
        BusyMessage = "SAP stock check (mock)…";
        StatusMessage = "Starting SAP RFC on background thread…";
        StockLines.Clear();

        var progress = new Progress<string>(message => StatusMessage = message);

        try
        {
            var result = await _erpService.GetStockAsync(Sku, progress, _cts.Token);

            StatusMessage = result.Message;
            if (!result.Success)
                return;

            foreach (var line in result.Lines)
                StockLines.Add(line);
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "SAP call cancelled — CancellationTokenSource.Cancel().";
            StockLines.Clear();
        }
        catch (Exception ex)
        {
            StatusMessage = $"SAP call failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
            BusyMessage = "Please wait…";
            _cts?.Dispose();
            _cts = null;
        }
    }

    [RelayCommand(CanExecute = nameof(CanCancel))]
    private void Cancel() => _cts?.Cancel();

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
