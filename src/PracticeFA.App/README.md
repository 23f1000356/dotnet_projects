# PracticeFA.App — P02 shell (Frame + Pages)

**Prerequisite:** [P01 — Clock-in board](../../projects/P01-ClockInBoard/)

## What P02 is

One **main window** with:

- **Left:** menu (Master, Reports, Exit) — like FA module menu
- **Right:** `Frame` that swaps **Page** content — not a new window each time

```
MainWindow (shell)
  ├── Menu
  └── Frame → MasterPage | ReportsPage
```

Feature screens in FA open as separate `Views/*` **windows** (P04). The Frame holds **hub pages** only.

## Run

```powershell
dotnet run --project src/PracticeFA.App/PracticeFA.App.csproj
```

Visual Studio: startup **PracticeFA.App** → **F5**.

## Files

| File | Role |
|------|------|
| `MainWindow.xaml` | Shell layout + `Frame` |
| `MainWindow.xaml.cs` | `Navigate(new MasterPage())` |
| `Pages/MasterPage.xaml` | Master hub |
| `Pages/ReportsPage.xaml` | Reports hub |

## Next

**P03** — theme dictionary · **P04** — open View windows from Master
