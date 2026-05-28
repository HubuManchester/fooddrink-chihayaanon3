namespace TasteNote.Views.Controls;

public partial class BeverageCard : ContentView
{
    public static readonly BindableProperty BeverageNameProperty =
        BindableProperty.Create(nameof(BeverageName), typeof(string), typeof(BeverageCard), string.Empty);

    public static readonly BindableProperty CategoryProperty =
        BindableProperty.Create(nameof(Category), typeof(string), typeof(BeverageCard), string.Empty);

    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(BeverageCard), string.Empty);

    public static readonly BindableProperty OriginProperty =
        BindableProperty.Create(nameof(Origin), typeof(string), typeof(BeverageCard), string.Empty);

    public static readonly BindableProperty IsFavoriteProperty =
        BindableProperty.Create(nameof(IsFavorite), typeof(bool), typeof(BeverageCard), false);

    public static readonly BindableProperty TagsProperty =
        BindableProperty.Create(nameof(Tags), typeof(string), typeof(BeverageCard), string.Empty);

    public string BeverageName
    {
        get => (string)GetValue(BeverageNameProperty);
        set => SetValue(BeverageNameProperty, value);
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

    public string Origin
    {
        get => (string)GetValue(OriginProperty);
        set => SetValue(OriginProperty, value);
    }

    public bool IsFavorite
    {
        get => (bool)GetValue(IsFavoriteProperty);
        set => SetValue(IsFavoriteProperty, value);
    }

    public string Tags
    {
        get => (string)GetValue(TagsProperty);
        set => SetValue(TagsProperty, value);
    }

    public BeverageCard()
    {
        InitializeComponent();
    }
}
