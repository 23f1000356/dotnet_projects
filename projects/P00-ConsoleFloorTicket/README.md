# P00 — Console floor ticket

**Stack:** C# · Console · No WPF · No SQL  
**Time:** 3–5 days · **Next:** [P29](../P29-LinqCollectionsLab/) or [P01](../P01-ClockInBoard/)

---

## What is P00? (simple)

**P00 is your first C# program** — no windows, no database, no WPF.

On the factory floor:

- **Production order (PO)** = job number (e.g. `PO-2026-001`)
- **Work center** = stage (CASTING, FSK, GRINDING, …)

The app asks for PO, work center, and quantity, then prints:

```text
PO-2026-001 | CASTING | Qty 12 | Next: GRINDING
```

Same **idea** as Floor Assistant routing — terminal only, no SQL or screens.

---

## Web → C# mapping

| You know (web) | P00 teaches |
|----------------|-------------|
| `const job = { po, wc, qty }` | `class ProductionTicket` |
| `if (!qty) return alert(...)` | `TryBuildTicket` → `true`/`false` |
| `array.find()` | `RoutingTable.Find(code)` |
| `console.log` | `Console.WriteLine` |

---

## Folder structure

```text
projects/P00-ConsoleFloorTicket/
  P00.Console.csproj     → console exe project
  Program.cs             → entry: prompts, validation, print
  RoutingTable.cs        → list of work centers + lookup
  Models/
    WorkCenter.cs        → one stage (code, name, next)
    ProductionTicket.cs  → one job at one stage
```

---

## Startup workflow

```text
Program.cs runs (top to bottom)
  → Print banner + table of work centers (RoutingTable.PrintAvailableCodes)
  → ReadRequired: PO number (loop until not empty)
  → ReadRequired: work center code
  → ReadQuantity: int > 0 only
  → TryBuildTicket(...)
       ├─ fail → print Error, exit code 1
       └─ ok   → FormatRoutingLine → print → wait Enter → exit 0
```

---

## File-by-file

### `Models/WorkCenter.cs`

One factory stage: `Code`, `Name`, `NextCode`.  
`NextDisplay` = next WC code or `(finished on floor)` if last step (RFD).

### `Models/ProductionTicket.cs`

Holds `PoNumber`, `WorkCenterCode`, `Quantity`.  
`FormatRoutingLine(nextStep)` builds the output string.

### `RoutingTable.cs`

In-memory chain: FKIT → WAXINJET → CASTING → GRINDING → FSK → POL → RFD.

| Method | Purpose |
|--------|---------|
| `Find(code)` | Case-insensitive lookup; returns `null` if unknown |
| `PrintAvailableCodes()` | Help text before user types |

**FA later:** `AppVariables.WCList`, routing tables in SQL.

### `Program.cs`

| Piece | Purpose |
|-------|---------|
| `ReadRequired` | Loop until user types non-empty text |
| `ReadQuantity` | `int.TryParse` + must be > 0 |
| `TryBuildTicket` | Validate PO, find WC, build object; `out` parameters for result/error |

**Pattern:** `Try*` + `out` — same style as `TryAdd` in P30/P31 and validation before FA `ExecSP`.

---

## Floor Assistant mapping

| P00 | Floor Assistant |
|-----|-----------------|
| `ProductionTicket` | Row models, `m_UserInfo`, DTOs |
| `TryBuildTicket` | Validation before save / `ExecSP` |
| `RoutingTable` | Work center master / routing |
| `Console.WriteLine` | UI labels, grids, MessageBox |

---

## Run

```powershell
cd <repo root>
dotnet run --project projects/P00-ConsoleFloorTicket/P00.Console.csproj
```

Visual Studio: startup **P00.Console** → **F5**.

**Try:** `PO-2026-001`, `casting`, `12` → Next: GRINDING  
**Try:** work center `XYZ` → error, no ticket printed

---

## Acceptance checklist

- [ ] Builds with `dotnet build`
- [ ] Invalid qty re-prompts (does not print)
- [ ] Unknown work center shows error
- [ ] CASTING shows Next: GRINDING
- [ ] No `dynamic` — all typed classes

## FA homework

- [ ] Open any FA `Views/*.xaml.cs` — find one `class` with properties
- [ ] Grep FA for `List<` in one file

---

## Experiments

1. Add a work center in `RoutingTable.cs`, rebuild, run again.  
2. Set breakpoint in `TryBuildTicket` on `RoutingTable.Find`.  
3. Read PO from a JSON file (stretch).
