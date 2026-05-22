# P31 — Exceptions, file logging, JSON export

**Stack:** C# · Exceptions · File I/O · JSON  
**Prerequisites:** [P30](../P30-InterfacesRepository/)  
**Next:** **P08** SQL · **P01/P02** WPF UI

---

## What is P31? (simple)

**P31 adds production-style error handling** on top of P30:

1. **Friendly messages** for users (`ValidationException`)
2. **Detailed logs** for IT (`logs/practice-YYYY-MM-DD.txt`)
3. **JSON export/import** of employee list (`export/employees.json`)
4. **`try/catch`** around menu actions so the app does not crash silently

Same layered design as P30 — logging and JSON are **services**, not scattered in every button.

---

## What P31 adds to P30

| Feature | File | FA idea |
|---------|------|---------|
| `ValidationException` | `Exceptions/ValidationException.cs` | Clear user message vs technical fault |
| `FileLogger` | `Services/FileLogger.cs` | Debug log files |
| `JsonEmployeeStore` | `Services/JsonEmployeeStore.cs` | Export/import data |
| Menu `try/catch` | `Program.cs` | Save handler + log on failure |
| Badge rule | `InMemoryEmployeeRepository` | Must start with `E`, length ≥ 4 |

---

## Folder structure

```text
projects/P31-ExceptionsLogging/
  Exceptions/ValidationException.cs
  Models/Employee.cs
  Repositories/
    IEmployeeRepository.cs      (+ ReplaceAll for import)
    InMemoryEmployeeRepository.cs
  Services/
    EmployeeService.cs
    FileLogger.cs
    JsonEmployeeStore.cs
  Program.cs
```

---

## Layer diagram

```text
Program (try/catch per menu action)
  ├── FileLogger
  ├── JsonEmployeeStore
  └── EmployeeService
        └── IEmployeeRepository
```

---

## `ValidationException`

Thrown when badge invalid (e.g. `BAD` instead of `E105`).

`EmployeeService.TryAdd` catches it → shows **message only**, no stack trace to user.

Other errors may log full exception + rethrow or show generic message.

---

## `FileLogger`

- Creates `logs/` under app output folder
- File: `practice-2026-05-22.txt` (date-based)
- `LogInfo` / `LogError` append timestamped lines
- `LogError` includes exception + stack trace

**Run from dotnet:** logs appear under  
`bin/Debug/net10.0/logs/`

---

## `JsonEmployeeStore`

| Method | Purpose |
|--------|---------|
| `Export(employees)` | `System.Text.Json` → `export/employees.json` |
| `Import()` | Read file → `List<Employee>` |

Menu **4 Import** calls `repository.ReplaceAll(imported)` — **round-trip** test.

---

## Program workflow

```text
Outer try/catch on whole menu loop (fatal errors)
  Inner try/catch on each menu choice
    1 List
    2 Add (validation demo: badge BAD)
    3 Export JSON
    4 Import JSON
    5 Force error → logged + message shown
    0 Exit
```

`ForceError` deliberately throws to teach log file inspection.

---

## Menu guide

| # | Action | Learn |
|---|--------|-------|
| 1 | List employees | Baseline |
| 2 | Add | Try `E105` OK · `BAD` → validation |
| 3 | Export | Open `export/employees.json` |
| 4 | Import | Reload list from JSON |
| 5 | Force error | Open log file, see stack trace |
| 0 | Exit | |

---

## Floor Assistant mapping

| P31 | Floor Assistant |
|-----|-----------------|
| `try/catch` on save | `Views/*.xaml.cs` save handlers |
| User MessageBox | `MyMessageBox` |
| Log files | IT diagnostic logs |
| Validation before save | Before `ExecSP` |
| Export | Report export / file output |

---

## Run

```powershell
dotnet run --project projects/P31-ExceptionsLogging/P31.Console.csproj
```

---

## Acceptance checklist

- [ ] Badge `BAD` shows friendly error, no crash
- [ ] Menu 5 appends stack trace to log file
- [ ] Export then Import round-trips employee count
- [ ] Log path printed at startup

## FA homework

- [ ] Find `MyMessageBox` or `catch` in one FA View save handler

---

## Experiments

1. Delete `export/employees.json` → Import should fail gracefully.  
2. Add Serilog (P49) later — same idea, richer logs.  
3. Wire `FileLogger` into P30 project copy.

---

## Learning path position

```text
P30 (interfaces) → P31 (errors + files) → P08 (SQL repository) → P01/P02 (WPF)
```

Console track **complete** after P31 — move to WPF **P01** or SQL **P06** next.
