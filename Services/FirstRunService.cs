namespace TasteNote.Services;

public class FirstRunService
{
    private readonly UserPreferenceRepository _prefRepo;

    public FirstRunService(UserPreferenceRepository prefRepo)
    {
        _prefRepo = prefRepo;
    }

    public async Task<bool> IsFirstRunAsync()
    {
        var value = await _prefRepo.GetAsync("first_run");
        return value != "completed";
    }

    public async Task MarkCompletedAsync()
    {
        await _prefRepo.SetAsync("first_run", "completed");
    }
}
