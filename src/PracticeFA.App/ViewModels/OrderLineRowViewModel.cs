using CommunityToolkit.Mvvm.ComponentModel;
using PracticeFA.App.Models;

namespace PracticeFA.App.ViewModels;

/// <summary>P40 — editable order line in detail grid.</summary>
public partial class OrderLineRowViewModel : ObservableObject
{
    public OrderLineRowViewModel(int lineNumber, string skuOrStyle, int quantity)
    {
        _lineNumber = lineNumber;
        _skuOrStyle = skuOrStyle;
        _quantity = quantity;
    }

    [ObservableProperty]
    private int _lineNumber;

    [ObservableProperty]
    private string _skuOrStyle;

    [ObservableProperty]
    private int _quantity;

    partial void OnQuantityChanged(int value) => QuantityChanged?.Invoke(this, EventArgs.Empty);

    public event EventHandler? QuantityChanged;

    public OrderLineInput ToInput() =>
        new() { LineNumber = LineNumber, SkuOrStyle = SkuOrStyle, Quantity = Quantity };
}
