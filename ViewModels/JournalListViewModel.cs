using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using TasteNote.Models;
using TasteNote.Services;

namespace TasteNote.ViewModels;

public partial class JournalListViewModel : BaseViewModel
{
    private readonly TastingRepository _tastingRepository;
    private readonly SpeechToTextService _speechService;

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    private string _selectedCategory = "全部";
    public string SelectedCategory
    {
        get => _selectedCategory;
        set => SetProperty(ref _selectedCategory, value);
    }

    public ObservableCollection<TastingRecord> Records { get; } = [];

    public List<string> Categories { get; } =
    [
        "全部", "咖啡", "茶饮", "鸡尾酒", "甜品", "主食", "小吃", "汤品"
    ];

    // --- XAML-bound aliases ---

    public bool IsRefreshing
    {
        get => IsBusy;
        set => IsBusy = value;
    }

    public ObservableCollection<TastingRecord> JournalRecords => Records;

    public JournalListViewModel(TastingRepository tastingRepository, SpeechToTextService speechService)
    {
        _tastingRepository = tastingRepository;
        _speechService = speechService;
        Title = "品鉴日志";
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task LoadRecords()
    {
        try
        {
            List<TastingRecord> records;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                records = await _tastingRepository.SearchAsync(SearchText);
                if (SelectedCategory != "全部")
                    records = records.Where(r => r.Category == SelectedCategory).ToList();
            }
            else
            {
                records = await _tastingRepository.GetByCategoryAsync(SelectedCategory);
            }

            Records.Clear();
            foreach (var record in records)
                Records.Add(record);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadRecords error: {ex.Message}");
        }
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task Refresh()
    {
        IsBusy = true;
        try { await LoadRecords(); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task Search() => await LoadRecords();

    [RelayCommand]
    private async Task SelectCategory(string category)
    {
        SelectedCategory = category;
        await LoadRecords();
    }

    [RelayCommand]
    private async Task DeleteRecord(TastingRecord record)
    {
        try
        {
            await _tastingRepository.DeleteAsync(record);
            Records.Remove(record);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DeleteRecord error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task GoToDetail(TastingRecord record)
    {
        if (record is null) return;
        await Shell.Current.GoToAsync($"journal/detail?id={record.Id}");
    }

    [RelayCommand]
    private async Task GoToEdit(int? id)
    {
        if (id.HasValue && id.Value > 0)
            await Shell.Current.GoToAsync($"journal/edit?id={id.Value}");
        else
            await Shell.Current.GoToAsync("journal/edit");
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task VoiceSearch()
    {
        try
        {
            var result = await _speechService.ListenAsync();
            if (!string.IsNullOrWhiteSpace(result))
            {
                SearchText = result;
                await LoadRecords();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"VoiceSearch error: {ex.Message}");
        }
    }

    [RelayCommand]
    private void LoadMore()
    {
        // Placeholder - pagination not currently implemented
    }

    [RelayCommand]
    private async Task NewRecord()
    {
        await Shell.Current.GoToAsync("journal/edit");
    }
}
