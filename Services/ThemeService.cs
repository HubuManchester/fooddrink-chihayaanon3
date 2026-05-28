namespace TasteNote.Services;

public class ThemeService
{
    private readonly UserPreferenceRepository _prefRepo;

    public string CurrentTheme { get; private set; } = "System";
    public string CurrentFontSize { get; private set; } = "Medium";

    public ThemeService(UserPreferenceRepository prefRepo)
    {
        _prefRepo = prefRepo;
    }

    public async Task LoadSettingsAsync()
    {
        try
        {
            var theme = await _prefRepo.GetAsync("theme");
            if (!string.IsNullOrEmpty(theme))
            {
                CurrentTheme = theme;
            }

            var fontSize = await _prefRepo.GetAsync("fontSize");
            if (!string.IsNullOrEmpty(fontSize))
            {
                CurrentFontSize = fontSize;
            }

            ApplyTheme();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ThemeService] 加载设置失败: {ex.Message}");
        }
    }

    public async Task SetThemeAsync(string theme)
    {
        try
        {
            CurrentTheme = theme;
            await _prefRepo.SetAsync("theme", theme);
            ApplyTheme();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ThemeService] 设置主题失败: {ex.Message}");
        }
    }

    public async Task SetFontSizeAsync(string fontSize)
    {
        try
        {
            CurrentFontSize = fontSize;
            await _prefRepo.SetAsync("fontSize", fontSize);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ThemeService] 设置字体大小失败: {ex.Message}");
        }
    }

    public double GetFontSizeScale()
    {
        return CurrentFontSize switch
        {
            "Small" => 0.85,
            "Medium" => 1.0,
            "Large" => 1.15,
            "ExtraLarge" => 1.3,
            _ => 1.0
        };
    }

    private void ApplyTheme()
    {
        try
        {
            if (Application.Current == null) return;

            Application.Current.UserAppTheme = CurrentTheme switch
            {
                "Light" => AppTheme.Light,
                "Dark" => AppTheme.Dark,
                _ => AppTheme.Unspecified
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ThemeService] 应用主题失败: {ex.Message}");
        }
    }
}
