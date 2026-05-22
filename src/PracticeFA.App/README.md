# Practice FA App — P02 through P34 (WPF shell)

**Stack:** WPF · `Frame` · `Page` · `UserControl` · Theme · `ShowDialog` · Menu · ToolBar  
**Prerequisite:** [P01 — Clock-in board](../../projects/P01-ClockInBoard/) · [P32](../projects/P32-TwoWayBindingLab/) · [P33](../projects/P33-ValidationLab/) (separate labs)  
**Location:** `src/PracticeFA.App/`  
**Current:** P06 + P34 complete · **Next:** P07 role menu · P10 capstone  
**P06 SQL setup:** [database/README.md](../../database/README.md)  
**P34 full guide:** [P34-README.md](P34-README.md)

---

## What is P02? (simple)

**P02 teaches shell navigation** — one main window stays open; the **right side swaps pages** (Master, Reports).

- **Left menu** = module picker (like FA menu)
- **Frame** = content area that changes
- **Page** = one hub screen (Master, Reports)

**Not in P02:** opening separate feature windows — that is **P04** (`Views/*` + `ShowDialog()`).

---

## Four UI levels (memorize for FA — P34 checkpoint)

```text
Window      →  MainWindow (shell — Menu, ToolBar, sidebar, Frame)
  Frame     →  MainFrame — swaps Page hubs
    Page    →  MasterPage, ReportsPage, …
    UserControl →  EmployeeSearchBox (reusable chunk inside Page or Window)
Window      →  Pop-up feature (P04) — Views/*
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
  App.xaml                      → merges Assets/Theme.xaml
  Assets/Theme.xaml             → P03 shared styles
  MainWindow.xaml(.cs)         → P02 shell + P34 Menu + ToolBar
  Controls/
    EmployeeSearchBox.xaml(.cs) → P34 reusable badge search
  App.config                 → P06 connection string
  Services/
    DataAccess.cs, LoginService.cs, AppState.cs
  Models/
    UserInfo.cs, UserInfoMapper.cs
    WipSummaryRow.cs, ModuleIds.cs, SearchHostContext.cs
  Views/
    SignInWindow.xaml(.cs)   → P06 login (app entry)
  Pages/
    MasterPage.xaml(.cs)        → hub + EmployeeSearchBox + P04 Views
    ReportsPage.xaml(.cs)
  Views/
    StyleWindow, BaggingWindow (+ search control), MisWindow
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

## P34 — UserControl + Menu + ToolBar (detailed)

**Full standalone guide:** [P34-README.md](P34-README.md)

**P34 completes the WPF pillar checkpoint** — you can explain **Frame vs Window vs UserControl** and how FA reuses UI chunks across screens.

### The problem P34 solves

FA repeats the same UI blocks everywhere: employee badge search, date range pickers, toolbars. Copy-pasting XAML into 200 screens is unmaintainable.

**P34 rule:** extract a reusable **UserControl**, host it on **two parents** with **different `DataContext`**, and add **Menu** / **ToolBar** to the main shell like production FA.

### What was added

| Piece | File | Role |
|-------|------|------|
| UserControl | `Controls/EmployeeSearchBox.xaml` | Badge `TextBox` + Search button |
| Dependency property | `BadgeText` | Parent binds or sets badge string (TwoWay) |
| Routed event | `SearchRequested` | Parent handles search (like FA button click) |
| Host context | `Models/SearchHostContext.cs` | Different data per parent |
| Menu | `MainWindow` — File / View / Help | Same navigation as sidebar |
| ToolBar | Refresh, Save (text buttons) | FA-style shell actions |

### Four UI levels (with UserControl)

```text
MainWindow
  Menu + ToolBar                    ← P34 shell chrome
  Frame → MasterPage
            EmployeeSearchBox       ← P34 UserControl (child of Page)
  ShowDialog → BaggingWindow
                  EmployeeSearchBox ← same control, different DataContext
```

| Type | Analogy | P34 example |
|------|---------|-------------|
| `Window` | App / dialog | `MainWindow`, `BaggingWindow` |
| `Page` | Route / hub | `MasterPage` |
| `UserControl` | Reusable React component | `EmployeeSearchBox` |
| `Frame` | `<Outlet />` | `MainFrame` |

### `EmployeeSearchBox` — dependency property

```csharp
public static readonly DependencyProperty BadgeTextProperty =
    DependencyProperty.Register(
        nameof(BadgeText), typeof(string), typeof(EmployeeSearchBox),
        new FrameworkPropertyMetadata(string.Empty,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
```

| Concept | Why |
|---------|-----|
| **Dependency property** | XAML can bind `BadgeText="{Binding SearchBadge}"` from parent |
| **TwoWay by default** | Typing in the control updates parent’s `SearchBadge` |
| **Routed event `SearchRequested`** | Bubble to parent — Search click without tight coupling |

Internal TextBox binds to the control’s own DP:

```xml
Text="{Binding BadgeText, ElementName=Root, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
```

`ElementName=Root` = bind to the UserControl’s `BadgeText`, not the parent’s DataContext directly (clean encapsulation).

### Same control, two parents, two contexts

**MasterPage** (`DataContext` = Master host):

```csharp
DataContext = new SearchHostContext("E101", "Master hub");
```

```xml
<controls:EmployeeSearchBox
    BadgeText="{Binding SearchBadge, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
    SearchRequested="EmployeeSearch_SearchRequested"/>
```

**BaggingWindow** (`DataContext` = Bagging host):

```csharp
DataContext = new SearchHostContext("E205", "Bagging floor");
```

Same XAML control, different starting badge (`E101` vs `E205`) and different search message — proves **UserControl does not own business data**; the **parent’s DataContext** does.

### Menu (same actions as sidebar)

| Menu | Item | Code |
|------|------|------|
| File | Exit | `Exit_Click` → `Close()` |
| View | Master | `NavigateToMaster()` |
| View | Reports | `NavigateToReports()` |
| Help | About | `About_Click` → MessageBox |

**Acceptance:** View → Master does the same as clicking **Master** on the left panel.

### ToolBar

| Button | Behavior |
|--------|----------|
| **Refresh** | Re-runs `NavigateToMaster()` or `NavigateToReports()` for current hub |
| **Save** | Placeholder MessageBox — P08 will call SQL |

Status line: `ToolbarStatusText` in sidebar shows last toolbar action.

### `MainWindow` layout (`DockPanel`)

```text
DockPanel
  Menu (Top)
  ToolBarTray (Top)
  Grid — sidebar + Frame (fills rest)
```

Menu and ToolBar dock above content — standard FA / desktop app layout.

### Page vs Window vs UserControl

| | `Page` | `Window` | `UserControl` |
|--|--------|----------|---------------|
| Root of screen? | No — inside Frame | Yes — dialog or shell | No — always hosted |
| P34 example | MasterPage | MainWindow, BaggingWindow | EmployeeSearchBox |
| FA | `Pages/*` | `MainWindowNew`, `Views/*` | `Controls/*`, shared in Views |

### End-to-end P34 flow

```text
1. App opens — Menu + ToolBar visible
2. MasterPage shows EmployeeSearchBox bound to E101
3. Change badge → Search → message uses [Master hub]
4. View menu → Reports (or left button)
5. Open Bagging Entry (P04) — dialog shows same search control
6. Search there → message uses [Bagging floor] and E205 context
7. Help → About
8. ToolBar Refresh → reloads current page
```

### Floor Assistant mapping

| P34 | Floor Assistant |
|-----|-----------------|
| `Controls/EmployeeSearchBox` | Shared search / filter controls |
| `BadgeText` DP | Bindable parameters on composite controls |
| `MainWindow` Menu | File / module / help menus |
| ToolBar | Save, Refresh, Print on main shell |
| UserControl in View + Page | Same XAML reused across modules |

**FA homework:** grep `UserControl` in FA solution — open one `.xaml` under `Controls/` or `Views/`.

### P34 acceptance checklist

- [ ] Same `EmployeeSearchBox` on MasterPage and `BaggingWindow`
- [ ] Different default badge / search message per host
- [ ] Menu View → Master / Reports matches sidebar
- [ ] Explain Frame vs Window vs UserControl

### P34 experiments

1. Add `EmployeeSearchBox` to `ReportsPage` with a third `SearchHostContext`.  
2. Add ToolBar button **Print** calling MessageBox.  
3. Bind `BadgeText="E999"` in XAML on one host only — see override vs binding.

### What P34 does not cover

- Custom control templates (full lookless controls)  
- MVVM commands on ToolBar (P05)  
- Real Refresh from SQL (P08)

### Quick reference

```xml
xmlns:controls="clr-namespace:PracticeFA.App.Controls"
<controls:EmployeeSearchBox BadgeText="{Binding SearchBadge, Mode=TwoWay}"
                           SearchRequested="OnSearch"/>
```

```csharp
DataContext = new SearchHostContext("E101", "Master hub");
```

---

## P06 — Real login + UserInfo (detailed)

**Full guide:** [database/README.md](../../database/README.md) (SSMS, sqlcmd, troubleshooting).

**Summary:** App starts at `SignInWindow` → `LoginService.TryLogin` → `DataAccess.ExecSp("dbo.spLogin")` → `UserInfoMapper.FromRow` → `AppState.CurrentUser` → `MainWindow`.

| Test user | Password |
|-----------|----------|
| operator1 | pass1 |
| manager1 | pass1 |
| operator2 | pass2 |

```powershell
# One-time DB setup
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\001_PracticeFA.sql

dotnet run --project src/PracticeFA.App/PracticeFA.App.csproj
```

**Rules:** No SQL in `SignInWindow.xaml.cs` · Bad password = friendly `ErrorText` · Session until Exit.

**Next:** P07 calls `spGetUserModules` to show/hide Master hub buttons by user.

---

## User journey

1. **Sign-in** (P06) → valid user opens shell  
2. App shows **Master** in Frame  
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

**P34**

- [ ] `EmployeeSearchBox` on Master + Bagging with different context  
- [ ] Menu navigates like sidebar  
- [ ] Explain Frame / Window / UserControl  

**P06**

- [ ] `001_PracticeFA.sql` run in SSMS  
- [ ] `operator1` / `pass1` opens main shell  
- [ ] Bad password shows friendly message  
- [ ] `SignInWindow.xaml.cs` has no SQL strings  

**Next**

- [ ] P07 role menu · P10 capstone  

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
P01 → P02 (shell) → P32 (binding lab) → P03 (theme) → P04 (Views) → P33 (validation lab) → P34 (UserControl + Menu) → P06 (SQL) → P10
```

**WPF pillar (this app):** P02 · P03 · P04 · P34 — plus P01 clock-in and P32/P33 in `projects/`.
