# P21 — LiveCharts hourly output dashboard

**Prerequisites:** P39 DI, Reports/MIS context  
**Package:** `LiveChartsCore.SkiaSharpView.WPF` 2.0.4  
**No SQL** — mock hourly data via `IOutputChartService`

## Open

**View → Hourly chart (P21)** or sidebar **Hourly chart (P21)**

## What it shows

- **Column chart** — units produced per hour (06:00–13:00)
- **Refresh chart** — reload from `MockOutputChartService`
- **Today total** — sum of bar values
- Plant code from signed-in user shifts mock values slightly

## Architecture

| Layer | Type |
|-------|------|
| View | `OutputChartView` — `CartesianChart` binds `Series`, `XAxes`, `YAxes` |
| ViewModel | `OutputChartViewModel` — builds `ColumnSeries<int>` from service |
| Service | `IOutputChartService` / `MockOutputChartService` |

ViewModel never references SQL — later P13 can return real aggregates from a stored procedure.

## Target framework

Project uses `net10.0-windows10.0.19041` so SkiaSharp restores correctly for LiveCharts2 (see LiveCharts issue #1772).

## FA link

Analytics / MIS screens in FA use chart controls for productivity and WIP — same binding idea: **data in VM → chart Series**.

## Test

1. Sign in as `operator1` / `pass1`
2. Open **Hourly chart (P21)** — bars appear on load
3. **Refresh chart** — totals update
4. Sign in as user on another plant — bar heights change slightly
