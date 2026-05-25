namespace PracticeFA.App.Models;

/// <summary>P40 — master list row from dbo.spGetOrderHeaders.</summary>
public sealed class OrderHeaderSummary
{
    public int OrderId { get; init; }
    public string BagTag { get; init; } = "";
    public string OperatorBadge { get; init; } = "";
    public string PlantCode { get; init; } = "";
    public string? CreatedBy { get; init; }
    public DateTime CreatedUtc { get; init; }
    public int LineCount { get; init; }
    public int TotalQuantity { get; init; }

    public string DisplayLabel => $"#{OrderId} {BagTag} · {TotalQuantity} pcs";
}
