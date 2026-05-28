using System.Globalization;
using Microsoft.Maui.Controls;

namespace TasteNote.Converters;

public class CategoryToIconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString() switch
        {
            "咖啡" => "☕",
            "茶饮" => "🍵",
            "鸡尾酒" => "🍸",
            "甜品" => "🍰",
            "主食" => "🍝",
            "小吃" => "🍢",
            "汤品" => "🍲",
            _ => "🍽"
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
