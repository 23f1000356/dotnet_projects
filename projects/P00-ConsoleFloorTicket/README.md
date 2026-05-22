# P00 — Console floor ticket (start here)

## What is P00? (simple)

**P00 is your first C# program — no windows, no database, no WPF.**

Imagine a jeweller y factory floor: a **production order (PO)** is a job number (“make 12 rings”). A **work center** is a stage — wax, casting, polishing, stone setting (FSK), etc.

P00 is a **tiny console app** that asks you:

1. PO number  
2. Which work center the job is at now  
3. How many pieces  

Then it prints one line, e.g.:

```text
PO-2026-001 | CASTING | Qty 12 | Next: GRINDING
```

That is the same **idea** as Floor Assistant: track a job moving through stages — but we only print to the terminal, not SQL or screens.

### Why do P00 before WPF?

| You know (web) | P00 teaches (C#) |
|----------------|------------------|
| `const obj = { po, wc, qty }` | `class ProductionTicket` with properties |
| `if (!qty) alert(...)` | Validation method returning `true`/`false` |
| `array.map` / filter | `List<T>` and lookup “next step” |
| Node `console.log` | `Console.WriteLine` |

Every FA screen is C# under the hood. P00 builds that muscle **without** XAML or SQL distraction.

### What you will learn

- `class` and properties  
- `List<>` and reading user input  
- `if` / validation before “save” (here: before print)  
- String formatting (`$"..."`)

### How to run

From this folder’s parent repo root:

```powershell
dotnet run --project projects/P00-ConsoleFloorTicket/P00.Console.csproj
```

Or open `PracticeFA.slnx` in Visual Studio, set **P00.Console** as startup project, press **F5**.

### Done when

- [ ] Invalid quantity shows an error and does not print  
- [ ] Unknown work center code is handled  
- [ ] At least CASTING and FSK show a sensible “Next: …” step  

Next project: **P29** (LINQ) or **P01** (WPF clock-in board).
