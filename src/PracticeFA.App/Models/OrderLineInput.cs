namespace PracticeFA.App.Models;

/// <summary>P36 — one line passed to dbo.spSaveOrder via table-valued parameter.</summary>
public sealed class OrderLineInput
{
    public int LineNumber { get; init; }
    public string SkuOrStyle { get; init; } = "";
    public int Quantity { get; init; }
}
