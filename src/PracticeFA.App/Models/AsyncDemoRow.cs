namespace PracticeFA.App.Models;

/// <summary>P20 — row shown after simulated slow stored-procedure load.</summary>
public sealed class AsyncDemoRow
{
    public int OrderId { get; init; }
    public string BagTag { get; init; } = "";
    public int TotalQuantity { get; init; }
}
