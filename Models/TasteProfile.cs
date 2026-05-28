using SQLite;
namespace TasteNote.Models;
[Table("TasteProfile")]
public class TasteProfile
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [MaxLength(10), Unique]
    public string ProfileDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    public double SweetAvg { get; set; }
    public double SourAvg { get; set; }
    public double BitterAvg { get; set; }
    public double SaltyAvg { get; set; }
    public double UmamiAvg { get; set; }
    [MaxLength(10)]
    public string? FavoriteCategory { get; set; }
    public int TotalRecords { get; set; }
}
