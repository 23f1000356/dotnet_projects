using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using PracticeFA.App.Services;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.ViewModels;

/// <summary>P21 — LiveCharts bar chart bound to hourly output (FA MIS style).</summary>
public partial class OutputChartViewModel : ObservableObject
{
    private readonly IOutputChartService _chartService;

    public OutputChartViewModel(IOutputChartService chartService) => _chartService = chartService;

    [ObservableProperty]
    private ISeries[] _series = [];

    [ObservableProperty]
    private Axis[] _xAxes = [];

    [ObservableProperty]
    private Axis[] _yAxes =
    [
        new Axis
        {
            Name = "Units",
            MinLimit = 0,
        },
    ];

    [ObservableProperty]
    private int _totalUnits;

    [ObservableProperty]
    private string _statusMessage = "Press Refresh to load today's hourly output chart.";

    [RelayCommand]
    private void Refresh()
    {
        var plant = AppState.CurrentUser?.PlantCode;
        var data = _chartService.GetTodayHourlyOutput(plant);

        Series =
        [
            new ColumnSeries<int>
            {
                Name = $"Output ({plant ?? "P01"})",
                Values = data.Select(p => p.Units).ToArray(),
            },
        ];

        XAxes =
        [
            new Axis
            {
                Labels = data.Select(p => p.HourLabel).ToArray(),
                LabelsRotation = 15,
                Name = "Hour",
            },
        ];

        TotalUnits = data.Sum(p => p.Units);
        StatusMessage =
            $"{data.Count} hour(s) · {TotalUnits} total units — mock MIS data (P21 LiveCharts).";
    }
}
