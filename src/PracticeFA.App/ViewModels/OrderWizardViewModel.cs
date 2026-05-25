using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PracticeFA.App.Models;
using PracticeFA.App.Services;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.ViewModels;

/// <summary>P42 — TabControl wizard: order info → lines → confirm → spSaveOrder.</summary>
public partial class OrderWizardViewModel : ObservableObject
{
    private readonly IOrderService _orderService;

    public OrderWizardViewModel(IOrderService orderService)
    {
        _orderService = orderService;
        Lines = new ObservableCollection<OrderLineRowViewModel>();
        Lines.CollectionChanged += OnLinesCollectionChanged;
        ResetWizard();
    }

    public ObservableCollection<OrderLineRowViewModel> Lines { get; }

    [ObservableProperty]
    private int _selectedTabIndex;

    [ObservableProperty]
    private string _bagTag = "";

    [ObservableProperty]
    private string _operatorBadge = "E101";

    [ObservableProperty]
    private string _plantCode = "P01";

    [ObservableProperty]
    private string _validationMessage = "";

    [ObservableProperty]
    private string _statusMessage = "Step 1 of 3 — enter order header, then Next.";

    [ObservableProperty]
    private bool _isBusy;

    public int TotalQuantity => Lines.Sum(l => l.Quantity);

    public string ConfirmSummary =>
        $"Bag tag: {BagTag}\n" +
        $"Operator: {OperatorBadge}\n" +
        $"Plant: {PlantCode}\n" +
        $"Lines: {Lines.Count}\n" +
        $"Total quantity: {TotalQuantity}";

    public bool CanShowBack => SelectedTabIndex > 0 && !IsBusy;

    public bool CanShowNext => SelectedTabIndex < 2 && !IsBusy;

    public bool CanShowSave => SelectedTabIndex == 2 && !IsBusy;

    partial void OnSelectedTabIndexChanged(int value)
    {
        ValidationMessage = "";
        StatusMessage = value switch
        {
            0 => "Step 1 of 3 — Order information",
            1 => "Step 2 of 3 — Order lines",
            2 => "Step 3 of 3 — Review and Save",
            _ => StatusMessage,
        };
        NotifyNavButtons();
    }

    partial void OnIsBusyChanged(bool value) => NotifyNavButtons();

    private bool CanGoBack() => CanShowBack;

    private bool CanGoNext() => CanShowNext;

    private bool CanGoSave() => CanShowSave;

    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private void Back()
    {
        ValidationMessage = "";
        SelectedTabIndex--;
    }

    [RelayCommand(CanExecute = nameof(CanGoNext))]
    private void Next()
    {
        if (!ValidateCurrentTab())
            return;

        ValidationMessage = "";
        SelectedTabIndex++;
    }

    [RelayCommand]
    private void AddLine()
    {
        var next = Lines.Count == 0 ? 1 : Lines.Max(l => l.LineNumber) + 1;
        var row = new OrderLineRowViewModel(next, "STYLE-A", 1);
        SubscribeLine(row);
        Lines.Add(row);
        OnPropertyChanged(nameof(TotalQuantity));
        OnPropertyChanged(nameof(ConfirmSummary));
    }

    [RelayCommand]
    private void RemoveLine()
    {
        if (Lines.Count == 0)
            return;
        UnsubscribeLine(Lines[^1]);
        Lines.RemoveAt(Lines.Count - 1);
        OnPropertyChanged(nameof(TotalQuantity));
        OnPropertyChanged(nameof(ConfirmSummary));
    }

    [RelayCommand(CanExecute = nameof(CanGoSave))]
    private async Task SaveAsync()
    {
        if (!ValidateTab(1))
        {
            SelectedTabIndex = 1;
            return;
        }

        IsBusy = true;
        StatusMessage = "Saving order via dbo.spSaveOrder…";
        try
        {
            var inputs = Lines.Select(l => l.ToInput()).ToList();
            var result = await Task.Run(() => _orderService.Save(
                BagTag,
                OperatorBadge,
                PlantCode,
                AppState.CurrentUser?.UserId,
                inputs));

            StatusMessage = result.Message;
            if (result.Success)
                ResetWizard();
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

    [RelayCommand]
    private void StartOver() => ResetWizard();

    private void ResetWizard()
    {
        SelectedTabIndex = 0;
        BagTag = $"BAG-{DateTime.Now:yyyyMMdd-HHmm}";
        OperatorBadge = "E101";
        PlantCode = AppState.CurrentUser?.PlantCode ?? "P01";
        ValidationMessage = "";
        ReplaceLines([new OrderLineRowViewModel(1, "STYLE-A", 10), new OrderLineRowViewModel(2, "STYLE-B", 5)]);
        StatusMessage = "Step 1 of 3 — enter order header, then Next.";
    }

    private bool ValidateCurrentTab() => SelectedTabIndex switch
    {
        0 => ValidateTab(0),
        1 => ValidateTab(1),
        _ => true,
    };

    private bool ValidateTab(int tabIndex)
    {
        switch (tabIndex)
        {
            case 0:
                if (string.IsNullOrWhiteSpace(BagTag))
                {
                    ValidationMessage = "Bag tag is required.";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(OperatorBadge))
                {
                    ValidationMessage = "Operator badge is required.";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(PlantCode))
                {
                    ValidationMessage = "Plant code is required.";
                    return false;
                }
                return true;

            case 1:
                if (Lines.Count == 0)
                {
                    ValidationMessage = "Add at least one line.";
                    return false;
                }
                foreach (var line in Lines)
                {
                    if (string.IsNullOrWhiteSpace(line.SkuOrStyle))
                    {
                        ValidationMessage = $"Line {line.LineNumber}: SKU / style is required.";
                        return false;
                    }
                    if (line.Quantity <= 0)
                    {
                        ValidationMessage = $"Line {line.LineNumber}: quantity must be greater than zero.";
                        return false;
                    }
                }
                return true;

            default:
                return true;
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
        OnPropertyChanged(nameof(TotalQuantity));
        OnPropertyChanged(nameof(ConfirmSummary));
    }

    private void SubscribeLine(OrderLineRowViewModel line) =>
        line.QuantityChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(TotalQuantity));
            OnPropertyChanged(nameof(ConfirmSummary));
        };

    private void UnsubscribeLine(OrderLineRowViewModel line) =>
        line.QuantityChanged -= (_, _) => { };

    private void NotifyNavButtons()
    {
        OnPropertyChanged(nameof(CanShowBack));
        OnPropertyChanged(nameof(CanShowNext));
        OnPropertyChanged(nameof(CanShowSave));
        BackCommand.NotifyCanExecuteChanged();
        NextCommand.NotifyCanExecuteChanged();
        SaveCommand.NotifyCanExecuteChanged();
    }
}
