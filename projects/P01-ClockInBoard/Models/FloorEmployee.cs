namespace PracticeFA.P01.Models;

public sealed class FloorEmployee
{
    public required string BadgeId { get; init; }
    public required string DisplayName { get; init; }
    public DateTime ClockedInAt { get; init; }
    public required string ClockedInBy { get; init; }

    public string ListLabel =>
        $"{DisplayName} ({BadgeId}) — since {ClockedInAt:HH:mm} — by {ClockedInBy}";
}
