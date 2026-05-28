namespace TasteNote.Services;

public class SpeechToTextService
{
    public async Task<string?> ListenAsync(CancellationToken ct = default)
    {
        try
        {
            // Speech recognition requires platform-specific implementation.
            // This stub returns null gracefully so the app doesn't crash.
            // To implement: use Android SpeechRecognizer via platform-specific code,
            // or add CommunityToolkit.Maui.Media package which includes ISpeechToText.
            await Task.Delay(100, ct);
            return null;
        }
        catch (OperationCanceledException)
        {
            return null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[SpeechToTextService] {ex.Message}");
            return null;
        }
    }
}
