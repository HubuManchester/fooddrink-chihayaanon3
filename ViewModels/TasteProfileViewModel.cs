using CommunityToolkit.Mvvm.Input;
using TasteNote.Models;
using TasteNote.Services;

namespace TasteNote.ViewModels;

public partial class TasteProfileViewModel : BaseViewModel
{
    private readonly TastingRepository _tastingRepository;
    private readonly FlavorCalculator _flavorCalculator;
    private readonly FavoritePlaceRepository _favoritePlaceRepository;

    private List<FlavorPoint> _flavorPoints = [];
    public List<FlavorPoint> FlavorPoints
    {
        get => _flavorPoints;
        set => SetProperty(ref _flavorPoints, value);
    }

    private int _totalRecords;
    public int TotalRecords
    {
        get => _totalRecords;
        set => SetProperty(ref _totalRecords, value);
    }

    private double _avgRating;
    public double AvgRating
    {
        get => _avgRating;
        set => SetProperty(ref _avgRating, value);
    }

    private int _placeCount;
    public int PlaceCount
    {
        get => _placeCount;
        set => SetProperty(ref _placeCount, value);
    }

    private string _favoriteCategory = string.Empty;
    public string FavoriteCategory
    {
        get => _favoriteCategory;
        set => SetProperty(ref _favoriteCategory, value);
    }

    private List<MonthlyStat> _monthlyStats = [];
    public List<MonthlyStat> MonthlyStats
    {
        get => _monthlyStats;
        set => SetProperty(ref _monthlyStats, value);
    }

    private List<CategoryGroup<TastingRecord>> _categoryStats = [];
    public List<CategoryGroup<TastingRecord>> CategoryStats
    {
        get => _categoryStats;
        set => SetProperty(ref _categoryStats, value);
    }

    private double _sweetScore;
    public double SweetScore
    {
        get => _sweetScore;
        set => SetProperty(ref _sweetScore, value);
    }

    private double _sourScore;
    public double SourScore
    {
        get => _sourScore;
        set => SetProperty(ref _sourScore, value);
    }

    private double _bitterScore;
    public double BitterScore
    {
        get => _bitterScore;
        set => SetProperty(ref _bitterScore, value);
    }

    private double _saltyScore;
    public double SaltyScore
    {
        get => _saltyScore;
        set => SetProperty(ref _saltyScore, value);
    }

    private double _umamiScore;
    public double UmamiScore
    {
        get => _umamiScore;
        set => SetProperty(ref _umamiScore, value);
    }

    public TasteProfileViewModel(
        TastingRepository tastingRepository,
        FlavorCalculator flavorCalculator,
        FavoritePlaceRepository favoritePlaceRepository)
    {
        _tastingRepository = tastingRepository;
        _flavorCalculator = flavorCalculator;
        _favoritePlaceRepository = favoritePlaceRepository;
        Title = "口味画像";
    }

    [RelayCommand]
    private async Task GoBack() => await Shell.Current.GoToAsync("..");

    [RelayCommand]
    private async Task LoadData()
    {
        try
        {
            // Load flavor summary and radar data
            var summary = await _flavorCalculator.CalculateSummaryAsync();
            FlavorPoints = _flavorCalculator.ToFlavorPoints(summary);
            TotalRecords = summary.TotalRecords;
            FavoriteCategory = summary.FavoriteCategory ?? "暂无";
            SweetScore = summary.SweetAvg;
            SourScore = summary.SourAvg;
            BitterScore = summary.BitterAvg;
            SaltyScore = summary.SaltyAvg;
            UmamiScore = summary.UmamiAvg;

            // Load average rating
            AvgRating = await _tastingRepository.GetAverageRatingAsync();

            // Load favorite place count
            PlaceCount = await _favoritePlaceRepository.GetCountAsync();

            // Load monthly stats
            MonthlyStats = await _flavorCalculator.GetMonthlyStatsAsync(6);

            // Load category stats
            var allRecords = await _tastingRepository.GetAllAsync();
            CategoryStats = allRecords
                .GroupBy(r => r.Category)
                .Select(g => new CategoryGroup<TastingRecord>
                {
                    Category = g.Key,
                    Items = g.OrderByDescending(r => r.CreatedAt).ToList()
                })
                .OrderByDescending(g => g.Items.Count)
                .ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadData error: {ex.Message}");
        }
    }
}
