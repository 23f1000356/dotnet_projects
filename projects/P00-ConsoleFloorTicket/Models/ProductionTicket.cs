namespace PracticeFA.P00.Models;

/// <summary>
/// One job at one work center — like a row you will later load from SQL.
/// </summary>
public sealed class ProductionTicket
{
    public required string PoNumber { get; init; }
    public required string WorkCenterCode { get; init; }
    public int Quantity { get; init; }

    public string FormatRoutingLine(string nextStep) =>
        $"{PoNumber} | {WorkCenterCode} | Qty {Quantity} | Next: {nextStep}";
}
