namespace TasteNote.Views.Controls;

public partial class TasteRadarCard : ContentView
{
    public static readonly BindableProperty SweetScoreProperty =
        BindableProperty.Create(nameof(SweetScore), typeof(double), typeof(TasteRadarCard), 0.0);

    public static readonly BindableProperty SourScoreProperty =
        BindableProperty.Create(nameof(SourScore), typeof(double), typeof(TasteRadarCard), 0.0);

    public static readonly BindableProperty BitterScoreProperty =
        BindableProperty.Create(nameof(BitterScore), typeof(double), typeof(TasteRadarCard), 0.0);

    public static readonly BindableProperty SaltyScoreProperty =
        BindableProperty.Create(nameof(SaltyScore), typeof(double), typeof(TasteRadarCard), 0.0);

    public static readonly BindableProperty UmamiScoreProperty =
        BindableProperty.Create(nameof(UmamiScore), typeof(double), typeof(TasteRadarCard), 0.0);

    public static readonly BindableProperty TotalRecordsProperty =
        BindableProperty.Create(nameof(TotalRecords), typeof(int), typeof(TasteRadarCard), 0);

    public double SweetScore
    {
        get => (double)GetValue(SweetScoreProperty);
        set => SetValue(SweetScoreProperty, value);
    }

    public double SourScore
    {
        get => (double)GetValue(SourScoreProperty);
        set => SetValue(SourScoreProperty, value);
    }

    public double BitterScore
    {
        get => (double)GetValue(BitterScoreProperty);
        set => SetValue(BitterScoreProperty, value);
    }

    public double SaltyScore
    {
        get => (double)GetValue(SaltyScoreProperty);
        set => SetValue(SaltyScoreProperty, value);
    }

    public double UmamiScore
    {
        get => (double)GetValue(UmamiScoreProperty);
        set => SetValue(UmamiScoreProperty, value);
    }

    public int TotalRecords
    {
        get => (int)GetValue(TotalRecordsProperty);
        set => SetValue(TotalRecordsProperty, value);
    }

    public TasteRadarCard()
    {
        InitializeComponent();
    }
}
