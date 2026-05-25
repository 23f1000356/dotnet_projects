# P43 — Global exception handler

**Prerequisites:** P31 concepts, P39 DI  
**No SQL** — app-only

## What it does

| Event | Behavior |
|-------|----------|
| `DispatcherUnhandledException` | Log full detail → friendly MessageBox → `Handled = true` (app keeps running) |
| `AppDomain.UnhandledException` | Log (may be terminating) |
| `TaskScheduler.UnobservedTaskException` | Log only, mark observed |

## Operator vs IT

| Audience | Sees |
|----------|------|
| Operator | Short message only — **no stack trace** |
| IT | `%AppData%/PracticeFA/logs/practice-YYYY-MM-DD.txt` with user, plant, reference id, full exception |

## Message rules

| Exception | Operator message |
|-----------|------------------|
| `ValidationException` | Exception message only (business rule) |
| `SqlException` | Database error + reference id |
| Other | Unexpected error + reference id |

## Test (Help menu)

1. **Simulate validation error (P43)** — warning dialog, message only, no reference id clutter
2. **Simulate unhandled error (P43)** — generic error + reference id; open log file and find matching `[REFERENCE]`
3. **Open error log folder (P43)** — opens `logs` in Explorer
4. **About** — shows today’s log file path

## Code

- `GlobalExceptionHandler` — attach in `App.OnStartup` after DI host builds
- `FileAppLogger` / `IAppLogger` — P31-style daily file
- `ValidationException` — throw from services for user-facing validation
