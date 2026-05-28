namespace TasteNote.Views.Controls;

public partial class PlaceCard : ContentView
{
    public static readonly BindableProperty PlaceNameProperty =
        BindableProperty.Create(nameof(PlaceName), typeof(string), typeof(PlaceCard), string.Empty);

    public static readonly BindableProperty AddressProperty =
        BindableProperty.Create(nameof(Address), typeof(string), typeof(PlaceCard), string.Empty);

    public static readonly BindableProperty CategoryProperty =
        BindableProperty.Create(nameof(Category), typeof(string), typeof(PlaceCard), string.Empty);

    public static readonly BindableProperty RatingProperty =
        BindableProperty.Create(nameof(Rating), typeof(int), typeof(PlaceCard), 3);

    public static readonly BindableProperty CoordinatesTextProperty =
        BindableProperty.Create(nameof(CoordinatesText), typeof(string), typeof(PlaceCard), string.Empty);

    public static readonly BindableProperty LastVisitDateProperty =
        BindableProperty.Create(nameof(LastVisitDate), typeof(string), typeof(PlaceCard), string.Empty);

    public string PlaceName
    {
        get => (string)GetValue(PlaceNameProperty);
        set => SetValue(PlaceNameProperty, value);
    }

    public string Address
    {
        get => (string)GetValue(AddressProperty);
        set => SetValue(AddressProperty, value);
    }

    public string Category
    {
        get => (string)GetValue(CategoryProperty);
        set => SetValue(CategoryProperty, value);
    }

    public int Rating
    {
        get => (int)GetValue(RatingProperty);
        set => SetValue(RatingProperty, value);
    }

    public string CoordinatesText
    {
        get => (string)GetValue(CoordinatesTextProperty);
        set => SetValue(CoordinatesTextProperty, value);
    }

    public string LastVisitDate
    {
        get => (string)GetValue(LastVisitDateProperty);
        set => SetValue(LastVisitDateProperty, value);
    }

    public PlaceCard()
    {
        InitializeComponent();
    }
}
