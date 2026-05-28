using System.Globalization;
using Microsoft.Maui.Controls;

namespace TasteNote.Converters;

public class RatingToStarConverter : IValueConverter
{
    public int StarIndex { get; set; }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int rating)
            return rating >= StarIndex;
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
