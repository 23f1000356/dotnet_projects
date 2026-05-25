namespace PracticeFA.App.Models;

/// <summary>P21 — one bar on the hourly output chart (FA MIS productivity).</summary>
public sealed class HourlyOutputPoint
{
    public string HourLabel { get; init; } = "";
    public int Units { get; init; }
}
