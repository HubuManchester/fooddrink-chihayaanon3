using CommunityToolkit.Mvvm.Input;
using TasteNote.Models;
using TasteNote.Services;

namespace TasteNote.ViewModels;

public partial class CameraViewModel : BaseViewModel
{
    private readonly TastingRepository _tastingRepository;

    private string? _imagePath;
    public string? ImagePath
    {
        get => _imagePath;
        set => SetProperty(ref _imagePath, value);
    }

    private bool _isProcessing;
    public bool IsProcessing
    {
        get => _isProcessing;
        set => SetProperty(ref _isProcessing, value);
    }

    private string _recognizedLabel = string.Empty;
    public string RecognizedLabel
    {
        get => _recognizedLabel;
        set => SetProperty(ref _recognizedLabel, value);
    }

    private float _confidence;
    public float Confidence
    {
        get => _confidence;
        set => SetProperty(ref _confidence, value);
    }

    private bool _hasResult;
    public bool HasResult
    {
        get => _hasResult;
        set => SetProperty(ref _hasResult, value);
    }

    private string _mappedName = string.Empty;
    public string MappedName
    {
        get => _mappedName;
        set => SetProperty(ref _mappedName, value);
    }

    private bool _flashEnabled;
    public bool FlashEnabled
    {
        get => _flashEnabled;
        set => SetProperty(ref _flashEnabled, value);
    }

    // Alias for XAML binding
    public string? CapturedImage
    {
        get => ImagePath;
        set => ImagePath = value;
    }

    public CameraViewModel(TastingRepository tastingRepository)
    {
        _tastingRepository = tastingRepository;
        Title = "拍照识别";
    }

    [RelayCommand]
    private async Task GoBack() => await Shell.Current.GoToAsync("..");

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
    private void ToggleFlash()
    {
        FlashEnabled = !FlashEnabled;
    }

    [RelayCommand]
    private async Task Recognize()
    {
        if (string.IsNullOrWhiteSpace(ImagePath)) return;

        try
        {
            IsProcessing = true;

            // Simulate food recognition since YOLO model may not exist
            // In a real app, this would use ONNX/SensorService for inference
            await Task.Delay(1500); // Simulate processing time

            var mockResults = new Dictionary<string, string>
            {
                { "咖啡", "拿铁" },
                { "茶饮", "乌龙茶" },
                { "甜品", "提拉米苏" },
                { "鸡尾酒", "莫吉托" },
                { "小吃", "薯条" },
            };

            var random = new Random();
            var index = random.Next(mockResults.Count);
            var kvp = mockResults.ElementAt(index);

            RecognizedLabel = kvp.Key;
            MappedName = kvp.Value;
            Confidence = (float)Math.Round(random.NextDouble() * 0.3 + 0.7, 2);
            HasResult = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Recognize error: {ex.Message}");
            HasResult = false;
        }
        finally
        {
            IsProcessing = false;
        }
    }

    // Alias for XAML binding
    [RelayCommand]
    private async Task SaveRecord() => await SaveAsRecord();

    // Alias for XAML binding
    [RelayCommand]
    private async Task RecognizeAgain()
    {
        HasResult = false;
        RecognizedLabel = string.Empty;
        MappedName = string.Empty;
        Confidence = 0;
        await Recognize();
    }

    [RelayCommand]
    private async Task SaveAsRecord()
    {
        try
        {
            var record = new TastingRecord
            {
                Title = MappedName,
                Category = RecognizedLabel,
                ImagePath = ImagePath,
                Rating = 3,
                CreatedAt = DateTime.Now
            };

            await _tastingRepository.SaveAsync(record);
            await Shell.Current.GoToAsync($"journal/edit?id={record.Id}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"SaveAsRecord error: {ex.Message}");
        }
    }
}
