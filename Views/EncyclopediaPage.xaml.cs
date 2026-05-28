using TasteNote.ViewModels;

namespace TasteNote.Views;

public partial class EncyclopediaPage : ContentPage
{
    public EncyclopediaPage(EncyclopediaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is EncyclopediaViewModel vm)
        {
            vm.LoadDataCommand.Execute(null);
        }
    }
}
