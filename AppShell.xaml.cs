using TasteNote.Services;
using TasteNote.Views;

namespace TasteNote;

public partial class AppShell : Shell
{
    private readonly SeedDataService _seedDataService;
    private readonly FirstRunService _firstRunService;
    private bool _firstRunChecked = false;

    public AppShell(SeedDataService seedDataService, FirstRunService firstRunService)
    {
        InitializeComponent();
        _seedDataService = seedDataService;
        _firstRunService = firstRunService;

        RegisterRoutes();

        Navigated += OnNavigated;

        _ = InitializeAsync();
    }

    private void RegisterRoutes()
    {
        Routing.RegisterRoute("journal/detail", typeof(JournalDetailPage));
        Routing.RegisterRoute("journal/edit", typeof(JournalEditPage));
        Routing.RegisterRoute("encyclopedia/detail", typeof(EncyclopediaDetailPage));
        Routing.RegisterRoute("camera", typeof(CameraPage));
        Routing.RegisterRoute("nearby", typeof(NearbyPlacesPage));
        Routing.RegisterRoute("tasteprofile", typeof(TasteProfilePage));
        Routing.RegisterRoute("onboarding", typeof(OnboardingPage));
    }

    private async Task InitializeAsync()
    {
        try
        {
            await _seedDataService.InitializeAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Seed data init error: {ex.Message}");
        }
    }

    private async void OnNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        if (_firstRunChecked) return;
        _firstRunChecked = true;

        try
        {
            if (await _firstRunService.IsFirstRunAsync())
            {
                await Current.GoToAsync("onboarding");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"First run check error: {ex.Message}");
        }
    }
}
