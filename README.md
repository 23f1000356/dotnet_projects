# Practice Floor Assistant

Learn WPF + C# + SQL by rebuilding **small slices** of the real Floor Assistant app.

## Quick start

1. Open `PracticeFA.slnx` in Visual Studio 2022+.
2. Set startup project: **PracticeFA.App**.
3. Press **F5**.

**C# track:** [P00](projects/P00-ConsoleFloorTicket/) → [P29](projects/P29-LinqCollectionsLab/) → [P30](projects/P30-InterfacesRepository/) → [P31](projects/P31-ExceptionsLogging/).

**WPF:** [P01 — Clock-in](projects/P01-ClockInBoard/) → [P02 — Frame shell](src/PracticeFA.App/) → P03+ in same app.

**Also in repo:** P01 — employee clock-in board (WPF, in-memory).

## Docs

- Quick map (web → WPF): [docs/ROADMAP.md](docs/ROADMAP.md)
- Schedule & pillars: [docs/LEARNING_PATHWAY.md](docs/LEARNING_PATHWAY.md)
- **Detailed build specs (P + SAP):** [docs/projects.md](docs/projects.md) — **74 projects** (53 code + 21 SAP)
- **SAP cheat sheet template:** [docs/sap-cheat-sheet-template.md](docs/sap-cheat-sheet-template.md)

## If you know React

| React | This repo |
|-------|-----------|
| `App.jsx` | `App.xaml` |
| Page component | `MainWindow.xaml` + `.xaml.cs` |
| `useState(list)` | `List<Employee>` + `ItemsSource` on ListBox |
| API call | Later: `ExecSP` → `DataTable` |
