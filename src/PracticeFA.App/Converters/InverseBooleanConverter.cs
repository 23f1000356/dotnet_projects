using System.Globalization;
using System.Windows.Data;

namespace PracticeFA.App.Converters;

/// <summary>P05 — disable buttons when ViewModel IsBusy is true.</summary>
public sealed class InverseBooleanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is bool busy ? !busy : true;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is bool enabled ? !enabled : false;
}
