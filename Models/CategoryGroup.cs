namespace TasteNote.Models;
public class CategoryGroup<T>
{
    public string Category { get; set; } = string.Empty;
    public List<T> Items { get; set; } = new();
}
