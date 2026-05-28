using TasteNote.ViewModels;

namespace TasteNote.Views;

public partial class JournalListPage : ContentPage
{
    public JournalListPage(JournalListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is JournalListViewModel vm)
        {
            vm.LoadRecordsCommand.Execute(null);
        }
    }
}
