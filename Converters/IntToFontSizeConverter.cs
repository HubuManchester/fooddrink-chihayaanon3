using System.Globalization;
using Microsoft.Maui.Controls;

namespace TasteNote.Converters;

public class IntToFontSizeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int i)
            return (double)i;
        return 14.0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
