using CommunityToolkit.Mvvm.Input;
using TasteNote.Models;
using TasteNote.Services;

namespace TasteNote.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly UserPreferenceRepository _userPreferenceRepository;
    private readonly TastingRepository _tastingRepository;
    private readonly FavoritePlaceRepository _favoritePlaceRepository;
    private readonly FlavorCalculator _flavorCalculator;
    private readonly TextToSpeechService _ttsService;
    private readonly SpeechToTextService _speechService;
    private readonly SeedDataService _seedDataService;

    private string _userName = string.Empty;
    public string UserName
    {
        get => _userName;
        set => SetProperty(ref _userName, value);
    }

    private string _currentTheme = "System";
    public string CurrentTheme
    {
        get => _currentTheme;
        set => SetProperty(ref _currentTheme, value);
    }

    private string _currentFontSize = "Medium";
    public string CurrentFontSize
    {
        get => _currentFontSize;
        set => SetProperty(ref _currentFontSize, value);
    }

    public List<string> Themes { get; } = ["Light", "Dark", "System"];
    public List<string> FontSizes { get; } = ["Small", "Medium", "Large", "ExtraLarge"];

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

    public string AppVersion => "1.0.0";

    public ProfileViewModel(
        UserPreferenceRepository userPreferenceRepository,
        TastingRepository tastingRepository,
        FavoritePlaceRepository favoritePlaceRepository,
        FlavorCalculator flavorCalculator,
        TextToSpeechService ttsService,
        SpeechToTextService speechService,
        SeedDataService seedDataService)
    {
        _userPreferenceRepository = userPreferenceRepository;
        _tastingRepository = tastingRepository;
        _favoritePlaceRepository = favoritePlaceRepository;
        _flavorCalculator = flavorCalculator;
        _ttsService = ttsService;
        _speechService = speechService;
        _seedDataService = seedDataService;
        Title = "设置";
    }

    [RelayCommand]
    private async Task LoadSettings()
    {
        try
        {
            UserName = await _userPreferenceRepository.GetAsync("user_name", string.Empty);
            CurrentTheme = await _userPreferenceRepository.GetAsync("theme", "System");
            CurrentFontSize = await _userPreferenceRepository.GetAsync("font_size", "Medium");

            var summary = await _flavorCalculator.CalculateSummaryAsync();
            FlavorPoints = _flavorCalculator.ToFlavorPoints(summary);
            TotalRecords = summary.TotalRecords;
            SweetScore = summary.SweetAvg;
            SourScore = summary.SourAvg;
            BitterScore = summary.BitterAvg;
            SaltyScore = summary.SaltyAvg;
            UmamiScore = summary.UmamiAvg;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadSettings error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task SetTheme(string theme)
    {
        try
        {
            CurrentTheme = theme;
            await _userPreferenceRepository.SetAsync("theme", theme);
            Application.Current!.UserAppTheme = theme switch
            {
                "Light" => AppTheme.Light,
                "Dark" => AppTheme.Dark,
                _ => AppTheme.Unspecified
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"SetTheme error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task SetFontSize(string fontSize)
    {
        try
        {
            CurrentFontSize = fontSize;
            await _userPreferenceRepository.SetAsync("font_size", fontSize);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"SetFontSize error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task SaveUserName()
    {
        try
        {
            await _userPreferenceRepository.SetAsync("user_name", UserName);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"SaveUserName error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task VoiceTest()
    {
        try
        {
            var result = await _speechService.ListenAsync();
            if (!string.IsNullOrWhiteSpace(result))
                System.Diagnostics.Debug.WriteLine($"VoiceTest result: {result}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"VoiceTest error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task TtsTest()
    {
        try
        {
            await _ttsService.SpeakAsync("你好，这是品味笔记的朗读测试");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"TtsTest error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task HapticTest()
    {
        try
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"HapticTest error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task ResetData()
    {
        try
        {
            var confirm = await Application.Current!.MainPage!.DisplayAlert(
                "确认重置",
                "这将删除所有数据并恢复默认设置，确定要继续吗？",
                "确定",
                "取消");

            if (!confirm) return;

            var records = await _tastingRepository.GetAllAsync();
            foreach (var record in records)
                await _tastingRepository.DeleteAsync(record);

            var places = await _favoritePlaceRepository.GetAllAsync();
            foreach (var place in places)
                await _favoritePlaceRepository.DeleteAsync(place);

            await _userPreferenceRepository.SetAsync("user_name", string.Empty);
            await _userPreferenceRepository.SetAsync("theme", "System");
            await _userPreferenceRepository.SetAsync("font_size", "Medium");

            UserName = string.Empty;
            CurrentTheme = "System";
            CurrentFontSize = "Medium";

            await _seedDataService.InitializeAsync();

            await Application.Current.MainPage.DisplayAlert("完成", "数据已重置", "确定");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ResetData error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task NavigateNearby() => await Shell.Current.GoToAsync("nearby");

    [RelayCommand]
    private async Task NavigateProfile() => await Shell.Current.GoToAsync("tasteprofile");

    // Aliases for XAML binding
    [RelayCommand]
    private async Task NavigateTasteProfile() => await NavigateProfile();

    [RelayCommand]
    private async Task TestVoice() => await VoiceTest();

    [RelayCommand]
    private async Task TestTts() => await TtsTest();

    [RelayCommand]
    private async Task TestVibration() => await HapticTest();

    [RelayCommand]
    private async Task ShowAbout()
    {
        await Application.Current!.MainPage!.DisplayAlert(
            "关于品味笔记",
            $"品味笔记 v{AppVersion}\n记录每一次味觉体验",
            "确定");
    }
}
