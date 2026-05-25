using PracticeFA.App.Models;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>P21 — deterministic fake hourly output (06:00–13:00) for LiveCharts demo.</summary>
public sealed class MockOutputChartService : IOutputChartService
{
    private static readonly int[] BaseUnits = [42, 55, 48, 61, 58, 52, 67, 44];

    public IReadOnlyList<HourlyOutputPoint> GetTodayHourlyOutput(string? plantCode = null)
    {
        var plant = plantCode ?? AppState.CurrentUser?.PlantCode ?? "P01";
        var plantOffset = plant.GetHashCode(StringComparison.Ordinal) % 12;

        var points = new List<HourlyOutputPoint>(BaseUnits.Length);
        for (var i = 0; i < BaseUnits.Length; i++)
        {
            var hour = 6 + i;
            points.Add(new HourlyOutputPoint
            {
                HourLabel = $"{hour:00}:00",
                Units = BaseUnits[i] + plantOffset + (i % 3),
            });
        }

        return points;
    }
}
