using Microsoft.Maui.Devices.Sensors;
using TasteNote.Models;

namespace TasteNote.Services;

public class GeolocationService
{
    public async Task<Location?> GetCurrentLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request);
            return location;
        }
        catch (FeatureNotSupportedException)
        {
            System.Diagnostics.Debug.WriteLine("[GeolocationService] 设备不支持定位功能");
            return null;
        }
        catch (PermissionException)
        {
            System.Diagnostics.Debug.WriteLine("[GeolocationService] 定位权限未授予");
            return null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[GeolocationService] 获取位置失败: {ex.Message}");
            return null;
        }
    }

    public string FormatCoordinates(double lat, double lon)
    {
        return $"纬度 {lat:F6}  经度 {lon:F6}";
    }

    public List<PlaceInfo> GetNearbyPlaces(double lat, double lon)
    {
        try
        {
            var places = new List<(string Name, string Address, string Type, double Plat, double Plon)>
            {
                ("独立咖啡实验室", "创意园区A栋1楼", "咖啡馆", 39.9120, 116.4570),
                ("隐泉茶室", "老街巷32号", "茶馆", 39.9080, 116.4620),
                ("夜色酒吧", "滨江大道88号", "酒吧", 39.9150, 116.4500),
                ("甜蜜时光甜品店", "商业街56号", "甜品店", 39.9100, 116.4650),
                ("左岸法餐厅", "湖滨路12号", "餐厅", 39.9060, 116.4580),
                ("一兰拉面", "美食广场B区", "餐厅", 39.9130, 116.4550),
                ("街角烧烤屋", "夜市C排8号", "小吃摊", 39.9110, 116.4600),
                ("龙舌兰之家", "酒吧街19号", "酒吧", 39.9090, 116.4630)
            };

            return places
                .Select(p =>
                {
                    var distance = CalculateDistance(lat, lon, p.Plat, p.Plon);
                    return new PlaceInfo
                    {
                        Name = p.Name,
                        Address = p.Address,
                        Type = p.Type,
                        Distance = Math.Round(distance, 1),
                        Location = new Location(p.Plat, p.Plon),
                        CoordinatesText = FormatCoordinates(p.Plat, p.Plon)
                    };
                })
                .OrderBy(p => p.Distance)
                .ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[GeolocationService] 获取附近地点失败: {ex.Message}");
            return new List<PlaceInfo>();
        }
    }

    public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double EarthRadiusKm = 6371.0;

        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusKm * c;
    }

    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}
