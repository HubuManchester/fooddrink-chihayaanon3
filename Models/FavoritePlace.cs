using SQLite;
namespace TasteNote.Models;
[Table("FavoritePlace")]
public class FavoritePlace
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(200)]
    public string? Address { get; set; }
    [MaxLength(10)]
    public string Category { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string CoordinatesText => $"纬度 {Latitude:F6} 经度 {Longitude:F6}";
    public int Rating { get; set; } = 3;
    [MaxLength(10)]
    public string? LastVisitDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
