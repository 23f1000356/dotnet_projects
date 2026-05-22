# P02 + P03 — Shell, pages, and shared theme

**Stack:** WPF · `Frame` · `Page` · Resource dictionaries  
**Prerequisite:** [P01 — Clock-in board](../../projects/P01-ClockInBoard/)  
**Location:** `src/PracticeFA.App/`  
**Current:** P03 complete · **Next:** P04 feature windows

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
  App.xaml                 → merges Assets/Theme.xaml globally
  Assets/Theme.xaml        → P03 colors + Button + DataGrid styles
  MainWindow.xaml          → shell: menu + Frame
  MainWindow.xaml.cs       → navigation only
  Models/WipSummaryRow.cs  → sample grid rows (Reports)
  Pages/
    MasterPage.xaml(.cs)   → product master hub
    ReportsPage.xaml(.cs)  → MIS hub + DataGrid
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
- **P03:** `DataGrid` with 3 columns (work center, open orders, status) — headers styled in `Theme.xaml`, not per column

---

## P03 — Resource dictionary theme (detailed)

**P02** = *where* screens live (shell + `Frame` + `Page`).  
**P03** = *how* they look — one shared design system for the whole app.

### The problem P03 solves

Before P03, every button might look like this:

```xml
<Button Background="#0066AA" Padding="12,8" .../>
```

If the brand color changes, you edit hundreds of XAML files. **P03 rule:** define colors and styles once in `Assets/Theme.xaml`, merge in `App.xaml`, then screens only reference keys like `{StaticResource PrimaryBrush}`.

### Web → WPF mapping

| Web / React | P03 (WPF) |
|-------------|-----------|
| `globals.css` or CSS variables | `Assets/Theme.xaml` |
| `:root { --primary: #0066AA }` | `<Color x:Key="PrimaryColor">` + `PrimaryBrush` |
| `button { ... }` | `<Style TargetType="Button">` (implicit) |
| `className="page-title"` | `Style="{StaticResource PageTitleStyle}"` |
| Import theme in root layout | Merge dictionary in `App.xaml` |

### How resources flow

```text
App.xaml starts
    └── merges Assets/Theme.xaml → Application.Resources
            └── every Window, Page, Button, DataGrid sees those keys

MainWindow.xaml     MasterPage.xaml      ReportsPage.xaml
  SidebarBrush        PageTitleStyle         DataGrid + headers from Theme
  implicit Button     implicit Button
```

Shell code (`MainWindow.xaml.cs`) does **not** set colors — XAML only.

### Step 1 — Merge theme globally (`App.xaml`)

```xml
<Application.Resources>
  <ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>
      <ResourceDictionary Source="Assets/Theme.xaml"/>
    </ResourceDictionary.MergedDictionaries>
  </ResourceDictionary>
</Application.Resources>
```

- `Application.Resources` = app-wide resource bag (global stylesheet).
- **Merged dictionary** = WPF loads `Theme.xaml` into that bag once.
- Do **not** merge `Theme.xaml` again on each Page.

### Step 2 — Colors and brushes (`Assets/Theme.xaml`)

```xml
<Color x:Key="PrimaryColor">#0066AA</Color>
<SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource PrimaryColor}"/>
```

| Type | Role |
|------|------|
| **Color** | Raw value (`#0066AA`) — single source of truth |
| **Brush** | What WPF paints with (`Background`, `Foreground` need a Brush) |

| Brush key | Use |
|-----------|-----|
| `PrimaryBrush` | Buttons, titles, DataGrid headers |
| `SurfaceBrush` | Cards, grid rows |
| `SidebarBrush` | Left menu background |
| `ErrorBrush` | Validation (P33) |
| `MutedTextBrush` | Secondary text |
| `BorderBrush` | Card and grid borders |

**Acceptance test:** change `PrimaryColor` on line 5 only → rebuild → all `PrimaryBrush` consumers update.

### Step 3 — `{StaticResource ...}` on pages

```xml
<Border Background="{StaticResource SidebarBrush}"/>
<TextBlock Style="{StaticResource PageTitleStyle}" Text="Reports"/>
<Button Content="Master"/>   <!-- implicit Button style, no Style= needed -->
```

**`StaticResource`** = resolve key when XAML loads (resource must exist — merged in `App.xaml` first).

### Named vs implicit styles

| Kind | Definition | Example |
|------|------------|---------|
| **Named** | `x:Key="PageTitleStyle"` + `Style="{StaticResource ...}"` | Titles, subtitles, `CardBorderStyle` |
| **Implicit** | `<Style TargetType="Button">` (no `x:Key`) | Every `Button` in the app |
| **Implicit** | `<Style TargetType="DataGridColumnHeader">` | Every grid header row |

Named = specific UI pieces. Implicit = all controls of that type (like global CSS `button { }`).

### Button — style + control template

Default WPF buttons are gray. P03 uses a **ControlTemplate**:

- `TemplateBinding Background` — uses style’s `PrimaryBrush`
- `CornerRadius="4"` — rounded corners
- Triggers on `IsMouseOver`, `IsPressed`, `IsEnabled` — hover/press without C#

**Style** = property values. **Template** = how the control is drawn. FA uses the same split for tabs and custom buttons.

### DataGrid on Reports (`ReportsPage.xaml`)

Columns define **Header** and **Binding** only — no per-column `Background="#..."`.

```xml
<DataGrid x:Name="WipGrid">
  <DataGrid.Columns>
    <DataGridTextColumn Header="Work center" Binding="{Binding WorkCenter}" Width="*"/>
    <DataGridTextColumn Header="Open orders" Binding="{Binding OpenOrders}" Width="120"/>
    <DataGridTextColumn Header="Status" Binding="{Binding Status}" Width="*"/>
  </DataGrid.Columns>
</DataGrid>
```

- `TargetType="DataGridColumnHeader"` in Theme → blue headers, white text, semi-bold.
- `TargetType="DataGrid"` in Theme → borders, zebra rows (`AlternatingRowBackground`).
- Data: `WipSummaryRow` + `ItemsSource` in `ReportsPage.xaml.cs` (fake rows; SQL in P13).

### P02 vs P03

| | P02 | P03 |
|--|-----|-----|
| Navigation | `Frame.Navigate` | Same |
| Colors | `#F3F4F6`, `#0066AA` inline | `{StaticResource ...}` |
| Buttons | Inline Padding/Background | Implicit style |
| Reports | Placeholder buttons | + styled `DataGrid` |
| New files | — | `Assets/Theme.xaml`, `Models/WipSummaryRow.cs` |

### Three ways to apply the theme

1. **Brush on property** — `Background="{StaticResource SidebarBrush}"`
2. **Named style** — `Style="{StaticResource PageTitleStyle}"`
3. **Implicit by type** — plain `<Button/>` picks `TargetType="Button"` style

### Floor Assistant mapping

| P03 | Floor Assistant |
|-----|-----------------|
| `Assets/Theme.xaml` | `Assets/*.xaml` (buttons, tabs, grids) |
| Merge in `App.xaml` | App-wide merged dictionaries |
| `{StaticResource PrimaryBrush}` | Same across `Views/*` |
| Implicit grid header style | Consistent production/MIS tables |

**FA homework:** open FA `App.xaml` → list `MergedDictionaries` → grep one View for `{StaticResource`.

### P03 experiments

1. Change `PrimaryColor` to `#2E7D32` or `#6A1B9A` — run app; menu, hub buttons, titles, grid headers should all match.
2. If one button stays old color, it has a **local** `Background=` overriding the theme — remove it.
3. Add a second `DataGrid` on Master — headers should style automatically.

### What P03 does not cover

- Runtime dark/light toggle (swap dictionaries at runtime).
- Per-module themes (extra merges per Page).
- MVVM (styling is XAML-only).
- SQL (grid data is sample only).

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

**P02**

- [ ] Explain Window vs Page vs Frame  
- [ ] Reports does not open a new app window  
- [ ] Point to `MainFrame.Navigate` in code  

**P03**

- [ ] Change `PrimaryColor` once → all buttons + grid headers update  
- [ ] DataGrid on Reports has styled headers without per-column XAML  
- [ ] No duplicate `Background="#..."` on every button  

**Next**

- [ ] P04 adds `Views/*` windows from hub buttons  

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
