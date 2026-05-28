using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using TasteNote.Models;
using TasteNote.Services;

namespace TasteNote.ViewModels;

public partial class MapViewModel : BaseViewModel
{
    private readonly FavoritePlaceRepository _favoritePlaceRepository;
    private readonly SensorService _sensorService;

    public ObservableCollection<FavoritePlace> Places { get; } = [];

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

    private double _compassHeading;
    public double CompassHeading
    {
        get => _compassHeading;
        set => SetProperty(ref _compassHeading, value);
    }

    private string _locationStatus = string.Empty;
    public string LocationStatus
    {
        get => _locationStatus;
        set => SetProperty(ref _locationStatus, value);
    }

    private int _placeCount;
    public int PlaceCount
    {
        get => _placeCount;
        set => SetProperty(ref _placeCount, value);
    }

    public MapViewModel(FavoritePlaceRepository favoritePlaceRepository, SensorService sensorService)
    {
        _favoritePlaceRepository = favoritePlaceRepository;
        _sensorService = sensorService;
        Title = "收藏地点";
    }

    [RelayCommand]
    private async Task LoadData()
    {
        try
        {
            var places = await _favoritePlaceRepository.GetAllAsync();
            Places.Clear();
            foreach (var place in places)
                Places.Add(place);
            PlaceCount = places.Count;

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

    public void StartCompass()
    {
        _sensorService.StartCompass(heading =>
        {
            MainThread.BeginInvokeOnMainThread(() => CompassHeading = heading);
        });
    }

    public void StopCompass()
    {
        _sensorService.StopCompass();
    }

    [RelayCommand]
    private async Task AddPlace()
    {
        try
        {
            if (CurrentLocation == null)
            {
                LocationStatus = "请先获取位置";
                return;
            }

            var place = new FavoritePlace
            {
                Name = $"收藏地点 {Places.Count + 1}",
                Latitude = CurrentLocation.Latitude,
                Longitude = CurrentLocation.Longitude,
                Category = "其他",
                CreatedAt = DateTime.Now
            };

            await _favoritePlaceRepository.SaveAsync(place);
            Places.Insert(0, place);
            PlaceCount = Places.Count;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AddPlace error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task OpenNearby() => await Shell.Current.GoToAsync("nearby");

    [RelayCommand]
    private async Task OpenProfile() => await Shell.Current.GoToAsync("tasteprofile");

    // Aliases for XAML binding
    [RelayCommand]
    private async Task NavigateNearby() => await OpenNearby();

    [RelayCommand]
    private async Task NavigateTasteProfile() => await OpenProfile();

    [RelayCommand]
    private async Task OpenMap(FavoritePlace place)
    {
        if (place == null) return;
        try
        {
            var location = new Location(place.Latitude, place.Longitude);
            var options = new MapLaunchOptions { Name = place.Name };
            await Map.Default.OpenAsync(location, options);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"OpenMap error: {ex.Message}");
        }
    }
}
