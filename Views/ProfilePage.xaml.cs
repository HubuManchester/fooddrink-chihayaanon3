using TasteNote.ViewModels;

namespace TasteNote.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _viewModel;

    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSettingsCommand.ExecuteAsync(null);
        UpdateButtonStyles();
    }

    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ProfileViewModel.CurrentTheme) ||
            e.PropertyName == nameof(ProfileViewModel.CurrentFontSize))
        {
            UpdateButtonStyles();
        }
    }

    private void UpdateButtonStyles()
    {
        var themeButtons = new[] { "ThemeLightButton", "ThemeDarkButton", "ThemeSystemButton" };
        var themeValues = new[] { "Light", "Dark", "System" };
        for (int i = 0; i < themeButtons.Length; i++)
        {
            if (FindByName(themeButtons[i]) is Button btn)
            {
                bool selected = _viewModel.CurrentTheme == themeValues[i];
                btn.BackgroundColor = selected
                    ? Color.FromArgb("#E67E22")
                    : Color.FromArgb("#E1E1E1");
                btn.TextColor = selected
                    ? Colors.White
                    : Color.FromArgb("#2C3E50");
            }
        }

        var fontButtons = new[] { "FontSmallButton", "FontMediumButton", "FontLargeButton", "FontExtraLargeButton" };
        var fontValues = new[] { "Small", "Medium", "Large", "ExtraLarge" };
        for (int i = 0; i < fontButtons.Length; i++)
        {
            if (FindByName(fontButtons[i]) is Button btn)
            {
                bool selected = _viewModel.CurrentFontSize == fontValues[i];
                btn.BackgroundColor = selected
                    ? Color.FromArgb("#E67E22")
                    : Color.FromArgb("#E1E1E1");
                btn.TextColor = selected
                    ? Colors.White
                    : Color.FromArgb("#2C3E50");
            }
        }
    }
}
