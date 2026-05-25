using System.Globalization;
using System.Windows.Data;

namespace PracticeFA.App.Converters;

/// <summary>P38 — format DateTime in XAML (parameter = format string, default "g").</summary>
public sealed class DateTimeDisplayConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null or DBNull)
            return "—";

        var format = parameter as string ?? "g";
        var dt = value is DateTime d
            ? d
            : System.Convert.ToDateTime(value, culture);

        if (format is "g" or "G")
            dt = dt.ToLocalTime();

        return dt.ToString(format, culture);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Binding.DoNothing;
}
