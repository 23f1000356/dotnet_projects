# P01 — Employee clock-in board (WPF)

**Stack:** WPF · XAML · Code-behind · In-memory state  
**Prerequisites:** [P00](../P00-ConsoleFloorTicket/) recommended  
**Next:** [P02 — Frame shell](../../src/PracticeFA.App/)

---

## What is P01? (simple)

**P01 is your first WPF (Windows) app** — a real window with text boxes, buttons, and a list.

It simulates:

1. **Sign-in** — who is using the app (fake session)
2. **Clock-in** — who is on the shop floor right now

No database — data lives in `List<>` and `Dictionary<>` until you close the app.

---

## P00 vs P01

| | P00 | P01 |
|--|-----|-----|
| UI | Black console | Graphical **Window** |
| Input | `Console.ReadLine` | `TextBox` + clicks |
| Output | One line of text | `ListBox` + labels |
| State | One ticket | Session + floor list |

---

## Two files per screen (like JSX + handlers)

| File | Role | Web |
|------|------|-----|
| `MainWindow.xaml` | Layout, controls | HTML/JSX |
| `MainWindow.xaml.cs` | Clicks, state, logic | Event handlers |

`InitializeComponent()` connects XAML `x:Name` controls to C# fields (`LoginBadgeBox`, `OnFloorList`, …).

---

## Folder structure

```text
projects/P01-ClockInBoard/
  App.xaml / App.xaml.cs       → starts app, opens MainWindow
  MainWindow.xaml              → UI (VIEW)
  MainWindow.xaml.cs           → logic (handlers)
  Models/
    UserSession.cs             → logged-in user + plant
    FloorEmployee.cs           → one person on floor list
```

---

## UI layout (`MainWindow.xaml`)

```text
Window
└── Grid (4 rows)
    ├── Title + description
    ├── Border (main area)
    │   ├── Left: Sign in + Clock in (TextBox, Button)
    │   └── Right: ListBox "On floor" + count + Clear
    └── Footer hint
```

| Control | Purpose |
|---------|---------|
| `LoginBadgeBox` | Your badge (E001, E002, …) |
| `SessionText` | Shows signed-in name + plant |
| `ClockInBadgeBox` | Starts **disabled** until sign-in |
| `OnFloorList` | `DisplayMemberPath="ListLabel"` per row |
| `FloorCountText` | e.g. `3 on floor` |

---

## State in code-behind (like React state)

| Field | Role |
|-------|------|
| `_demoUsers` | E001/E002 → name + plant |
| `_employeeDirectory` | E101–E104 → job titles |
| `_currentUser` | Session after sign-in (`null` = not signed in) |
| `_onFloor` | `List<FloorEmployee>` — everyone clocked in |

---

## Flow 1: Sign in

```text
SignIn_Click
  → read LoginBadgeBox
  → empty? MessageBox
  → lookup _demoUsers or create Guest
  → _currentUser = session
  → SessionText updated
  → enable ClockInBadgeBox + ClockInButton
```

**FA:** `SignIn_New` + SP → `clsDictionary.UserInfo`.

---

## Flow 2: Clock in

```text
ClockIn_Click
  → _currentUser null? → "Sign in first"
  → duplicate badge on _onFloor? → MessageBox
  → resolve name from _employeeDirectory or "Unknown badge"
  → _onFloor.Add(new FloorEmployee { ClockedInBy = current user })
  → RefreshList()
```

---

## Flow 3: Refresh list (important WPF pattern)

```csharp
OnFloorList.ItemsSource = null;
OnFloorList.ItemsSource = _onFloor.OrderBy(...).ToList();
```

ListBox often needs **ItemsSource reset** to redraw. Later MVVM uses `ObservableCollection` for auto-updates.

---

## Flow 4: Clear list

Confirm Yes/No → `_onFloor.Clear()` → `RefreshList()`.

---

## Floor Assistant mapping

| P01 | Floor Assistant |
|-----|-----------------|
| `MainWindow` | Feature `Views/*` |
| Sign-in panel | `SignIn_New` |
| `_currentUser` | `m_UserInfo` / `UserInfo` |
| `ListBox` | `DataGrid` + `DataTable` (later) |
| `MessageBox` | `MyMessageBox` |
| `PlantCode` | Login plant (1001, 1003) |

---

## Run

```powershell
dotnet run --project projects/P01-ClockInBoard/P01.App.csproj
```

Visual Studio: **P01.App** startup → **F5**.

| Step | Action |
|------|--------|
| 1 | Sign in `E001` or `E002` |
| 2 | Clock in `E101`–`E104` |
| 3 | Duplicate badge → blocked |
| 4 | Clear list → confirm |

---

## Acceptance checklist

- [ ] Explain XAML vs `.xaml.cs`
- [ ] Explain why clock-in starts disabled
- [ ] Explain `ItemsSource` refresh pattern
- [ ] F5 runs without errors

## FA homework

- [ ] Find `SignIn_New.xaml` in FA — list controls used
- [ ] Compare session fields to `m_UserInfo`

---

## Next

**P02** — same app idea but **shell + Frame + Pages** in `src/PracticeFA.App`.  
**P32** — `{Binding}` · **P05** — MVVM
