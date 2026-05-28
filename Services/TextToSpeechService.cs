namespace TasteNote.Services;

public class TextToSpeechService
{
    private CancellationTokenSource? _cts;

    public async Task SpeakAsync(string text)
    {
        try
        {
            Stop();

            _cts = new CancellationTokenSource();
            await TextToSpeech.Default.SpeakAsync(text, cancelToken: _cts.Token);
        }
        catch (OperationCanceledException)
        {
            // Expected when speech is cancelled
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[TextToSpeechService] 语音合成失败: {ex.Message}");
        }
    }

    public void Stop()
    {
        try
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[TextToSpeechService] 停止语音失败: {ex.Message}");
        }
    }

    public bool IsSpeaking => _cts != null && !_cts.IsCancellationRequested;
}
