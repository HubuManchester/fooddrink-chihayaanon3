using SQLite;
namespace TasteNote.Models;
[Table("TastingRecord")]
public class TastingRecord
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(10)]
    public string Category { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public int Rating { get; set; } = 3;
    public string? FlavorTags { get; set; }
    public string? Notes { get; set; }
    public string? VoiceNotePath { get; set; }
    public string? VoiceNoteText { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    [MaxLength(100)]
    public string? LocationName { get; set; }
    public bool IsFavorite { get; set; }
    public bool IsCustom { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
