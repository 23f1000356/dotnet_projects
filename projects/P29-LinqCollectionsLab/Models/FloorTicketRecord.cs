namespace PracticeFA.P29.Models;

/// <summary>
/// One floor ticket row — P00 fields plus date for sorting/filtering labs.
/// </summary>
public sealed class FloorTicketRecord
{
    public required string PoNumber { get; init; }
    public required string WorkCenterCode { get; init; }
    public int Quantity { get; init; }
    public DateOnly Date { get; init; }

    public override string ToString() =>
        $"{Date:yyyy-MM-dd} | {PoNumber,-14} | {WorkCenterCode,-10} | Qty {Quantity,3}";
}
