namespace TasteNote.Views.Controls;

public partial class JournalCard : ContentView
{
    public static readonly BindableProperty RecordTitleProperty =
        BindableProperty.Create(nameof(RecordTitle), typeof(string), typeof(JournalCard), string.Empty);

    public static readonly BindableProperty CategoryProperty =
        BindableProperty.Create(nameof(Category), typeof(string), typeof(JournalCard), string.Empty);

    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(JournalCard), string.Empty);

    public static readonly BindableProperty RatingProperty =
        BindableProperty.Create(nameof(Rating), typeof(int), typeof(JournalCard), 3);

    public static readonly BindableProperty LocationNameProperty =
        BindableProperty.Create(nameof(LocationName), typeof(string), typeof(JournalCard), string.Empty);

    public static readonly BindableProperty CreatedDateProperty =
        BindableProperty.Create(nameof(CreatedDate), typeof(string), typeof(JournalCard), string.Empty);

    public static readonly BindableProperty IsFavoriteProperty =
        BindableProperty.Create(nameof(IsFavorite), typeof(bool), typeof(JournalCard), false);

    public static readonly BindableProperty FlavorTagsProperty =
        BindableProperty.Create(nameof(FlavorTags), typeof(string), typeof(JournalCard), string.Empty);

    public string RecordTitle
    {
        get => (string)GetValue(RecordTitleProperty);
        set => SetValue(RecordTitleProperty, value);
    }

    public string Category
    {
        get => (string)GetValue(CategoryProperty);
        set => SetValue(CategoryProperty, value);
    }

    public string ImageSource
    {
        get => (string)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public int Rating
    {
        get => (int)GetValue(RatingProperty);
        set => SetValue(RatingProperty, value);
    }

    public string LocationName
    {
        get => (string)GetValue(LocationNameProperty);
        set => SetValue(LocationNameProperty, value);
    }

    public string CreatedDate
    {
        get => (string)GetValue(CreatedDateProperty);
        set => SetValue(CreatedDateProperty, value);
    }

    public bool IsFavorite
    {
        get => (bool)GetValue(IsFavoriteProperty);
        set => SetValue(IsFavoriteProperty, value);
    }

    public string FlavorTags
    {
        get => (string)GetValue(FlavorTagsProperty);
        set => SetValue(FlavorTagsProperty, value);
    }

    public JournalCard()
    {
        InitializeComponent();
    }
}
