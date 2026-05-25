using System.Globalization;
using System.Windows.Data;

namespace PracticeFA.App.Converters;

/// <summary>P38 — format weight as 3 decimals + "g" (FA product weight labels).</summary>
public sealed class DecimalWeightConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null or DBNull)
            return "—";

        if (!TryToDecimal(value, out var grams))
            return "—";

        return $"{grams.ToString("0.000", culture)} g";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Binding.DoNothing;

    private static bool TryToDecimal(object value, out decimal grams)
    {
        switch (value)
        {
            case decimal d:
                grams = d;
                return true;
            case double dbl:
                grams = (decimal)dbl;
                return true;
            case float f:
                grams = (decimal)f;
                return true;
            case int i:
                grams = i;
                return true;
            default:
                return decimal.TryParse(System.Convert.ToString(value, CultureInfo.InvariantCulture),
                    NumberStyles.Number, CultureInfo.InvariantCulture, out grams);
        }
    }
}
