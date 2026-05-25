namespace PracticeFA.App.Models;

/// <summary>P45/P14 — one stock line returned from SAP (mock).</summary>
public sealed class ErpStockLine
{
    public string Sku { get; init; } = "";
    public string Plant { get; init; } = "";
    public int Quantity { get; init; }
    public string Unit { get; init; } = "EA";
}
