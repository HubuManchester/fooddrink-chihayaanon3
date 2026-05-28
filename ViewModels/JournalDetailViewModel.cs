using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.Input;
using TasteNote.Models;
using TasteNote.Services;

namespace TasteNote.ViewModels;

public partial class JournalDetailViewModel : BaseViewModel
{
    private readonly TastingRepository _tastingRepository;
    private readonly TextToSpeechService _ttsService;

    private TastingRecord? _record;
    public TastingRecord? Record
    {
        get => _record;
        set { SetProperty(ref _record, value); UpdateFlavorTags(); }
    }

    private bool _isFavorite;
    public bool IsFavorite
    {
        get => _isFavorite;
        set => SetProperty(ref _isFavorite, value);
    }

    private bool _isSpeaking;
    public bool IsSpeaking
    {
        get => _isSpeaking;
        set => SetProperty(ref _isSpeaking, value);
    }

    private List<string> _flavorTagList = [];
    public List<string> FlavorTagList
    {
        get => _flavorTagList;
        set => SetProperty(ref _flavorTagList, value);
    }

    // --- XAML-bound properties ---

    public string? ImageUrl => Record?.ImagePath;
    public string Category => Record?.Category ?? string.Empty;
    public string Notes => Record?.Notes ?? string.Empty;

    public string FavoriteIcon => IsFavorite ? "❤️" : "🤍";

    public bool Star1 => (Record?.Rating ?? 0) >= 1;
    public bool Star2 => (Record?.Rating ?? 0) >= 2;
    public bool Star3 => (Record?.Rating ?? 0) >= 3;
    public bool Star4 => (Record?.Rating ?? 0) >= 4;
    public bool Star5 => (Record?.Rating ?? 0) >= 5;

    public string Location
    {
        get
        {
            if (Record == null) return string.Empty;
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(Record.LocationName)) parts.Add(Record.LocationName);
            if (Record.Latitude.HasValue && Record.Longitude.HasValue)
                parts.Add($"{Record.Latitude.Value:F4}, {Record.Longitude.Value:F4}");
            return string.Join(" · ", parts);
        }
    }

    public List<string> FlavorTags => FlavorTagList;

    public bool HasVoiceNote => !string.IsNullOrWhiteSpace(Record?.VoiceNotePath);
    public string VoicePlayIcon => IsSpeaking ? "⏹" : "▶";
    public string VoiceDuration => string.Empty; // Placeholder - duration not available without playback

    public string TTSButtonText => IsSpeaking ? "停止朗读" : "朗读笔记";

    // Alias for XAML compatibility
    public IAsyncRelayCommand ToggleTTSCommand => ToggleSpeakingCommand;

    public JournalDetailViewModel(
        TastingRepository tastingRepository,
        TextToSpeechService ttsService)
    {
        _tastingRepository = tastingRepository;
        _ttsService = ttsService;
        Title = "品鉴详情";
    }

    private void UpdateFlavorTags()
    {
        FlavorTagList = [];
        if (Record?.FlavorTags != null)
        {
            try
            {
                var tags = JsonSerializer.Deserialize<List<string>>(Record.FlavorTags);
                if (tags != null) FlavorTagList = tags;
            }
            catch { }
        }
    }

    [RelayCommand]
    private async Task LoadRecord(int id)
    {
        try
        {
            Record = await _tastingRepository.GetByIdAsync(id);
            if (Record != null)
            {
                IsFavorite = Record.IsFavorite;
                Title = Record.Title;
            }

            // Notify computed properties
            OnPropertyChanged(nameof(ImageUrl));
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(Notes));
            OnPropertyChanged(nameof(FavoriteIcon));
            OnPropertyChanged(nameof(Star1));
            OnPropertyChanged(nameof(Star2));
            OnPropertyChanged(nameof(Star3));
            OnPropertyChanged(nameof(Star4));
            OnPropertyChanged(nameof(Star5));
            OnPropertyChanged(nameof(Location));
            OnPropertyChanged(nameof(HasVoiceNote));
            OnPropertyChanged(nameof(TTSButtonText));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadRecord error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task ToggleFavorite()
    {
        if (Record == null) return;
        try
        {
            Record.IsFavorite = !Record.IsFavorite;
            IsFavorite = Record.IsFavorite;
            await _tastingRepository.SaveAsync(Record);
            OnPropertyChanged(nameof(FavoriteIcon));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ToggleFavorite error: {ex.Message}");
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
                OnPropertyChanged(nameof(TTSButtonText));
                OnPropertyChanged(nameof(VoicePlayIcon));
                return;
            }

            if (Record?.Notes == null) return;
            IsSpeaking = true;
            OnPropertyChanged(nameof(TTSButtonText));
            OnPropertyChanged(nameof(VoicePlayIcon));
            await _ttsService.SpeakAsync(Record.Notes);
            IsSpeaking = false;
            OnPropertyChanged(nameof(TTSButtonText));
            OnPropertyChanged(nameof(VoicePlayIcon));
        }
        catch (Exception ex)
        {
            IsSpeaking = false;
            OnPropertyChanged(nameof(TTSButtonText));
            OnPropertyChanged(nameof(VoicePlayIcon));
            System.Diagnostics.Debug.WriteLine($"ToggleSpeaking error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task PlayVoice()
    {
        // Placeholder - voice playback requires platform-specific media APIs
    }

    [RelayCommand]
    private void RecordVoice()
    {
        // Placeholder - navigate to voice recording
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task Edit()
    {
        if (Record == null) return;
        await Shell.Current.GoToAsync($"journal/edit?id={Record.Id}");
    }
}
