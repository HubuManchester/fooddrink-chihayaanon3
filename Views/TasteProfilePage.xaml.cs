using TasteNote.ViewModels;

namespace TasteNote.Views;

public partial class TasteProfilePage : ContentPage
{
    private readonly TasteProfileViewModel _viewModel;

    public TasteProfilePage(TasteProfileViewModel viewModel)
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
