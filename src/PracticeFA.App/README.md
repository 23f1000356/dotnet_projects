# P02 — Main shell (Frame + Pages)

**Stack:** WPF · `Frame` · `Page` navigation  
**Prerequisite:** [P01 — Clock-in board](../../projects/P01-ClockInBoard/)  
**Location:** `src/PracticeFA.App/`  
**Next:** P03 theme · P04 feature windows

---

## What is P02? (simple)

**P02 teaches shell navigation** — one main window stays open; the **right side swaps pages** (Master, Reports).

- **Left menu** = module picker (like FA menu)
- **Frame** = content area that changes
- **Page** = one hub screen (Master, Reports)

**Not in P02:** opening separate feature windows — that is **P04** (`Views/*` + `ShowDialog()`).

---

## Three UI levels (memorize for FA)

```text
Window   →  MainWindow (shell — stays open)
  Frame  →  MainFrame — swaps content
    Page →  MasterPage, ReportsPage, …
Window   →  Pop-up feature (P04) — Views/*
```

```text
┌─────────────────────────────────────────┐
│ MainWindow                              │
│  ┌──────────┬──────────────────────────┐ │
│  │ Master   │  MasterPage or           │ │
│  │ Reports  │  ReportsPage  (Frame)    │ │
│  │ Exit     │                          │ │
│  └──────────┴──────────────────────────┘ │
└─────────────────────────────────────────┘
         P04 later: button → new Window (modal)
```

---

## P01 vs P02

| | P01 | P02 |
|--|-----|-----|
| Screens | One window, all controls | Shell + changing **Pages** |
| Navigation | None | `MainFrame.Navigate(...)` |
| FA analog | Simple screen | `MainWindowNew` + `Pages/*` |

---

## Folder structure

```text
src/PracticeFA.App/
  App.xaml                 → StartupUri="MainWindow.xaml"
  MainWindow.xaml          → shell: menu + Frame
  MainWindow.xaml.cs       → navigation only
  Pages/
    MasterPage.xaml(.cs)   → product master hub
    ReportsPage.xaml(.cs)  → MIS hub
```

---

## Startup workflow

```text
App starts
  → MainWindow constructor
  → InitializeComponent()
  → NavigateToMaster()  // default page
  → User clicks Reports → NavigateToReports()
  → User clicks Exit → Close()
```

---

## `MainWindow.xaml` — the shell

| Part | Purpose |
|------|---------|
| Column 0 (200px) | Menu: Master, Reports, Exit, `NavHintText` |
| Column 1 (*) | `<Frame x:Name="MainFrame" />` |
| `NavigationUIVisibility="Hidden"` | No browser-style back bar on Frame |

**Web analogy:** sidebar layout + `<Outlet />` for route content.

---

## `MainWindow.xaml.cs` — navigation

```csharp
MainFrame.Navigate(new MasterPage());
MainFrame.Navigate(new ReportsPage());
```

| Method | Effect |
|--------|--------|
| `Navigate(new Page())` | Replace Frame content |
| `Close()` | Exit app |

Shell should stay **thin** — only menu + navigate. No SQL, no business rules here.

**Alternative (FA sometimes uses):**

```csharp
MainFrame.Navigate(new Uri("/Pages/MasterPage.xaml", UriKind.Relative));
```

---

## `Page` vs `Window`

| | `Page` | `Window` |
|--|--------|----------|
| Lives in | `Frame` | Standalone or dialog |
| P02 | MasterPage, ReportsPage | MainWindow only |
| FA | `Pages/BaggingPage`, … | `Views/*` |

---

## `MasterPage`

- Hub for **Master** module (style, SKU, customer, …)
- Buttons use `Tag` + `Placeholder_Click` → MessageBox (“P04 will open View…”)
- **FA:** `Pages/MasterPage` → `new StyleCreationView().ShowDialog()`

---

## `ReportsPage`

- Hub for **MIS / reports** (read-only reports later in P13)
- Placeholder buttons for hourly productivity, WIP, rejection

---

## User journey

1. App opens → **Master** in Frame  
2. Click **Reports** (left) → content swaps; same window  
3. Click **Master** → back  
4. Hub button → placeholder MessageBox  
5. **Exit** → app closes  

Reports does **not** open a second application window.

---

## React Router analogy

| React | P02 |
|-------|-----|
| Layout + sidebar | `MainWindow` left column |
| `<Outlet />` | `Frame` |
| `/master` component | `MasterPage` |
| Modal feature | P04 `ShowDialog()` |

---

## Floor Assistant mapping

| P02 | Floor Assistant |
|-----|-----------------|
| `MainWindow` | `MainWindowNew` |
| Left menu | Module menu / tree |
| `MainFrame` | `Frame` in main window |
| `MasterPage` | `Pages/MasterPage` |
| `ReportsPage` | MIS hub page |
| Hub buttons | Open `Views/*.xaml` |

**FA homework:** Grep `Frame` in `MainWindowNew` · trace one `Navigate` in `Pages/*.xaml.cs`.

---

## Run

```powershell
dotnet run --project src/PracticeFA.App/PracticeFA.App.csproj
```

Visual Studio: startup **PracticeFA.App** → **F5**.

---

## Acceptance checklist

- [ ] Explain Window vs Page vs Frame  
- [ ] Reports does not open a new app window  
- [ ] Point to `MainFrame.Navigate` in code  
- [ ] Know P04 adds `Views/*` windows from hub buttons  

---

## Experiments

1. Add **Bagging** menu button + empty `BaggingPage`.  
2. Breakpoint on `NavigateToReports()`.  
3. Find FA `MainWindowNew` + one `Pages` file side by side.

---

## Learning path

```text
P01 (one window) → P02 (shell + pages) → P03 (theme) → P04 (Views) → P06 (SQL login) → P10 (full mini FA)
```
