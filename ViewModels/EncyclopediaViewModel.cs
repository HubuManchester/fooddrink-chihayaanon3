using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using TasteNote.Models;
using TasteNote.Services;

namespace TasteNote.ViewModels;

public partial class EncyclopediaViewModel : BaseViewModel
{
    private readonly BeverageRepository _beverageRepository;

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    // Alias for XAML binding
    public bool IsRefreshing
    {
        get => IsBusy;
        set => IsBusy = value;
    }

    private string _selectedCategory = "全部";
    public string SelectedCategory
    {
        get => _selectedCategory;
        set => SetProperty(ref _selectedCategory, value);
    }

    public ObservableCollection<Beverage> Beverages { get; } = [];

    public List<string> Categories { get; } =
    [
        "全部", "咖啡", "茶饮", "鸡尾酒", "甜品", "主食", "小吃", "汤品"
    ];

    public EncyclopediaViewModel(BeverageRepository beverageRepository)
    {
        _beverageRepository = beverageRepository;
        Title = "饮品百科";
    }

    [RelayCommand]
    private async Task LoadData()
    {
        try
        {
            List<Beverage> beverages;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                beverages = await _beverageRepository.SearchAsync(SearchText);
                if (SelectedCategory != "全部")
                {
                    beverages = beverages.Where(b => b.Category == SelectedCategory).ToList();
                }
            }
            else
            {
                beverages = await _beverageRepository.GetByCategoryAsync(SelectedCategory);
            }

            Beverages.Clear();
            foreach (var beverage in beverages)
            {
                Beverages.Add(beverage);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadData error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task Refresh()
    {
        IsBusy = true;
        try
        {
            await LoadData();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task Search()
    {
        await LoadData();
    }

    [RelayCommand]
    private async Task SelectCategory(string category)
    {
        SelectedCategory = category;
        await LoadData();
    }

    [RelayCommand]
    private async Task GoToDetail(Beverage beverage)
    {
        if (beverage is null) return;
        await Shell.Current.GoToAsync($"encyclopedia/detail?id={beverage.Id}");
    }
}
