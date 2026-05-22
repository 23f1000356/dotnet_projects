# P31 — Exceptions, file logging, JSON export

**Prerequisites:** [P30](../P30-InterfacesRepository/)

## What this adds on top of P30

| Feature | File | FA idea |
|---------|------|---------|
| `ValidationException` | `Exceptions/ValidationException.cs` | User-friendly message vs technical error |
| File log | `Services/FileLogger.cs` | `logs/practice-{date}.txt` |
| JSON export/import | `Services/JsonEmployeeStore.cs` | Backup / transfer employee list |
| `try/catch` on menu | `Program.cs` | Like FA save handler + log |

## Run

```powershell
dotnet run --project projects/P31-ExceptionsLogging/P31.Console.csproj
```

## Try these

1. **Add** badge `BAD` → validation message (no stack trace to user).
2. **Export** (menu 3) → open `export/employees.json`.
3. **Import** (menu 4) → list reloads from file.
4. **Force error** (menu 5) → check `logs/practice-YYYY-MM-DD.txt` for stack trace.

Log and export folders are under `bin/Debug/net10.0/` when you run from dotnet.

## Next

**P08** — real SQL repository · **P01** — WPF UI
