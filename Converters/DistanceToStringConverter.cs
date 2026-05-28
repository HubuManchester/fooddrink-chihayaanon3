using System.Globalization;
using Microsoft.Maui.Controls;

namespace TasteNote.Converters;

public class DistanceToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double distance)
            return $"{distance:F1} km";
        return "0.0 km";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
