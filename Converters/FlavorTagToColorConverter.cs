using System.Globalization;
using Microsoft.Maui.Controls;

namespace TasteNote.Converters;

public class FlavorTagToColorConverter : IValueConverter
{
    private static readonly Color FlavorSweet = Color.FromArgb("#FFB6C1");
    private static readonly Color FlavorSour = Color.FromArgb("#FFFACD");
    private static readonly Color FlavorBitter = Color.FromArgb("#D2B48C");
    private static readonly Color FlavorSalty = Color.FromArgb("#ADD8E6");
    private static readonly Color FlavorUmami = Color.FromArgb("#90EE90");
    private static readonly Color FlavorOther = Color.FromArgb("#D3D3D3");

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var tag = value?.ToString() ?? string.Empty;

        return tag switch
        {
            "果香" => FlavorSweet,
            "蜜甜" => FlavorSweet,
            "奶油" => FlavorSweet,
            "花香" => FlavorSweet,
            "回甘" => FlavorSweet,
            "酸甜" => FlavorSour,
            "醇苦" => FlavorBitter,
            "微苦" => FlavorBitter,
            "烟熏" => FlavorBitter,
            "草本" => FlavorBitter,
            "清爽" => FlavorSalty,
            "鲜味" => FlavorUmami,
            _ => FlavorOther
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
