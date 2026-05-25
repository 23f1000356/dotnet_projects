# P20 — Async + busy overlay

**Prerequisites:** P39 DI, P40 (`010` for order headers in demo)  
**No new SQL** — uses existing `dbo.spGetOrderHeaders`

## Open

**View → Async demo (P20)** or sidebar **Async demo (P20)**

## What it teaches

| Pattern | Practice FA | Floor Assistant |
|---------|-------------|-----------------|
| Slow work off UI thread | `Task.Run` + `async/await` | `BackgroundWorker` (older) |
| UI stays clickable | Overlay blocks input only on the active screen | Same problem on long SP / SAP |
| Operator feedback | `BusyOverlay` + message | Busy text / progress in FA |
| Disable actions | `IsBusy` + `InverseBoolConverter` | Disable Save/Refresh while loading |

## Components

| File | Role |
|------|------|
| `Controls/BusyOverlay.xaml` | Semi-transparent mask + indeterminate `ProgressBar` |
| `AsyncDemoService` | 3s `Task.Delay` then `ExecSp` on background thread |
| `AsyncDemoView` | Demo page — press **Load (slow ~3s)** |
| `OrdersView` / `AttendanceView` | Reuse overlay with `BusyMessage` |

## Test

1. Open **Async demo (P20)** → **Load (slow ~3s)**
2. While loading: click sidebar buttons — shell navigates; demo page shows overlay
3. After ~3s: grid fills from `spGetOrderHeaders`
4. Open **Attendance (P05)** → **Refresh** — overlay shows “Loading attendance…”
5. Open **Orders (P40)** → **Refresh** — overlay shows “Loading order headers…”

## Mock DB

Set `PRACTICE_FA_MOCK_DB=1` — demo still delays 3s, returns mock headers.

## FA homework

Grep one FA View for `BackgroundWorker` or `async` — compare to this `Task.Run` pattern.
