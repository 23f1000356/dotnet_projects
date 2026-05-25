# P38 — Value converters & multi-binding

**Prerequisites:** P05  
**App:** `Assets/Converters.xaml` merged in `App.xaml`

## Converters (global resources)

| Key | Type | Use |
|-----|------|-----|
| `InverseBoolConverter` | bool → enabled | Busy disables buttons |
| `BoolToVisibilityConverter` | bool → Visible/Collapsed | INACTIVE badge (`Invert`) |
| `DecimalWeightConverter` | decimal → `1250.500 g` | Attendance Weight column |
| `DateTimeDisplayConverter` | DateTime → formatted text | Attendance clock times |
| `EmployeeLabelConverter` | MultiBinding | `E101 - Sara Chen` |

## SQL (optional weight column)

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\009_P38_AttendanceWeight.sql
```

## See P38 on screen

1. **Attendance (P05)** — Weight column (`DecimalWeight`), Clock in/out (`DateTimeDisplay`)
2. **Employee maintenance (6001)** — MultiBinding employee label, INACTIVE badge (`BoolToVisibility`), gray row (`DataTrigger` on `IsActive=False`)
3. Uncheck **Active only** → Search → inactive rows styled
