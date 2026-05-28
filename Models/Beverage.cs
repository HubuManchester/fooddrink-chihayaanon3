using SQLite;
namespace TasteNote.Models;
[Table("Beverage")]
public class Beverage
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(10)]
    public string Category { get; set; } = string.Empty;
    [MaxLength(20)]
    public string? SubCategory { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    [MaxLength(50)]
    public string? Origin { get; set; }
    public string? FlavorProfile { get; set; }
    public string? Story { get; set; }
    public string? BrewMethod { get; set; }
    public string? Tags { get; set; }
    public bool IsFavorite { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
