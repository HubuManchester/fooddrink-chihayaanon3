using TasteNote.ViewModels;

namespace TasteNote.Views;

public partial class NearbyPlacesPage : ContentPage
{
    private readonly NearbyPlacesViewModel _viewModel;

    public NearbyPlacesPage(NearbyPlacesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDataCommand.ExecuteAsync(null);
    }
}
