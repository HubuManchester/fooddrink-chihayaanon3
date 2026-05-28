namespace TasteNote.Models;
public class PlaceInfo
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Distance { get; set; }
    public string Type { get; set; } = string.Empty;
    public string DistanceText => $"{Distance:F1} km";
    public string CoordinatesText { get; set; } = string.Empty;
    public Location? Location { get; set; }
}
