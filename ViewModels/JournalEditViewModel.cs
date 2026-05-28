using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.Input;
using TasteNote.Models;
using TasteNote.Services;

namespace TasteNote.ViewModels;

public partial class JournalEditViewModel : BaseViewModel
{
    private readonly TastingRepository _tastingRepository;
    private readonly SpeechToTextService _speechService;

    private string _title = string.Empty;
    public new string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private string _category = "咖啡";
    public string Category
    {
        get => _category;
        set => SetProperty(ref _category, value);
    }

    private int _rating = 3;
    public int Rating
    {
        get => _rating;
        set
        {
            SetProperty(ref _rating, value);
            OnPropertyChanged(nameof(Star1Text));
            OnPropertyChanged(nameof(Star2Text));
            OnPropertyChanged(nameof(Star3Text));
            OnPropertyChanged(nameof(Star4Text));
            OnPropertyChanged(nameof(Star5Text));
        }
    }

    private string _notes = string.Empty;
    public string Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    private string _locationName = string.Empty;
    public string LocationName
    {
        get => _locationName;
        set => SetProperty(ref _locationName, value);
    }

    private string? _imagePath;
    public string? ImagePath
    {
        get => _imagePath;
        set
        {
            SetProperty(ref _imagePath, value);
            OnPropertyChanged(nameof(HasImage));
            OnPropertyChanged(nameof(HasNoImage));
            OnPropertyChanged(nameof(ImageUrl));
        }
    }

    public ObservableCollection<string> FlavorTags { get; } = [];

    public List<string> Categories { get; } =
    [
        "咖啡", "茶饮", "鸡尾酒", "甜品", "主食", "小吃", "汤品"
    ];

    private bool _isListening;
    public bool IsListening
    {
        get => _isListening;
        set => SetProperty(ref _isListening, value);
    }

    private string _locationStatus = string.Empty;
    public string LocationStatus
    {
        get => _locationStatus;
        set
        {
            SetProperty(ref _locationStatus, value);
            OnPropertyChanged(nameof(HasLocationStatus));
        }
    }

    private bool _isEditing;
    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    private int _editingId;

    private bool _flashEnabled;
    public bool FlashEnabled
    {
        get => _flashEnabled;
        set => SetProperty(ref _flashEnabled, value);
    }

    private string _customTagText = string.Empty;
    public string CustomTagText
    {
        get => _customTagText;
        set => SetProperty(ref _customTagText, value);
    }

    public List<string> AllFlavorTags { get; } =
    [
        "果香", "花香", "蜜甜", "焦糖", "奶油", "醇苦", "微苦", "回甘",
        "烟熏", "草本", "丝滑", "清爽", "浓郁", "轻盈", "气泡感", "薄荷", "微辣", "酸甜"
    ];

    // --- XAML-bound aliases ---

    public string? ImageUrl => ImagePath;
    public bool HasImage => ImagePath != null;
    public bool HasNoImage => ImagePath == null;
    public string SelectedCategory { get => Category; set => Category = value; }
    public List<string> PresetFlavorTags => AllFlavorTags;
    public ObservableCollection<string> SelectedFlavorTags => FlavorTags;
    public bool HasLocationStatus => !string.IsNullOrWhiteSpace(LocationStatus);

    public string Star1Text => Rating >= 1 ? "⭐" : "☆";
    public string Star2Text => Rating >= 2 ? "⭐" : "☆";
    public string Star3Text => Rating >= 3 ? "⭐" : "☆";
    public string Star4Text => Rating >= 4 ? "⭐" : "☆";
    public string Star5Text => Rating >= 5 ? "⭐" : "☆";

    public JournalEditViewModel(TastingRepository tastingRepository, SpeechToTextService speechService)
    {
        _tastingRepository = tastingRepository;
        _speechService = speechService;
        Title = "新建品鉴";
    }

    [RelayCommand]
    private async Task Save()
    {
        try
        {
            var record = new TastingRecord
            {
                Title = _title,
                Category = _category,
                Rating = _rating,
                Notes = string.IsNullOrWhiteSpace(_notes) ? null : _notes,
                LocationName = string.IsNullOrWhiteSpace(_locationName) ? null : _locationName,
                ImagePath = _imagePath,
                FlavorTags = FlavorTags.Count > 0 ? JsonSerializer.Serialize(FlavorTags.ToList()) : null,
            };

            if (_isEditing && _editingId > 0)
            {
                record.Id = _editingId;
            }

            await _tastingRepository.SaveAsync(record);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Save error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task TakePhoto()
    {
        try
        {
            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo != null)
            {
                var localPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using var stream = await photo.OpenReadAsync();
                using var fileStream = File.OpenWrite(localPath);
                await stream.CopyToAsync(fileStream);
                ImagePath = localPath;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"TakePhoto error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task PickPhoto()
    {
        try
        {
            var photo = await MediaPicker.Default.PickPhotoAsync();
            if (photo != null)
            {
                ImagePath = photo.FullPath;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"PickPhoto error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task GetLocation()
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
                LocationName = $"纬度 {location.Latitude:F4} 经度 {location.Longitude:F4}";
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
            System.Diagnostics.Debug.WriteLine($"GetLocation error: {ex.Message}");
        }
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task VoiceInput()
    {
        try
        {
            IsListening = true;
            var result = await _speechService.ListenAsync();

            if (!string.IsNullOrWhiteSpace(result))
            {
                Notes = string.IsNullOrWhiteSpace(Notes) ? result : $"{Notes}\n{result}";
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"VoiceInput error: {ex.Message}");
        }
        finally
        {
            IsListening = false;
        }
    }

    [RelayCommand]
    private void SetRating(int rating)
    {
        Rating = rating;
    }

    [RelayCommand]
    private void ToggleFlash()
    {
        FlashEnabled = !FlashEnabled;
    }

    [RelayCommand]
    private void AddCustomTag()
    {
        var tag = CustomTagText?.Trim();
        if (string.IsNullOrWhiteSpace(tag)) return;
        if (!FlavorTags.Contains(tag))
            FlavorTags.Add(tag);
        CustomTagText = string.Empty;
    }

    [RelayCommand]
    private void ToggleFlavorTag(string tag)
    {
        if (FlavorTags.Contains(tag))
            FlavorTags.Remove(tag);
        else
            FlavorTags.Add(tag);
    }

    [RelayCommand]
    private async Task LoadRecord(int id)
    {
        try
        {
            var record = await _tastingRepository.GetByIdAsync(id);
            if (record == null) return;

            _editingId = record.Id;
            IsEditing = true;
            Title = record.Title;
            Category = record.Category;
            Rating = record.Rating;
            Notes = record.Notes ?? string.Empty;
            LocationName = record.LocationName ?? string.Empty;
            ImagePath = record.ImagePath;

            FlavorTags.Clear();
            if (record.FlavorTags != null)
            {
                try
                {
                    var tags = JsonSerializer.Deserialize<List<string>>(record.FlavorTags);
                    if (tags != null)
                    {
                        foreach (var tag in tags)
                            FlavorTags.Add(tag);
                    }
                }
                catch { }
            }

            base.Title = "编辑品鉴";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadRecord error: {ex.Message}");
        }
    }
}
