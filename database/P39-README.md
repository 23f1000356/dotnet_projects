# P39 — Dependency injection in WPF

**Prerequisites:** P05 (MVVM)  
**Composition root:** `App.xaml.cs` (`Host.CreateApplicationBuilder`)

## Registered services

| Interface | Implementation | Lifetime |
|-----------|----------------|----------|
| `IDataAccess` | `SqlDataAccess` or `MockDataAccess` | Singleton |
| `ISettingsService` | `JsonSettingsService` | Singleton |
| `IAttendanceService` | `AttendanceServiceImpl` | Singleton |
| `AttendanceViewModel` | — | Transient |
| `SettingsViewModel` | — | Transient |

## Mock database (no SQL)

```powershell
$env:PRACTICE_FA_MOCK_DB = "1"
dotnet run --project src/PracticeFA.App/PracticeFA.App.csproj
```

Attendance shows **mock rows**; status bar shows **P39 Mock DB**.

## Rules

- Views: `AttendanceView(AttendanceViewModel vm)` — no `new AttendanceViewModel()`
- Navigation: `AppNavigation.CreatePage<AttendanceView>()`
- Legacy code: `DataAccess.ExecSp` / `SettingsService.Load` delegate to DI

## Swap IDataAccess in tests

Register `MockDataAccess` (or your own `IDataAccess`) in `ServiceRegistration.AddPracticeFaServices`.
