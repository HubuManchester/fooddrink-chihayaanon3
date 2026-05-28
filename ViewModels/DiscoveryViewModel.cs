using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using TasteNote.Models;
using TasteNote.Services;

namespace TasteNote.ViewModels;

public partial class DiscoveryViewModel : BaseViewModel
{
    private readonly TastingRepository _tastingRepository;
    private readonly BeverageRepository _beverageRepository;
    private readonly FlavorCalculator _flavorCalculator;
    private readonly TasteProfileRepository _tasteProfileRepository;
    private readonly UserPreferenceRepository _userPreferenceRepository;
    private readonly FavoritePlaceRepository _favoritePlaceRepository;

    private FlavorSummary? _currentSummary;

    private string _greeting = string.Empty;
    public string Greeting
    {
        get => _greeting;
        set => SetProperty(ref _greeting, value);
    }

    private string _dateText = string.Empty;
    public string DateText
    {
        get => _dateText;
        set => SetProperty(ref _dateText, value);
    }

    private List<FlavorPoint> _tastePoints = [];
    public List<FlavorPoint> TastePoints
    {
        get => _tastePoints;
        set => SetProperty(ref _tastePoints, value);
    }

    private int _totalRecords;
    public int TotalRecords
    {
        get => _totalRecords;
        set => SetProperty(ref _totalRecords, value);
    }

    // --- XAML-bound properties ---

    public bool IsRefreshing
    {
        get => IsBusy;
        set => IsBusy = value;
    }

    // Flavor radar values (0-1 range for ProgressBar)
    public double Sweetness => _currentSummary?.SweetAvg / 5.0 ?? 0;
    public double Bitterness => _currentSummary?.BitterAvg / 5.0 ?? 0;
    public double Sourness => _currentSummary?.SourAvg / 5.0 ?? 0;
    public double Aroma => _currentSummary?.UmamiAvg / 5.0 ?? 0;
    public double Body => _currentSummary?.SaltyAvg / 5.0 ?? 0;

    // Flavor display text
    public string SweetnessText => (_currentSummary?.SweetAvg ?? 0).ToString("0.0");
    public string BitternessText => (_currentSummary?.BitterAvg ?? 0).ToString("0.0");
    public string SournessText => (_currentSummary?.SourAvg ?? 0).ToString("0.0");
    public string AromaText => (_currentSummary?.UmamiAvg ?? 0).ToString("0.0");
    public string BodyText => (_currentSummary?.SaltyAvg ?? 0).ToString("0.0");

    public string RadarRecordCount => $"基于{TotalRecords}条记录";

    public ObservableCollection<Beverage> Recommendations => RecommendedBeverages;
    public ObservableCollection<Beverage> RecommendedBeverages { get; } = [];

    public DiscoveryViewModel(
        TastingRepository tastingRepository,
        BeverageRepository beverageRepository,
        FlavorCalculator flavorCalculator,
        TasteProfileRepository tasteProfileRepository,
        UserPreferenceRepository userPreferenceRepository,
        FavoritePlaceRepository favoritePlaceRepository)
    {
        _tastingRepository = tastingRepository;
        _beverageRepository = beverageRepository;
        _flavorCalculator = flavorCalculator;
        _tasteProfileRepository = tasteProfileRepository;
        _userPreferenceRepository = userPreferenceRepository;
        _favoritePlaceRepository = favoritePlaceRepository;
        Title = "发现";
    }

    private void UpdateGreeting()
    {
        var hour = DateTime.Now.Hour;
        Greeting = hour switch
        {
            < 6 => "夜深了",
            < 9 => "早上好",
            < 12 => "上午好",
            < 14 => "中午好",
            < 18 => "下午好",
            < 22 => "晚上好",
            _ => "夜深了"
        };
        DateText = DateTime.Now.ToString("M月d日");
    }

    [RelayCommand]
    private async Task LoadData()
    {
        try
        {
            UpdateGreeting();

            // Load taste profile / radar data
            var summary = await _flavorCalculator.CalculateSummaryAsync();
            _currentSummary = summary;
            TastePoints = _flavorCalculator.ToFlavorPoints(summary);
            TotalRecords = summary.TotalRecords;

            // Notify computed flavor properties
            OnPropertyChanged(nameof(Sweetness));
            OnPropertyChanged(nameof(Bitterness));
            OnPropertyChanged(nameof(Sourness));
            OnPropertyChanged(nameof(Aroma));
            OnPropertyChanged(nameof(Body));
            OnPropertyChanged(nameof(SweetnessText));
            OnPropertyChanged(nameof(BitternessText));
            OnPropertyChanged(nameof(SournessText));
            OnPropertyChanged(nameof(AromaText));
            OnPropertyChanged(nameof(BodyText));
            OnPropertyChanged(nameof(RadarRecordCount));

            // Load random 3 beverage recommendations
            RecommendedBeverages.Clear();
            var recommendations = await _beverageRepository.GetRandomRecommendationsAsync(3);
            foreach (var beverage in recommendations)
            {
                RecommendedBeverages.Add(beverage);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadData error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task Refresh()
    {
        IsBusy = true;
        try
        {
            await LoadData();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task OpenCamera()
    {
        await Shell.Current.GoToAsync("camera");
    }

    [RelayCommand]
    private async Task ShakeDiscover()
    {
        try
        {
            var beverages = await _beverageRepository.GetAllAsync();
            if (beverages.Count == 0) return;

            var random = new Random();
            var beverage = beverages[random.Next(beverages.Count)];
            await Shell.Current.GoToAsync($"encyclopedia/detail?id={beverage.Id}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ShakeDiscover error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task NavigateToProfile()
    {
        await Shell.Current.GoToAsync("tasteprofile");
    }

    [RelayCommand]
    private async Task PhotoRecognize()
    {
        await Shell.Current.GoToAsync("camera");
    }

    [RelayCommand]
    private async Task NearbyShops()
    {
        await Shell.Current.GoToAsync("nearby");
    }

    [RelayCommand]
    private async Task OpenRecommendation(Beverage beverage)
    {
        if (beverage == null) return;
        await Shell.Current.GoToAsync($"encyclopedia/detail?id={beverage.Id}");
    }

    [RelayCommand]
    private void MoreFeatures()
    {
        // Placeholder for future features
    }
}
