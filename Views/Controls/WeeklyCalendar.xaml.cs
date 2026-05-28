namespace TasteNote.Views.Controls;

public partial class WeeklyCalendar : ContentView
{
    public static readonly BindableProperty SelectedDateProperty =
        BindableProperty.Create(nameof(SelectedDate), typeof(DateTime), typeof(WeeklyCalendar), DateTime.Today, propertyChanged: OnSelectedDateChanged);

    public static readonly BindableProperty RecordsWithDatesProperty =
        BindableProperty.Create(nameof(RecordsWithDates), typeof(List<DateTime>), typeof(WeeklyCalendar), null, propertyChanged: OnRecordsWithDatesChanged);

    private readonly Button[] _dateButtons;
    private readonly VisualElement[] _dots;

    public DateTime SelectedDate
    {
        get => (DateTime)GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public List<DateTime> RecordsWithDates
    {
        get => (List<DateTime>)GetValue(RecordsWithDatesProperty);
        set => SetValue(RecordsWithDatesProperty, value);
    }

    public event EventHandler<DateTime>? DateSelected;

    public WeeklyCalendar()
    {
        InitializeComponent();
        _dateButtons = new[] { DateBtn0, DateBtn1, DateBtn2, DateBtn3, DateBtn4, DateBtn5, DateBtn6 };
        _dots = new VisualElement[] { Dot0, Dot1, Dot2, Dot3, Dot4, Dot5, Dot6 };
        UpdateWeek();
    }

    private static void OnSelectedDateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is WeeklyCalendar cal)
            cal.UpdateWeek();
    }

    private static void OnRecordsWithDatesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is WeeklyCalendar cal)
            cal.UpdateDots();
    }

    private void UpdateWeek()
    {
        var startOfWeek = SelectedDate.AddDays(-(int)SelectedDate.DayOfWeek + (int)DayOfWeek.Monday);
        if (SelectedDate.DayOfWeek == DayOfWeek.Sunday)
            startOfWeek = SelectedDate.AddDays(-6);

        for (int i = 0; i < 7; i++)
        {
            var date = startOfWeek.AddDays(i);
            _dateButtons[i].Text = date.Day.ToString();
            _dateButtons[i].CommandParameter = date;

            if (date.Date == SelectedDate.Date)
            {
                _dateButtons[i].BackgroundColor = Color.FromArgb("#E67E22");
                _dateButtons[i].TextColor = Colors.White;
            }
            else if (date.Date == DateTime.Today)
            {
                _dateButtons[i].BackgroundColor = Color.FromArgb("#FDEBD0");
                _dateButtons[i].TextColor = Color.FromArgb("#D35400");
            }
            else
            {
                _dateButtons[i].BackgroundColor = Color.FromArgb("#E1E1E1");
                _dateButtons[i].TextColor = Color.FromArgb("#2C3E50");
            }
        }

        UpdateDots();
    }

    private void UpdateDots()
    {
        var records = RecordsWithDates ?? new List<DateTime>();
        var startOfWeek = SelectedDate.AddDays(-(int)SelectedDate.DayOfWeek + (int)DayOfWeek.Monday);
        if (SelectedDate.DayOfWeek == DayOfWeek.Sunday)
            startOfWeek = SelectedDate.AddDays(-6);

        for (int i = 0; i < 7; i++)
        {
            var date = startOfWeek.AddDays(i).Date;
            _dots[i].IsVisible = records.Any(r => r.Date == date);
        }
    }

    private void OnDateClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is DateTime date)
        {
            SelectedDate = date;
            DateSelected?.Invoke(this, date);
        }
    }
}
