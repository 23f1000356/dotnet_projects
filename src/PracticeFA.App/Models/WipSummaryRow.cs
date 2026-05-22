namespace PracticeFA.App.Models;

public sealed class WipSummaryRow
{
    public string WorkCenter { get; init; } = "";
    public int OpenOrders { get; init; }
    public string Status { get; init; } = "";
}
