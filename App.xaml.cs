namespace TasteNote;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var services = IPlatformApplication.Current?.Services;
        var appShell = services!.GetRequiredService<AppShell>();
        return new Window(appShell);
    }
}

// TasteNote App - Main entry point - Final version
