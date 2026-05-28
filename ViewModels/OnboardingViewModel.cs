using CommunityToolkit.Mvvm.Input;
using TasteNote.Services;

namespace TasteNote.ViewModels;

public partial class OnboardingViewModel : BaseViewModel
{
    private readonly FirstRunService _firstRunService;

    private int _currentStep;
    public int CurrentStep
    {
        get => _currentStep;
        set => SetProperty(ref _currentStep, value);
    }

    public string[] Titles { get; } =
    [
        "欢迎来到品味笔记",
        "记录每一次品味",
        "发现更多美味"
    ];

    public string[] Descriptions { get; } =
    [
        "你的私人品味管理助手，帮你记录和探索美食与饮品的世界",
        "通过拍照、语音和文字，轻松记录每一次品鉴体验",
        "智能推荐、风味分析，让你的味蕾之旅更加精彩"
    ];

    public string[] Icons { get; } =
    [
        "",
        "",
        ""
    ];

    // --- XAML-bound properties ---

    public List<OnboardingStep> Steps { get; }

    public OnboardingViewModel(FirstRunService firstRunService)
    {
        _firstRunService = firstRunService;
        Title = "引导";

        Steps =
        [
            new() { Icon = Icons[0], Title = Titles[0], Description = Descriptions[0] },
            new() { Icon = Icons[1], Title = Titles[1], Description = Descriptions[1] },
            new() { Icon = Icons[2], Title = Titles[2], Description = Descriptions[2] },
        ];
    }

    [RelayCommand]
    private async Task Next()
    {
        if (CurrentStep < 2)
        {
            CurrentStep++;
        }
        else
        {
            await _firstRunService.MarkCompletedAsync();
            await Shell.Current.GoToAsync("//discover");
        }
    }

    [RelayCommand]
    private async Task Skip()
    {
        await _firstRunService.MarkCompletedAsync();
        await Shell.Current.GoToAsync("//discover");
    }

    [RelayCommand]
    private void StepChanged(int position)
    {
        CurrentStep = position;
    }
}

public class OnboardingStep
{
    public string Icon { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
