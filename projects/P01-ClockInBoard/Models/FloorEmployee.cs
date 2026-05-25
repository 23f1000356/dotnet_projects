namespace PracticeFA.P01.Models;

public sealed class FloorEmployee
{
    public required string BadgeId { get; init; }
    public required string DisplayName { get; init; }
    public DateTime ClockedInAt { get; init; }
    public required string ClockedInBy { get; init; }

    /// <summary>Wireframe B row: E101  Anil — Wax        14:30</summary>
    public string ListLabel =>
        $"{BadgeId,-6}  {DisplayName,-22}  {ClockedInAt:HH:mm}";
}
