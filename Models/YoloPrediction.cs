namespace TasteNote.Models;
public class YoloPrediction
{
    public string Label { get; set; } = string.Empty;
    public float Confidence { get; set; }
    public RectF BoundingBox { get; set; }
}
