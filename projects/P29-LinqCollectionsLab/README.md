# P29 — LINQ & collections lab

**Stack:** C# · LINQ · `List<T>` · Console  
**Prerequisite:** [P00](../P00-ConsoleFloorTicket/)  
**Next:** [P30](../P30-InterfacesRepository/)

---

## What is P29? (simple)

**P00** handled **one** ticket. **P29** handles a **list of 12 tickets** and asks questions with **LINQ** — filter, sort, group — the same skills FA uses before binding a grid.

No SQL — all data is in memory in `SampleData.cs`.

---

## Why after P00?

| P00 | P29 |
|-----|-----|
| One `ProductionTicket` | `List<FloorTicketRecord>` |
| `if` validation | `.Where`, `.OrderBy`, `.GroupBy` |
| Single print | Five query demos |

---

## Folder structure

```text
projects/P29-LinqCollectionsLab/
  Models/FloorTicketRecord.cs   → PO, WC, qty, date
  SampleData.cs                 → 12 seed rows
  Program.cs                    → run queries, print results
```

---

## The five queries

| # | Syntax style | Question |
|---|--------------|----------|
| 1 | **Method** `.Where()` | All rows at **CASTING** |
| 2 | **Query** `from ... where` | Quantity **> 5** |
| 3 | **Method** `.OrderBy()` | All rows **by date** |
| 4 | **Method** `.GroupBy()` | **Count** (and total qty) per work center |
| 5 | **Chained** (bonus) | CASTING, qty > 5, **newest first** |

You must understand **both** syntax styles — FA code uses each.

### Example (method syntax)

```csharp
var castingOnly = tickets
    .Where(t => t.WorkCenterCode.Equals("CASTING", StringComparison.OrdinalIgnoreCase));
```

### Example (query syntax)

```csharp
var highQty =
    from t in tickets
    where t.Quantity > 5
    select t;
```

---

## Program workflow

```text
Load SampleData.CreateTickets() → 12 rows
  → Print all rows (header + data)
  → Run query 1 → print
  → Run query 2 → print
  → … queries 3–5
  → Press Enter to exit
```

`PrintRows` materializes with `.ToList()` and handles empty results.

---

## `FloorTicketRecord`

Same fields as P00 ticket **plus** `DateOnly Date` for sorting and reporting.

`ToString()` / display line used when printing rows.

---

## Floor Assistant mapping

| P29 | Floor Assistant |
|-----|-----------------|
| `List<T>` before SQL | In-memory filter on collections |
| `.Where` on rows | Filter grid data, ViewModels |
| `.GroupBy` | Summary reports, MIS |
| `DataTable.AsEnumerable()` | LINQ on ADO.NET results (later) |

---

## Run

```powershell
dotnet run --project projects/P29-LinqCollectionsLab/P29.Console.csproj
```

Compare output row counts with hand-counting the printed table.

---

## Acceptance checklist

- [ ] Four+ LINQ queries produce correct output
- [ ] Used both method and query syntax at least once
- [ ] No SQL in project

## FA homework

- [ ] Grep FA for `.Where(` in one ViewModel
- [ ] Grep for `from ` in one file

---

## Experiments

1. Add a row with `Quantity = 50` in `SampleData.cs` — see query 2 change.  
2. Change query 1 to filter `FSK` only.  
3. Add query 6: total quantity per plant (if you add plant field).

---

## Next

**P30** — same employee idea but **interface + repository** instead of raw lists in menu code.
