using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PracticeFA.App.Models;
using PracticeFA.App.Services;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.ViewModels;

/// <summary>P40 — master-detail: headers list drives detail lines grid.</summary>
public partial class OrdersViewModel : ObservableObject
{
    private readonly IOrderService _orderService;

    public OrdersViewModel(IOrderService orderService)
    {
        _orderService = orderService;
        Headers = new ObservableCollection<OrderHeaderSummary>();
        Lines = new ObservableCollection<OrderLineRowViewModel>();
        Lines.CollectionChanged += OnLinesCollectionChanged;
    }

    public ObservableCollection<OrderHeaderSummary> Headers { get; }

    public ObservableCollection<OrderLineRowViewModel> Lines { get; }

    [ObservableProperty]
    private OrderHeaderSummary? _selectedHeader;

    [ObservableProperty]
    private string _bagTag = "BAG-NEW";

    [ObservableProperty]
    private string _operatorBadge = "E101";

    [ObservableProperty]
    private string _plantCode = "P01";

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _busyMessage = "Please wait…";

    [ObservableProperty]
    private string _statusMessage = "Select an order or create new — P40 master-detail.";

    /// <summary>P40 — sum of detail line quantities (updates when lines change).</summary>
    public int DetailTotalQuantity => Lines.Sum(l => l.Quantity);

    public string DetailTotalDisplay =>
        SelectedHeader is null
            ? $"Draft total: {DetailTotalQuantity} pcs"
            : $"Order #{SelectedHeader.OrderId} detail total: {DetailTotalQuantity} pcs (DB had {SelectedHeader.TotalQuantity})";

    partial void OnSelectedHeaderChanged(OrderHeaderSummary? value)
    {
        if (value is null)
            return;

        BagTag = value.BagTag;
        OperatorBadge = value.OperatorBadge;
        PlantCode = value.PlantCode;
        _ = LoadLinesForSelectedAsync();
    }

    partial void OnIsBusyChanged(bool value)
    {
        RefreshHeadersCommand.NotifyCanExecuteChanged();
        NewOrderCommand.NotifyCanExecuteChanged();
        AddLineCommand.NotifyCanExecuteChanged();
        RemoveLineCommand.NotifyCanExecuteChanged();
        SaveOrderCommand.NotifyCanExecuteChanged();
    }

    private bool CanRun() => !IsBusy;

    [RelayCommand(CanExecute = nameof(CanRun))]
    private async Task RefreshHeadersAsync()
    {
        IsBusy = true;
        BusyMessage = "Loading order headers…";
        StatusMessage = "Loading headers…";
        try
        {
            var headers = await Task.Run(_orderService.GetHeaders);
            Headers.Clear();
            foreach (var h in headers)
                Headers.Add(h);
            StatusMessage = $"{Headers.Count} order(s) — dbo.spGetOrderHeaders";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to load headers: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LoadLinesForSelectedAsync()
    {
        if (SelectedHeader is null)
            return;

        IsBusy = true;
        BusyMessage = "Loading order lines…";
        try
        {
            var lines = await Task.Run(() => _orderService.GetLines(SelectedHeader.OrderId));
            ReplaceLines(lines.Select(l => new OrderLineRowViewModel(l.LineNumber, l.SkuOrStyle, l.Quantity)));
            StatusMessage = $"Loaded {Lines.Count} line(s) for order #{SelectedHeader.OrderId}";
            OnPropertyChanged(nameof(DetailTotalDisplay));
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to load lines: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanRun))]
    private void NewOrder()
    {
        SelectedHeader = null;
        BagTag = $"BAG-{DateTime.Now:HHmmss}";
        OperatorBadge = "E101";
        PlantCode = AppState.CurrentUser?.PlantCode ?? "P01";
        ReplaceLines([new OrderLineRowViewModel(1, "STYLE-A", 10)]);
        StatusMessage = "New order draft — edit header + lines, then Save (dbo.spSaveOrder).";
        OnPropertyChanged(nameof(DetailTotalDisplay));
    }

    [RelayCommand(CanExecute = nameof(CanRun))]
    private void AddLine()
    {
        var next = Lines.Count == 0 ? 1 : Lines.Max(l => l.LineNumber) + 1;
        var row = new OrderLineRowViewModel(next, "", 1);
        SubscribeLine(row);
        Lines.Add(row);
        OnPropertyChanged(nameof(DetailTotalQuantity));
        OnPropertyChanged(nameof(DetailTotalDisplay));
    }

    [RelayCommand(CanExecute = nameof(CanRun))]
    private void RemoveLine()
    {
        if (Lines.Count == 0)
            return;
        UnsubscribeLine(Lines[^1]);
        Lines.RemoveAt(Lines.Count - 1);
        OnPropertyChanged(nameof(DetailTotalQuantity));
        OnPropertyChanged(nameof(DetailTotalDisplay));
    }

    [RelayCommand(CanExecute = nameof(CanRun))]
    private async Task SaveOrderAsync()
    {
        IsBusy = true;
        BusyMessage = "Saving order…";
        StatusMessage = "Saving order…";
        try
        {
            var inputs = Lines.Select(l => l.ToInput()).ToList();
            var createdBy = AppState.CurrentUser?.UserId;
            var result = await Task.Run(() => _orderService.Save(
                BagTag, OperatorBadge, PlantCode, createdBy, inputs));

            StatusMessage = result.Message;
            if (!result.Success)
                return;

            await RefreshHeadersAsync();
            if (result.OrderId is int id)
                SelectedHeader = Headers.FirstOrDefault(h => h.OrderId == id);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Save failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ReplaceLines(IEnumerable<OrderLineRowViewModel> rows)
    {
        foreach (var line in Lines)
            UnsubscribeLine(line);
        Lines.Clear();
        foreach (var row in rows)
        {
            SubscribeLine(row);
            Lines.Add(row);
        }
    }

    private void OnLinesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(DetailTotalQuantity));
        OnPropertyChanged(nameof(DetailTotalDisplay));
    }

    private void SubscribeLine(OrderLineRowViewModel line) =>
        line.QuantityChanged += OnLineQuantityChanged;

    private void UnsubscribeLine(OrderLineRowViewModel line) =>
        line.QuantityChanged -= OnLineQuantityChanged;

    private void OnLineQuantityChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(DetailTotalQuantity));
        OnPropertyChanged(nameof(DetailTotalDisplay));
    }
}
