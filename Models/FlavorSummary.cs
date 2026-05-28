namespace TasteNote.Models;
public class FlavorSummary
{
    public double SweetAvg { get; set; }
    public double SourAvg { get; set; }
    public double BitterAvg { get; set; }
    public double SaltyAvg { get; set; }
    public double UmamiAvg { get; set; }
    public int TotalRecords { get; set; }
    public string? FavoriteCategory { get; set; }
}
