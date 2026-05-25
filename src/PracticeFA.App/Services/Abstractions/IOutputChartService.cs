using PracticeFA.App.Models;

namespace PracticeFA.App.Services.Abstractions;

/// <summary>P21 — hourly / daily output for MIS charts (FA: analytics SP or aggregated view).</summary>
public interface IOutputChartService
{
    IReadOnlyList<HourlyOutputPoint> GetTodayHourlyOutput(string? plantCode = null);
}
