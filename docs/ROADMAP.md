# Practice Floor Assistant — learning roadmap

You already know **HTML/CSS/JS, React, Node, SQL, Python**. This doc maps those skills to **WPF + C# + SQL Server** the same way Floor Assistant (FA) is built.

Open and run: `PracticeFA.slnx` → set **PracticeFA.App** as startup → **F5**.

---

## Learning order (four pillars)

1. **C#** → 2. **WPF** → 3. **SQL + ADO.NET** → 4. **MVVM**

Do not put MVVM before SQL (bindings are easier once you’ve loaded a grid from a proc). Full project list: [LEARNING_PATHWAY.md](LEARNING_PATHWAY.md).

---

## 1. Mental model: web stack → FA stack

| You know (web) | Floor Assistant equivalent | Practice project |
|----------------|---------------------------|------------------|
| `index.html` + CSS | `.xaml` (UI markup) | `MainWindow.xaml` |
| React component JSX | XAML + code-behind or View | `Pages/*.xaml`, `Views/*` |
| `useState` / Redux | `INotifyPropertyChanged`, `clsDictionary.UserInfo` | Tier 1 ViewModels |
| `onClick` handler | `Button.Click` or `RelayCommand` | Tier 0 code-behind → Tier 1 commands |
| React Router | `Frame` navigates to `Page` | Tier 0 #2 |
| Modal / new route | `Window.Show()` / `ShowDialog()` | Tier 0 #3 |
| Express API route | Stored procedure via `clsAccess.ExecSP` | Tier 2 #6–8 |
| `fetch` + JSON | `SqlDataAdapter.Fill` → `DataTable` | Tier 2 #9 |
| Sequelize / Prisma entities | **Often NOT used** — grids bind `DataTable` | FA pattern |
| `session` / JWT cookie | `clsDictionary.UserInfo` after `SignIn_New` | Tier 2 #6, Tier 3 #10 |
| Middleware `auth` | `CheckAuth(moduleId)` | Tier 2 #7 |
| OpenAI in a service file | `AiOrchestrationService` → DAL | Tier 5 #17–18 |
| SAP / printer npm package | `SAPRFCHandler`, `PrinterService` | Tier 5 #14–15 |

**Big idea:** FA is a **desktop React app** where “API” = **SQL Server stored procedures** returning **tables**, not JSON entities.

---

## 2. FA startup flow (what you will reproduce)

```
App.xaml
  → Splash (DB check, version)
  → SignIn (SP: employee details → UserInfo)
  → MainWindow (menu, status bar)
       → Frame → Page hub (Master, Production, …)
            → Button opens Window (feature screen)
                 → Grid bound to DataTable
                 → Save → ExecSP
```

Your **Tier 3 project #10** is a miniature of this chain.

---

## 3. Phased build order (weeks)

### Phase 1 — Language & UI (1–2 weeks)

| # | Build | FA learns |
|---|--------|-----------|
| 1 | Clock-in board | XAML, Window, ListBox — **done in repo** |
| 2 | Frame + 2 hub pages | `MainWindow` + `Pages/MasterPage`, `ReportsPage` |
| 3 | Module launcher grid | `ShowDialog()` for feature windows |

**C# essentials:** classes, `List<T>`, `async/await` (UI thread later), namespaces, `partial` class for XAML.

**Run:** `dotnet build` then F5 in Visual Studio.

### Phase 2 — Data (1 week)

| # | Build | FA learns |
|---|--------|-----------|
| 6 | Real login | `SqlConnection`, `DataTable`, map `DataRow` → `UserInfo` |
| 7 | Role menu | `spGetUserModules` → hide buttons |
| 8 | CRUD SPs only | `DataAccess.ExecSp` like `clsAccess` |
| 9 | Grid ← `DataTable` | Legacy FA binding |

**SQL practice:** In SSMS, run `PChk_M04_010_EmployeeDetails` from FA’s `SignIn_New` → compare columns to `m_UserInfo` in code.

### Phase 3 — Shell (few days)

| # | Build | FA learns |
|---|--------|-----------|
| 10 | Mini FA | Splash + login + Frame + status “SQL OK” |

### Phase 4 — One business vertical (ongoing)

Pick **one** of: Bagging lite (#11), Quotation lite (#12), MIS lite (#13).

### Phase 5 — Modern FA style

| # | Build | FA learns |
|---|--------|-----------|
| 4 | Attendance MVVM | `INotifyPropertyChanged`, `RelayCommand` |
| 5 | Settings → JSON in `%AppData%` | User prefs |
| 20–22 | Async, logging, refactor | Production hygiene |

### Phase 6 — Integrations (one at a time)

SAP mock, printer, scale, chat, webhook, email — **each in its own `Services/*.cs`**, never in XAML.cs.

---

## 4. Fastest path (if you are in a hurry)

```
1 → 2 → 4 → 6 → 7 → 10 → 11 or 12 → 14 → 15 → 17
```

Skip 3, 5, 8–9, 13, 16–22 until work needs them.

---

## 5. Domain cheat sheet (jewelry factory)

- **Style** = design identity; **SKU** = manufacturable variant.
- **Production order (PO)** = internal make order (not diamond purchase order).
- **Work center** = floor stage (WAXINJET → CASTING → FSK → RFD).
- **Bagging** = diamond bags with barcodes; **SPO** = semi-finished stock unit.

You do **not** need every module in FA—learn one vertical deeply.

---

## 6. “Am I ready to contribute?” checklist

- [ ] Build `FloorAssistant_V01.sln` (x86 Debug)
- [ ] Explain App → Splash → MainWindow → SignIn → Page → View
- [ ] Trace one screen’s SQL (SP name in C#)
- [ ] Read one `Pages/*.xaml.cs` button handler
- [ ] Compare one ViewModel screen vs one legacy view
- [ ] Find `clsDictionary.UserInfo` and `CheckAuth`
- [ ] Know new AI code goes through `AiOrchestrationService`

---

## 7. Folder layout (target as you grow)

```
PracticeFA/
  src/PracticeFA.App/
    App.xaml
    MainWindow.xaml          # Shell (later)
    Pages/                   # Frame navigation hubs
    Views/                   # Feature windows
    ViewModels/              # Tier 1+
    Models/                  # UserInfo, DTOs
    Services/                # DataAccess, Chat, Erp
    Resources/
  database/
    001_tables.sql
    002_stored_procs.sql
  docs/
    ROADMAP.md
```

---

## 8. Key WPF topics (with links to study)

- [Data binding overview](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/)
- [Frame navigation](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/frame)
- [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/inotifypropertychanged)
- [Async and UI thread](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/threading-model)
- ADO.NET: `SqlConnection`, `SqlDataAdapter.Fill`, `DataTable`

---

## 9. What to do today

1. Run **Tier 0 #1** (clock-in board).
2. Read `MainWindow.xaml` + `.xaml.cs` side by side (like `.jsx` + handler file).
3. Implement **Tier 0 #2** yourself: left menu buttons, right `Frame` → two pages.

When #2 works, you understand FA’s **MainWindow + Pages** pattern.
