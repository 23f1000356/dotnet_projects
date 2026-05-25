# P05 — Attendance list (pure MVVM)

**Prerequisites:** P08 (`002`)  
**NuGet:** `CommunityToolkit.Mvvm`  
**App:** `ViewModels/AttendanceViewModel.cs` · `Views/AttendanceView.xaml` (empty code-behind except DataContext)

## Run script

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\008_P05_Attendance.sql
```

## Open screen

Sign in → sidebar **Attendance (P05)** or menu **View → Attendance (P05)**

## MVVM rules (this screen)

| OK | Not OK |
|----|--------|
| `Command="{Binding RefreshCommand}"` | `Click="Refresh_Click"` in AttendanceView |
| SQL in `AttendanceService` | SQL in `AttendanceView.xaml.cs` |
| `IsBusy` disables buttons via `InverseBooleanConverter` | Business logic in code-behind |

## Test

1. **Refresh** — loads sample rows (E101, E102)
2. Select a row → edit fields → **Save**
3. **New row** → badge `E103`, clock-in `07:00` → **Save** → appears in grid
4. While loading, buttons disabled (`Busy: True`)

## SSMS

```sql
EXEC dbo.spGetAttendance;
```
