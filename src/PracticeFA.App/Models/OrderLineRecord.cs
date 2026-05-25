namespace PracticeFA.App.Models;

/// <summary>P40 — detail line from dbo.spGetOrderLines.</summary>
public sealed class OrderLineRecord
{
    public int OrderLineId { get; init; }
    public int OrderId { get; init; }
    public int LineNumber { get; init; }
    public string SkuOrStyle { get; init; } = "";
    public int Quantity { get; init; }
}
