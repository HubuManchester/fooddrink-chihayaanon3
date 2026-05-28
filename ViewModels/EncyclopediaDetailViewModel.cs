using System.Text.Json;
using CommunityToolkit.Mvvm.Input;
using TasteNote.Models;
using TasteNote.Services;

namespace TasteNote.ViewModels;

public partial class EncyclopediaDetailViewModel : BaseViewModel
{
    private readonly BeverageRepository _beverageRepository;
    private readonly TextToSpeechService _ttsService;

    // Parsed flavor values (0-1 range for ProgressBar)
    private double _sweetness;
    private double _bitterness;
    private double _sourness;
    private double _aroma;
    private double _body;

    private bool _isStoryExpanded;
    private bool _isSpeaking;

    private Beverage? _beverage;
    public Beverage? Beverage
    {
        get => _beverage;
        set => SetProperty(ref _beverage, value);
    }

    public bool IsSpeaking
    {
        get => _isSpeaking;
        set => SetProperty(ref _isSpeaking, value);
    }

    // --- XAML-bound properties ---

    public string? ImageUrl => Beverage?.ImagePath;
    public string Name => Beverage?.Name ?? string.Empty;
    public string Category => Beverage?.Category ?? string.Empty;
    public string Origin => Beverage?.Origin ?? string.Empty;

    public double Sweetness { get => _sweetness; set => SetProperty(ref _sweetness, value); }
    public double Bitterness { get => _bitterness; set => SetProperty(ref _bitterness, value); }
    public double Sourness { get => _sourness; set => SetProperty(ref _sourness, value); }
    public double Aroma { get => _aroma; set => SetProperty(ref _aroma, value); }
    public double Body { get => _body; set => SetProperty(ref _body, value); }

    public string SweetnessText => (_sweetness * 5).ToString("0.0");
    public string BitternessText => (_bitterness * 5).ToString("0.0");
    public string SournessText => (_sourness * 5).ToString("0.0");
    public string AromaText => (_aroma * 5).ToString("0.0");
    public string BodyText => (_body * 5).ToString("0.0");

    public string StoryButtonText => IsSpeaking ? "停止朗读" : "朗读故事";

    public string Story => Beverage?.Story ?? string.Empty;
    public int StoryMaxLines => _isStoryExpanded ? 999 : 3;
    public bool HasStory => !string.IsNullOrWhiteSpace(Beverage?.Story);
    public bool HasBrewMethod => BrewSteps.Count > 0;

    private List<FlavorPoint> _flavorPoints = [];
    public List<FlavorPoint> FlavorPoints
    {
        get => _flavorPoints;
        set => SetProperty(ref _flavorPoints, value);
    }

    private List<BrewStepModel> _brewSteps = [];
    public List<BrewStepModel> BrewSteps
    {
        get => _brewSteps;
        set => SetProperty(ref _brewSteps, value);
    }

    private List<string> _tagList = [];
    public List<string> TagList
    {
        get => _tagList;
        set => SetProperty(ref _tagList, value);
    }

    public EncyclopediaDetailViewModel(BeverageRepository beverageRepository, TextToSpeechService ttsService)
    {
        _beverageRepository = beverageRepository;
        _ttsService = ttsService;
        Title = "饮品详情";
    }

    [RelayCommand]
    private async Task Load(int id)
    {
        try
        {
            Beverage = await _beverageRepository.GetByIdAsync(id);
            if (Beverage == null) return;

            Title = Beverage.Name;

            // Parse flavor profile for radar bars
            Sweetness = 0; Bitterness = 0; Sourness = 0; Aroma = 0; Body = 0;
            FlavorPoints = [];
            if (!string.IsNullOrWhiteSpace(Beverage.FlavorProfile))
            {
                try
                {
                    using var doc = JsonDocument.Parse(Beverage.FlavorProfile);
                    var root = doc.RootElement;
                    var points = new List<FlavorPoint>();
                    string[] dims = { "sweet", "sour", "bitter", "salty", "umami" };
                    string[] labels = { "甜", "酸", "苦", "咸", "鲜" };
                    for (int i = 0; i < dims.Length; i++)
                    {
                        double score = root.TryGetProperty(dims[i], out var el) ? el.GetDouble() : 0;
                        points.Add(new FlavorPoint { Dimension = labels[i], Score = score });
                    }
                    FlavorPoints = points;

                    // Map to 0-1 range for ProgressBar
                    Sweetness = Math.Clamp(root.TryGetProperty("sweet", out var sw) ? sw.GetDouble() / 5.0 : 0, 0, 1);
                    Sourness = Math.Clamp(root.TryGetProperty("sour", out var so) ? so.GetDouble() / 5.0 : 0, 0, 1);
                    Bitterness = Math.Clamp(root.TryGetProperty("bitter", out var bi) ? bi.GetDouble() / 5.0 : 0, 0, 1);
                    Aroma = Math.Clamp(root.TryGetProperty("umami", out var um) ? um.GetDouble() / 5.0 : 0, 0, 1);
                    Body = Math.Clamp(root.TryGetProperty("salty", out var sa) ? sa.GetDouble() / 5.0 : 0, 0, 1);
                }
                catch { }
            }

            BrewSteps = [];
            if (!string.IsNullOrWhiteSpace(Beverage.BrewMethod))
            {
                try
                {
                    var steps = JsonSerializer.Deserialize<List<string>>(Beverage.BrewMethod);
                    if (steps != null)
                        BrewSteps = steps.Select((text, index) => new BrewStepModel { Index = index, Text = text }).ToList();
                }
                catch { }
            }

            TagList = [];
            if (!string.IsNullOrWhiteSpace(Beverage.Tags))
            {
                try
                {
                    var tags = JsonSerializer.Deserialize<List<string>>(Beverage.Tags);
                    if (tags != null) TagList = tags;
                }
                catch { }
            }

            // Notify computed properties
            OnPropertyChanged(nameof(ImageUrl));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(Origin));
            OnPropertyChanged(nameof(SweetnessText));
            OnPropertyChanged(nameof(BitternessText));
            OnPropertyChanged(nameof(SournessText));
            OnPropertyChanged(nameof(AromaText));
            OnPropertyChanged(nameof(BodyText));
            OnPropertyChanged(nameof(Story));
            OnPropertyChanged(nameof(HasStory));
            OnPropertyChanged(nameof(HasBrewMethod));
            OnPropertyChanged(nameof(StoryMaxLines));
            OnPropertyChanged(nameof(StoryButtonText));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Load error: {ex.Message}");
        }
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task ToggleSpeaking()
    {
        try
        {
            if (_ttsService.IsSpeaking)
            {
                _ttsService.Stop();
                IsSpeaking = false;
                OnPropertyChanged(nameof(StoryButtonText));
                return;
            }

            if (Beverage?.Story == null) return;
            IsSpeaking = true;
            OnPropertyChanged(nameof(StoryButtonText));
            await _ttsService.SpeakAsync(Beverage.Story);
            IsSpeaking = false;
            OnPropertyChanged(nameof(StoryButtonText));
        }
        catch (Exception ex)
        {
            IsSpeaking = false;
            OnPropertyChanged(nameof(StoryButtonText));
            System.Diagnostics.Debug.WriteLine($"ToggleSpeaking error: {ex.Message}");
        }
    }

    [RelayCommand]
    private void ToggleStory()
    {
        _isStoryExpanded = !_isStoryExpanded;
        OnPropertyChanged(nameof(StoryMaxLines));
    }

    // Alias for XAML compatibility
    public IAsyncRelayCommand ReadStoryCommand => ToggleSpeakingCommand;

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task ToggleFavorite()
    {
        if (Beverage == null) return;
        try
        {
            await _beverageRepository.ToggleFavoriteAsync(Beverage);
            OnPropertyChanged(nameof(Beverage));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ToggleFavorite error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task RecordTasting()
    {
        if (Beverage == null) return;
        await Shell.Current.GoToAsync($"journal/edit?name={Uri.EscapeDataString(Beverage.Name)}&category={Uri.EscapeDataString(Beverage.Category)}");
    }
}
