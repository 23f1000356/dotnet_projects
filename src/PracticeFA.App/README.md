# P02 + P03 + P04 — Shell, theme, and modal Views

**Stack:** WPF · `Frame` · `Page` · Resource dictionaries · `ShowDialog`  
**Prerequisite:** [P01 — Clock-in board](../../projects/P01-ClockInBoard/)  
**Location:** `src/PracticeFA.App/`  
**Current:** P04 complete · **Next:** P32 TwoWay binding or P06 SQL login

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
         P04: hub button → Views/* Window (ShowDialog)
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
  Models/
    WipSummaryRow.cs       → sample grid rows (Reports)
    ModuleIds.cs           → fake module ids (1001, 2001, 3001)
  Views/
    StyleWindow.xaml(.cs)  → module 1001 modal
    BaggingWindow.xaml(.cs)→ module 2001 modal
    MisWindow.xaml(.cs)    → module 3001 modal
  Pages/
    MasterPage.xaml(.cs)   → hub → ShowDialog Views
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

- Hub for **Master** module (style, bagging shortcuts, …)
- **P04:** three buttons open modal `Views/*` windows via `OpenFeature()`
- **FA:** `Pages/MasterPage` → `new StyleCreationView { Owner = main }.ShowDialog()`

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

## P04 — Module launcher → feature windows (detailed)

**P02** = *where* hubs live (`Frame` + `Page`).  
**P03** = *how* they look (`Theme.xaml`).  
**P04** = hub **Page** opens separate **Windows** as **modal dialogs** — the most common Floor Assistant pattern after a menu click.

### The problem P04 solves

Real FA flow:

1. Main app stays open (left menu).
2. User opens **Master** or **Bagging** hub (`Page` in `Frame`).
3. User clicks **Style Creation** or **Bagging Entry**.
4. A **new window** opens on top for data entry.
5. User **Save** or **Cancel** — window closes; hub works again.

P04 teaches that layer: not `Frame.Navigate`, but `new SomeWindow().ShowDialog()`.

### Three UI levels (full picture)

```text
Window  →  MainWindow              ← shell, always open (P02)
  Frame →  MasterPage / ReportsPage     ← hubs inside shell (P02)
Window  →  StyleWindow, BaggingWindow, MisWindow   ← features (P04)
```

```text
┌─────────────────────────────────────────────────────┐
│ MainWindow (Owner)                                  │
│  ┌─────────┬──────────────────────────────────────┐ │
│  │ Master  │  MasterPage (hub)                    │ │
│  │ Reports │    [Style Creation] [Bagging] …    │ │
│  └─────────┴──────────────────────────────────────┘ │
└─────────────────────────────────────────────────────┘
              click Style Creation
                    ↓
        ┌──────────────────────────┐
        │ StyleWindow (modal)        │  ← blocks hub until closed
        │  TextBox + Save / Cancel   │
        └──────────────────────────┘
```

| Level | Type | Stays in shell? | How you open it |
|-------|------|-----------------|-----------------|
| Shell | `MainWindow` | Yes | App startup |
| Hub | `Page` in `Frame` | Yes | `MainFrame.Navigate(new MasterPage())` |
| Feature | `Window` in `Views/` | No — pop-up | `ShowDialog()` |

### Web → WPF mapping

| Web / React | P04 (WPF) |
|-------------|-----------|
| App layout + sidebar | `MainWindow` |
| Route `/master` | `MasterPage` in `Frame` |
| `dialog.showModal()` | `ShowDialog()` |
| Modal `onClose(result)` | `DialogResult` true / false |
| Center on parent | `Owner = mainWindow` |

### Folder roles

| Path | Role |
|------|------|
| `Pages/MasterPage` | Launcher — buttons only |
| `Views/StyleWindow` | Feature screen (module 1001) |
| `Views/BaggingWindow` | Feature screen (module 2001) |
| `Views/MisWindow` | Feature screen (module 3001) |
| `Models/ModuleIds.cs` | Fake FA module numbers |

**FA naming:** `Pages/*` = hubs · `Views/*` = work screens (CRUD, scan, reports).

### Module IDs (`Models/ModuleIds.cs`)

```csharp
public const int StyleCreation = 1001;
public const int BaggingEntry = 2001;
public const int MisProductivity = 3001;
```

Passed into window constructors: `new StyleWindow(ModuleIds.StyleCreation)`.  
Real FA uses module ids for permissions, logging, and which View to open.

| Button | Module id | Window |
|--------|-----------|--------|
| Style Creation | 1001 | `Views/StyleWindow` |
| Bagging Entry | 2001 | `Views/BaggingWindow` |
| MIS Productivity | 3001 | `Views/MisWindow` |

### Step 1 — Hub buttons (`MasterPage.xaml`)

Hub has three buttons; each has its own `Click` handler.  
The hub does **not** contain the form — it only **launches** the right `Window`.

### Step 2 — Open dialog (`MasterPage.xaml.cs`)

```csharp
private void StyleCreation_Click(object sender, RoutedEventArgs e) =>
    OpenFeature(new StyleWindow(ModuleIds.StyleCreation));

private void OpenFeature(Window featureWindow)
{
    var owner = Window.GetWindow(this);
    if (owner is not null)
        featureWindow.Owner = owner;

    var saved = featureWindow.ShowDialog() == true;
    LastDialogText.Text = saved
        ? $"Last dialog: {featureWindow.Title} — Saved (DialogResult=true)"
        : $"Last dialog: {featureWindow.Title} — Cancelled (DialogResult=false)";
}
```

| Line | Meaning |
|------|---------|
| `new StyleWindow(...)` | Create feature window (not shown yet) |
| `Window.GetWindow(this)` | From a `Page`, find parent `MainWindow` |
| `Owner = owner` | Center dialog on shell |
| `ShowDialog()` | **Modal** — code waits until window closes |
| `== true` | User clicked Save |
| `LastDialogText` | Shows Save vs Cancel after close |

**Why `GetWindow(this)?** `MasterPage` is a `Page` inside `Frame`, not a `Window`. `GetWindow` walks up to `MainWindow` for `Owner`.

### `ShowDialog()` vs `Show()`

| | `Show()` | `ShowDialog()` |
|--|----------|----------------|
| Modal? | No — hub still clickable | **Yes** — hub blocked |
| Code continues | Immediately | After window closes |
| Return value | None | `bool?` (`DialogResult`) |
| FA | Rare for edits | **Very common** for Save/Cancel |

### Step 3 — Feature window (`Views/StyleWindow`)

**XAML** — root is `<Window>`, not `<Page>`:

```xml
<Window WindowStartupLocation="CenterOwner" ResizeMode="NoResize" ...>
```

- `CenterOwner` — center on `Owner` (`MainWindow`)
- `ResizeMode="NoResize"` — fixed dialog size

**Code-behind** — Save / Cancel:

```csharp
private void Save_Click(object sender, RoutedEventArgs e)
{
    DialogResult = true;
    Close();
}

private void Cancel_Click(object sender, RoutedEventArgs e)
{
    DialogResult = false;
    Close();
}
```

| Button | `DialogResult` | Hub sees |
|--------|----------------|----------|
| Save | `true` | `ShowDialog() == true` |
| Cancel | `false` | Cancelled |

Set `DialogResult` **before** `Close()`. Later P08+ will call SQL on Save.

`BaggingWindow` and `MisWindow` follow the same pattern with different labels.

### End-to-end flow (one click)

```text
1. MainWindow → Frame shows MasterPage
2. User clicks "Style Creation (1001)"
3. new StyleWindow(1001), Owner = MainWindow
4. ShowDialog() — hub frozen
5. User clicks Save → DialogResult = true, Close()
6. ShowDialog() returns true → LastDialogText updated
7. User can open Bagging or MIS next
```

### P02 + P03 + P04 together

| Project | You learn |
|---------|-----------|
| P02 | `MainWindow` + `Frame.Navigate(MasterPage)` |
| P03 | `Theme.xaml` on all buttons/grids |
| P04 | `Views/*` + `ShowDialog()` from hub |

`MainWindow.xaml.cs` stays thin — only `Navigate` to Master/Reports. P04 logic is in **MasterPage** + **Views** only.

### Page vs Window

| | `Page` in `Frame` | `Window` in `Views/` |
|--|-------------------|----------------------|
| Stays in shell | Yes | No — pop-up |
| Open with | `Frame.Navigate` | `ShowDialog()` |
| P04 example | MasterPage | StyleWindow, BaggingWindow, MisWindow |
| FA | `Pages/BaggingPage` | `Views/BaggingEntryView.xaml` |

### Floor Assistant mapping

| P04 | Floor Assistant |
|-----|-----------------|
| `Pages/MasterPage` | Module hub pages |
| `Views/StyleWindow` | `Views/*.xaml` feature screens |
| `ShowDialog()` | Modal edits, confirmations |
| `Owner = main` | Dialog centered on shell |
| `ModuleIds` | Real menu/security module ids |
| Save → `DialogResult = true` | Then `ExecSP`, refresh grid |

**FA homework:** grep `ShowDialog` in one `Pages/*.xaml.cs` · trace button → `new *View` → Save.

### P04 experiments

1. Breakpoint on `ShowDialog()` — step Save vs Cancel return values.
2. Remove `Owner` — see dialog position; restore `Owner`.
3. Replace `ShowDialog()` with `Show()` — hub stays clickable; compare.
4. Add a fourth `Views/*` window from a new Master button.

### What P04 does not cover

- SQL / `ExecSP` on Save (P08+)
- MVVM (P05, P32)
- Validation (P33)
- Passing selected grid row into dialog (common FA pattern — later)
- Non-modal `Show()` windows

### Quick reference

```csharp
var owner = Window.GetWindow(this);
var dlg = new StyleWindow(ModuleIds.StyleCreation) { Owner = owner };
if (dlg.ShowDialog() == true)
{
    // user saved — later: read fields, call service, refresh grid
}
```

**Memorize:** Hub = `Page` + buttons · Feature = `Window` in `Views/` · Connect with `ShowDialog()` + `Owner` + `DialogResult`.

---

## User journey

1. App opens → **Master** in Frame  
2. Click **Reports** (left) → content swaps; same window  
3. Click **Master** → back  
4. Hub button (e.g. Style Creation) → **modal** `Views/*` window; hub blocked until Save/Cancel  
5. **Exit** → app closes  

Reports does **not** open a second application window. P04 feature windows are **dialogs on top** of the shell, not a second app.

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

**P04**

- [ ] Three modules open three different windows  
- [ ] `ShowDialog` blocks hub; `Owner` set  
- [ ] Save/Cancel set `DialogResult`  

**Next**

- [ ] P32 binding lab or P06 SQL login  

---

## Experiments

**P02**

1. Add **Bagging** menu button + empty `BaggingPage` in `Frame`.  
2. Breakpoint on `NavigateToReports()`.  

**P04**

3. Breakpoint on `ShowDialog()` in `OpenFeature`.  
4. Try `Show()` instead of `ShowDialog()` — feel non-modal behavior.  

**FA**

5. Find `MainWindowNew` + one `Pages` file + one `Views` file side by side.

---

## Learning path

```text
P01 → P02 (shell) → P03 (theme) → P04 (Views/ShowDialog) → P32/P06 → P10 (mini FA)
```
