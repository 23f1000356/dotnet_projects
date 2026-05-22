# P34 — UserControl + Menu + ToolBar (detailed guide)

**Location:** `src/PracticeFA.App/`  
**Prerequisites:** [P02](README.md) shell · [P03](README.md) theme · [P04](README.md) modal Views  
**Works with:** [P06](../../database/README.md) sign-in (main shell opens after login)

---

## What is P34? (simple)

**P34 teaches reusable UI and shell chrome** — the pieces FA uses on almost every screen:

1. **UserControl** — one XAML component used in many places (`EmployeeSearchBox`)
2. **Menu** — File / View / Help on `MainWindow`
3. **ToolBar** — Refresh / Save actions

After P34 you can explain: **Frame vs Window vs UserControl**.

---

## The problem P34 solves

Floor Assistant has hundreds of screens. Copy-pasting “badge search” XAML into every Page and View is unmaintainable.

| Bad | Good (P34) |
|-----|------------|
| Duplicate search UI in 50 files | One `Controls/EmployeeSearchBox.xaml` |
| Menu logic only in sidebar buttons | Menu **and** sidebar call same `NavigateToMaster()` |
| No shared toolbar | `ToolBar` on shell for Refresh / Save |

---

## Four UI levels (WPF pillar checkpoint)

```text
Window       →  MainWindow (Menu, ToolBar, sidebar, Frame)
  Frame      →  MainFrame navigates Pages
    Page     →  MasterPage, ReportsPage
    UserControl →  EmployeeSearchBox (hosted inside Page or Window)
Window       →  BaggingWindow, StyleWindow (P04 dialogs)
```

```text
┌──────────────────────────────────────────────────────────┐
│ MainWindow                                                │
│  [File] [View] [Help]     [Refresh] [Save]    ← P34      │
│  ┌─────────┬────────────────────────────────────────────┐ │
│  │ Master  │  MasterPage                                 │ │
│  │ Reports │    ┌ EmployeeSearchBox (UserControl) ─┐   │ │
│  │ Exit    │    │ Badge [____] [Search]              │   │ │
│  └─────────┴────┴────────────────────────────────────┴───┘ │
└──────────────────────────────────────────────────────────┘
         Click Bagging (P04) → BaggingWindow
                                   └── same EmployeeSearchBox, different DataContext
```

| Type | Lives where | Opens how | P34 example |
|------|-------------|-----------|-------------|
| **Window** | Desktop root | `Show()` / `ShowDialog()` | `MainWindow`, `BaggingWindow` |
| **Page** | Inside `Frame` | `Frame.Navigate` | `MasterPage` |
| **UserControl** | Inside Page/Window | Included in XAML | `EmployeeSearchBox` |
| **Frame** | Inside Window | Hosts Pages | `MainFrame` |

**Web analogy:** UserControl ≈ reusable React component; Menu/ToolBar ≈ app layout header.

---

## Files you need to know

```text
src/PracticeFA.App/
  Controls/
    EmployeeSearchBox.xaml      → UI: badge TextBox + Search button
    EmployeeSearchBox.xaml.cs   → BadgeText DP + SearchRequested event
  Models/
    SearchHostContext.cs        → Different data per host (E101 vs E205)
  MainWindow.xaml(.cs)         → Menu, ToolBar, sidebar, Frame
  Pages/MasterPage.xaml(.cs)    → Host #1 for search control
  Views/BaggingWindow.xaml(.cs) → Host #2 for search control
```

---

## Part 1 — `EmployeeSearchBox` UserControl

### XAML (`Controls/EmployeeSearchBox.xaml`)

- Root tag is `<UserControl>` (not `Page`, not `Window`)
- Internal `TextBox` binds to the control’s own property:

```xml
Text="{Binding BadgeText, ElementName=Root, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
```

`ElementName=Root` means: bind to **this UserControl’s** `BadgeText` property. Parents set `BadgeText="{Binding SearchBadge}"` on the control tag — data flows parent → control → inner TextBox.

### Dependency property (`BadgeText`)

```csharp
public static readonly DependencyProperty BadgeTextProperty =
    DependencyProperty.Register(
        nameof(BadgeText),
        typeof(string),
        typeof(EmployeeSearchBox),
        new FrameworkPropertyMetadata(
            string.Empty,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
```

| Why DP? | Benefit |
|---------|---------|
| XAML can set `BadgeText="E101"` or bind `{Binding SearchBadge}` | Reusable across hosts |
| TwoWay by default | Typing updates parent’s bound property |

**FA:** composite controls expose bindable properties the same way.

### Routed event (`SearchRequested`)

```csharp
private void Search_Click(object sender, RoutedEventArgs e) =>
    RaiseEvent(new RoutedEventArgs(SearchRequestedEvent, this));
```

Parent handles search **without** the UserControl knowing about SQL or navigation:

```xml
<controls:EmployeeSearchBox SearchRequested="EmployeeSearch_SearchRequested" .../>
```

---

## Part 2 — Same control, two parents, two contexts

### Host 1 — `MasterPage`

```csharp
// MasterPage.xaml.cs constructor
DataContext = new SearchHostContext("E101", "Master hub");
```

```xml
<controls:EmployeeSearchBox
    BadgeText="{Binding SearchBadge, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
    SearchRequested="EmployeeSearch_SearchRequested"/>
```

Search message example: `[Master hub] Search badge E101 — P08 will call spGetEmployeeByBadge.`

### Host 2 — `BaggingWindow`

```csharp
DataContext = new SearchHostContext("E205", "Bagging floor");
```

Same XAML control type; different default badge and message text.

**Acceptance test:** control looks the same; **data and handler output** differ — proves UserControl does not hard-code business context.

### `SearchHostContext`

| Property | Master | Bagging |
|----------|--------|---------|
| `SearchBadge` | E101 | E205 |
| `HostName` | Master hub | Bagging floor |

---

## Part 3 — Menu (`MainWindow.xaml`)

```xml
<Menu DockPanel.Dock="Top">
  <MenuItem Header="_File">
    <MenuItem Header="E_xit" Click="Exit_Click"/>
  </MenuItem>
  <MenuItem Header="_View">
    <MenuItem Header="_Master" Click="Master_Click"/>
    <MenuItem Header="_Reports" Click="Reports_Click"/>
  </MenuItem>
  <MenuItem Header="_Help">
    <MenuItem Header="_About Practice FA" Click="About_Click"/>
  </MenuItem>
</Menu>
```

| Item | Calls | Same as sidebar? |
|------|-------|------------------|
| View → Master | `NavigateToMaster()` | Yes |
| View → Reports | `NavigateToReports()` | Yes |
| File → Exit | `Close()` + clear `AppState` (P06) | Yes |

Underline in `_Master` = Alt keyboard shortcut (WPF menu mnemonic).

---

## Part 4 — ToolBar

```xml
<ToolBarTray DockPanel.Dock="Top">
  <ToolBar>
    <Button Content="Refresh" Click="ToolbarRefresh_Click"/>
    <Button Content="Save" Click="ToolbarSave_Click"/>
  </ToolBar>
</ToolBarTray>
```

| Button | Behavior |
|--------|----------|
| **Refresh** | Re-navigates current hub (Master or Reports) |
| **Save** | Placeholder MessageBox — P08 will persist via SP |

Status: `ToolbarStatusText` in sidebar shows last action or P06 signed-in user.

---

## Layout — `DockPanel` on `MainWindow`

```text
DockPanel
  Menu           → Dock Top
  ToolBarTray    → Dock Top
  Grid (sidebar + Frame) → fills remaining space
```

Menu and toolbar stack above the P02 shell — standard desktop FA layout.

---

## Run and test P34

**Requires P06 database + login** (see [database/README.md](../../database/README.md)).

```powershell
cd "c:\Users\Vishakha.Roy\Desktop\Sample_dotnet"
dotnet run --project src/PracticeFA.App/PracticeFA.App.csproj
```

### Test checklist

1. Sign in (`operator1` / `pass1`)
2. On **Master** — see `EmployeeSearchBox`, default badge **E101**, click **Search**
3. **View → Reports** — same as left **Reports** button
4. **View → Master** — back; search still works
5. Open **Bagging Entry (2001)** — dialog shows search with **E205** context
6. **Help → About** — shows session user (P06)
7. **ToolBar → Refresh** — status line updates time
8. **ToolBar → Save** — placeholder dialog

---

## Floor Assistant mapping

| P34 | Floor Assistant |
|-----|-----------------|
| `Controls/*.xaml` | Shared controls under `Views/` / `Controls/` |
| Dependency properties | Bindable parameters on composite UI |
| `MainWindow` Menu | Module / file / help menus |
| ToolBar | Save, refresh, print on main shell |
| UserControl in Page + View | Same chunk on hub and feature screen |

**Homework:** grep `UserControl` in FA solution; open one `.xaml` and list its dependency properties.

---

## P34 vs nearby projects

| Project | Topic |
|---------|--------|
| P02 | Frame + Pages |
| P03 | Theme.xaml |
| P04 | Views + ShowDialog |
| **P34** | UserControl + Menu + ToolBar |
| P06 | SQL login before shell |
| P07 | Hide hub buttons by `spGetUserModules` |

---

## Acceptance criteria

- [ ] `EmployeeSearchBox` on **MasterPage** and **BaggingWindow**
- [ ] Different `SearchHostContext` / search messages per host
- [ ] Menu **View → Master/Reports** matches sidebar
- [ ] Can explain **Frame / Window / UserControl**

---

## Experiments

1. Add `EmployeeSearchBox` to `ReportsPage` with a third context.  
2. Add ToolBar button **Print** with `MessageBox`.  
3. Set `BadgeText="E999"` in XAML on one host only — see binding vs literal.  
4. Remove `SearchRequested` handler — Search button does nothing (event wiring practice).

---

## What P34 does not cover

- Custom control templates (lookless controls)  
- MVVM `ICommand` on toolbar (P05)  
- Module visibility from DB (P07)  
- Real Save to SQL (P08)

---

## Quick reference

```xml
xmlns:controls="clr-namespace:PracticeFA.App.Controls"

<controls:EmployeeSearchBox
    BadgeText="{Binding SearchBadge, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
    SearchRequested="OnSearch"/>
```

```csharp
DataContext = new SearchHostContext("E101", "Master hub");
```

**Memorize:** UserControl = reusable chunk · Menu/ToolBar = shell chrome · Host sets `DataContext`, control stays dumb.
