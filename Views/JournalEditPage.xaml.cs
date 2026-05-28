using TasteNote.ViewModels;

namespace TasteNote.Views;

[QueryProperty(nameof(RecordId), "id")]
public partial class JournalEditPage : ContentPage
{
    private readonly JournalEditViewModel _viewModel;

    public string? RecordId { get; set; }

    public JournalEditPage(JournalEditViewModel viewModel)
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
