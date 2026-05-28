using SQLite;
namespace TasteNote.Models;
[Table("UserPreference")]
public class UserPreference
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [MaxLength(100), Unique]
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
