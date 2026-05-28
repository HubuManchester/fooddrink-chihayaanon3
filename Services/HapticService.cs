namespace TasteNote.Services;

public class HapticService
{
    public async Task PerformSuccessAsync()
    {
        try
        {
            if (Vibration.Default.IsSupported)
            {
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
            }
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[HapticService] 成功震动反馈失败: {ex.Message}");
        }
    }

    public async Task PerformErrorAsync()
    {
        try
        {
            if (Vibration.Default.IsSupported)
            {
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(150));
                await Task.Delay(100);
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(300));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[HapticService] 错误震动反馈失败: {ex.Message}");
        }
    }

    public async Task PerformLightAsync()
    {
        try
        {
            if (Vibration.Default.IsSupported)
            {
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(50));
            }
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[HapticService] 轻触震动反馈失败: {ex.Message}");
        }
    }
}
