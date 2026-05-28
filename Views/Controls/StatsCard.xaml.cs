namespace TasteNote.Views.Controls;

public partial class StatsCard : ContentView
{
    public static readonly BindableProperty TotalCountProperty =
        BindableProperty.Create(nameof(TotalCount), typeof(int), typeof(StatsCard), 0);

    public static readonly BindableProperty MonthlyCountProperty =
        BindableProperty.Create(nameof(MonthlyCount), typeof(int), typeof(StatsCard), 0);

    public static readonly BindableProperty FavoriteCountProperty =
        BindableProperty.Create(nameof(FavoriteCount), typeof(int), typeof(StatsCard), 0);

    public static readonly BindableProperty AvgRatingProperty =
        BindableProperty.Create(nameof(AvgRating), typeof(double), typeof(StatsCard), 0.0);

    public static readonly BindableProperty PlaceCountProperty =
        BindableProperty.Create(nameof(PlaceCount), typeof(int), typeof(StatsCard), 0);

    public int TotalCount
    {
        get => (int)GetValue(TotalCountProperty);
        set => SetValue(TotalCountProperty, value);
    }

    public int MonthlyCount
    {
        get => (int)GetValue(MonthlyCountProperty);
        set => SetValue(MonthlyCountProperty, value);
    }

    public int FavoriteCount
    {
        get => (int)GetValue(FavoriteCountProperty);
        set => SetValue(FavoriteCountProperty, value);
    }

    public double AvgRating
    {
        get => (double)GetValue(AvgRatingProperty);
        set => SetValue(AvgRatingProperty, value);
    }

    public int PlaceCount
    {
        get => (int)GetValue(PlaceCountProperty);
        set => SetValue(PlaceCountProperty, value);
    }

    public StatsCard()
    {
        InitializeComponent();
    }
}
