using System.Globalization;
using Microsoft.Maui.Controls;

namespace TasteNote.Converters;

public class StringEqualsConverter : IValueConverter
{
    public string CompareTo { get; set; } = string.Empty;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString() == CompareTo;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
