using System.Globalization;
using System.Windows.Data;

namespace PracticeFA.App.Converters;

/// <summary>P38 — MultiBinding: BadgeId + " - " + DisplayName.</summary>
public sealed class EmployeeLabelConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Length < 2)
            return "";

        var badge = values[0]?.ToString() ?? "";
        var name = values[1]?.ToString() ?? "";
        return string.IsNullOrWhiteSpace(name)
            ? badge
            : $"{badge} - {name}";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
