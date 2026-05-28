using TasteNote.ViewModels;

namespace TasteNote.Views;

[QueryProperty(nameof(BeverageId), "id")]
public partial class EncyclopediaDetailPage : ContentPage
{
    private readonly EncyclopediaDetailViewModel _viewModel;

    public string BeverageId { get; set; } = string.Empty;

    public EncyclopediaDetailPage(EncyclopediaDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!string.IsNullOrEmpty(BeverageId) && int.TryParse(BeverageId, out int id))
        {
            _viewModel.LoadCommand.Execute(id);
        }
    }
}
