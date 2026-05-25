namespace PracticeFA.App.Models;

public sealed class StyleRecord
{
    public int StyleId { get; init; }
    public string StyleCode { get; init; } = "";
    public string Description { get; init; } = "";
    public string? CreatedBy { get; init; }
    public DateTime CreatedUtc { get; init; }
    public DateTime? UpdatedUtc { get; init; }
}
