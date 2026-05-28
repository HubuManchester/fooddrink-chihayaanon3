using TasteNote.ViewModels;

namespace TasteNote.Views;

public partial class DiscoveryPage : ContentPage
{
    public DiscoveryPage(DiscoveryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is DiscoveryViewModel vm)
        {
            vm.LoadDataCommand.Execute(null);
        }
    }
}
