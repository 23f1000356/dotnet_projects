# P29 — LINQ & collections lab

**Prerequisite:** [P00](../P00-ConsoleFloorTicket/)

## What this project teaches

After P00 (single ticket), P29 works with a **list** of tickets and asks questions using **LINQ** — the same style FA uses to filter grids and reports before binding.

| Query | Syntax | Question |
|-------|--------|----------|
| 1 | Method `.Where()` | Which rows are at **CASTING**? |
| 2 | Query `from ... where` | Which rows have **qty > 5**? |
| 3 | `.OrderBy()` | Show all rows **by date** |
| 4 | `.GroupBy()` | How many tickets **per work center**? |
| 5 | Chained | CASTING + qty > 5, **newest first** (bonus) |

## Run

```powershell
dotnet run --project projects/P29-LinqCollectionsLab/P29.Console.csproj
```

Or set **P29.Console** as startup project in Visual Studio → **F5**.

## Files

| File | Purpose |
|------|---------|
| `Models/FloorTicketRecord.cs` | One row (PO, WC, qty, date) |
| `SampleData.cs` | 12 hard-coded sample rows |
| `Program.cs` | Five LINQ demos + print |

## FA homework

Grep Floor Assistant for `.Where(` or `from ` in one ViewModel file.

## Next

**P30** — interfaces + repository pattern.
