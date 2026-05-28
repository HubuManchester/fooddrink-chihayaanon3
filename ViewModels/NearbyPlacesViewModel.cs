using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using TasteNote.Models;

namespace TasteNote.ViewModels;

public partial class NearbyPlacesViewModel : BaseViewModel
{
    public ObservableCollection<PlaceInfo> Places { get; } = [];

    private Location? _currentLocation;
    public Location? CurrentLocation
    {
        get => _currentLocation;
        set => SetProperty(ref _currentLocation, value);
    }

    private string _coordinatesText = string.Empty;
    public string CoordinatesText
    {
        get => _coordinatesText;
        set => SetProperty(ref _coordinatesText, value);
    }

    private string _locationStatus = string.Empty;
    public string LocationStatus
    {
        get => _locationStatus;
        set => SetProperty(ref _locationStatus, value);
    }

    public NearbyPlacesViewModel()
    {
        Title = "附近地点";
    }

    [RelayCommand]
    private async Task GoBack() => await Shell.Current.GoToAsync("..");

    [RelayCommand]
    private async Task LoadData()
    {
        try
        {
            LocationStatus = "正在获取位置...";

            var location = await Geolocation.Default.GetLastKnownLocationAsync()
                ?? await Geolocation.Default.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(10)
                });

            if (location != null)
            {
                CurrentLocation = location;
                CoordinatesText = $"纬度 {location.Latitude:F6} 经度 {location.Longitude:F6}";
                LocationStatus = "位置已获取";

                // Load nearby places - simulate with mock data since there's no external API service
                Places.Clear();
                var mockPlaces = GetMockNearbyPlaces(location);
                foreach (var place in mockPlaces)
                {
                    Places.Add(place);
                }
            }
            else
            {
                LocationStatus = "无法获取位置";
            }
        }
        catch (Exception ex)
        {
            LocationStatus = "位置获取失败";
            System.Diagnostics.Debug.WriteLine($"LoadData error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task OpenMap(PlaceInfo place)
    {
        if (place?.Location == null) return;

        try
        {
            var options = new MapLaunchOptions { Name = place.Name };
            await Map.Default.OpenAsync(place.Location, options);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"OpenMap error: {ex.Message}");
        }
    }

    private static List<PlaceInfo> GetMockNearbyPlaces(Location center)
    {
        var random = new Random();
        var names = new[] { "街角咖啡馆", "隐茶空间", "深夜食堂", "甜品工坊", "老街小吃", "茶园小憩" };
        var types = new[] { "咖啡馆", "茶馆", "餐厅", "甜品店", "小吃店", "茶馆" };

        return names.Select((name, i) => new PlaceInfo
        {
            Name = name,
            Address = $"距离当前位置约",
            Distance = Math.Round(random.NextDouble() * 3 + 0.1, 1),
            Type = types[i],
            Location = new Location(
                center.Latitude + (random.NextDouble() - 0.5) * 0.01,
                center.Longitude + (random.NextDouble() - 0.5) * 0.01)
        }).OrderBy(p => p.Distance).ToList();
    }
}
