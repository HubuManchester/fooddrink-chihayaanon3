using TasteNote.ViewModels;

namespace TasteNote.Views;

[QueryProperty(nameof(RecordId), "id")]
public partial class JournalDetailPage : ContentPage
{
    private readonly JournalDetailViewModel _viewModel;

    public string? RecordId { get; set; }

    public JournalDetailPage(JournalDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!string.IsNullOrEmpty(RecordId) && int.TryParse(RecordId, out int id))
        {
            _viewModel.LoadRecordCommand.Execute(id);
        }
    }
}
