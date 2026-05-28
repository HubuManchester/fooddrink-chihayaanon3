using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TasteNote.Services;
using TasteNote.ViewModels;
using TasteNote.Views;

namespace TasteNote;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Shell
        builder.Services.AddSingleton<AppShell>();

        // Database
        builder.Services.AddSingleton<DatabaseService>();

        // Repositories
        builder.Services.AddSingleton<TastingRepository>();
        builder.Services.AddSingleton<BeverageRepository>();
        builder.Services.AddSingleton<FavoritePlaceRepository>();
        builder.Services.AddSingleton<TasteProfileRepository>();
        builder.Services.AddSingleton<UserPreferenceRepository>();

        // Services
        builder.Services.AddSingleton<SeedDataService>();
        builder.Services.AddSingleton<TextToSpeechService>();
        builder.Services.AddSingleton<SpeechToTextService>();
        builder.Services.AddSingleton<CameraService>();
        builder.Services.AddSingleton<HapticService>();
        builder.Services.AddSingleton<SensorService>();
        builder.Services.AddSingleton<ThemeService>();
        builder.Services.AddSingleton<FlavorCalculator>();
        builder.Services.AddSingleton<FirstRunService>();
        builder.Services.AddSingleton<GeolocationService>();

        // ViewModels
        builder.Services.AddTransient<OnboardingViewModel>();
        builder.Services.AddTransient<DiscoveryViewModel>();
        builder.Services.AddTransient<JournalListViewModel>();
        builder.Services.AddTransient<JournalDetailViewModel>();
        builder.Services.AddTransient<JournalEditViewModel>();
        builder.Services.AddTransient<EncyclopediaViewModel>();
        builder.Services.AddTransient<EncyclopediaDetailViewModel>();
        builder.Services.AddTransient<MapViewModel>();
        builder.Services.AddTransient<NearbyPlacesViewModel>();
        builder.Services.AddTransient<CameraViewModel>();
        builder.Services.AddTransient<TasteProfileViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();

        // Pages
        builder.Services.AddTransient<OnboardingPage>();
        builder.Services.AddTransient<DiscoveryPage>();
        builder.Services.AddTransient<JournalListPage>();
        builder.Services.AddTransient<JournalDetailPage>();
        builder.Services.AddTransient<JournalEditPage>();
        builder.Services.AddTransient<EncyclopediaPage>();
        builder.Services.AddTransient<EncyclopediaDetailPage>();
        builder.Services.AddTransient<MapPage>();
        builder.Services.AddTransient<NearbyPlacesPage>();
        builder.Services.AddTransient<CameraPage>();
        builder.Services.AddTransient<TasteProfilePage>();
        builder.Services.AddTransient<ProfilePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
