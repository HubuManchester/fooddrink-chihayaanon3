namespace TasteNote.Services;

public class CameraService
{
    private readonly IMediaPicker _mediaPicker;
    private bool _flashEnabled;

    public CameraService(IMediaPicker mediaPicker)
    {
        _mediaPicker = mediaPicker;
    }

    public bool FlashEnabled
    {
        get => _flashEnabled;
        set => _flashEnabled = value;
    }

    public async Task<FileResult?> TakePhotoAsync()
    {
        try
        {
            var photo = await MediaPicker.Default.CapturePhotoAsync();
            return photo;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[CameraService] 拍照失败: {ex.Message}");
            return null;
        }
    }

    public async Task<FileResult?> PickPhotoAsync()
    {
        try
        {
            var photo = await MediaPicker.Default.PickPhotoAsync();
            return photo;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[CameraService] 选择照片失败: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> SaveToFileAsync(FileResult fileResult)
    {
        try
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(fileResult.FileName)}";
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            using var stream = await fileResult.OpenReadAsync();
            using var fileStream = File.OpenWrite(filePath);
            await stream.CopyToAsync(fileStream);

            return filePath;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[CameraService] 保存文件失败: {ex.Message}");
            return null;
        }
    }
}
