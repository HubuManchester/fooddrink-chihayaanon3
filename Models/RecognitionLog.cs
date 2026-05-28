using SQLite;
namespace TasteNote.Models;
[Table("RecognitionLog")]
public class RecognitionLog
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? ImagePath { get; set; }
    [MaxLength(50)]
    public string RecognizedLabel { get; set; } = string.Empty;
    public float Confidence { get; set; }
    [MaxLength(50)]
    public string? MappedName { get; set; }
    public bool WasSaved { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
