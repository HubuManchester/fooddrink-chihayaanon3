namespace TasteNote.Models;
public class BrewStepModel
{
    public int Index { get; set; }
    public string Text { get; set; } = string.Empty;
    public string DisplayIndex => $"步骤{Index + 1}";
}
