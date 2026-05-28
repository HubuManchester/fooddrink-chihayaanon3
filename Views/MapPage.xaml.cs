using TasteNote.ViewModels;

namespace TasteNote.Views;

public partial class MapPage : ContentPage
{
    private readonly MapViewModel _viewModel;

    public MapPage(MapViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDataCommand.ExecuteAsync(null);
        _viewModel.StartCompass();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.StopCompass();
    }
}
