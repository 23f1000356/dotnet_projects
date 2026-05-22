# Complete learning pathway — Floor Assistant tech stack

**Audience:** You know HTML/CSS/JS, React, Node, SQL, Python. Goal: contribute to **Floor Assistant QA** (WPF, .NET Framework 4.8, SQL Server, SPs, SAP x86).

**Practice repo:** `PracticeFA` — build mini projects here first, then read the same pattern in `FloorAssistant_V01.sln`.

**Detailed specs for each project:** [projects.md](projects.md) (53 **P** code + 21 **S** SAP = 74 total)

**Legend:** ✅ = in repo or started | 🔲 = you build next

---

## Four pillars — correct order

Learn in this sequence (not WPF before C#):

| Order | Pillar | Why | Mini projects (min 3–4 each) |
|-------|--------|-----|------------------------------|
| **1** | **C#** | Every `.xaml.cs`, ViewModel, and DAL file is C# | **4:** P00, P29, P30, P31 |
| **2** | **WPF** | UI layer; needs language basics first | **7:** P01✅, P02–P04, P32–P34 |
| **3** | **SQL + ADO.NET** | FA “API” = stored procedures → `DataTable` | **8:** P06–P09, P24, P35, P36, P41 |
| **4** | **MVVM** | Pattern on top of WPF after code-behind makes sense | **5:** P05, P05b, P38–P40 |
| + | **Advanced / domain / integrations** | FA production features | P10, P11–P13, P42–P52 — see [projects.md](projects.md) |

Then combine: **P10** (shell) + **P11–P13** (one domain) + integrations (P14+).

**Fastest path with this order:** `P00 → P01 → P02 → P06 → P07 → P05 → P10 → P11`

### Projects by pillar (quick list) — **53 total**, see [projects.md](projects.md)

| Pillar | IDs | Count |
|--------|-----|-------|
| **C#** | P00, P29, P30, P31 | 4 |
| **WPF** | P01✅, P02–P04, P32–P34 | 7 |
| **SQL + ADO.NET** | P06–P09, P24, P35, P36, P41 | 8 |
| **MVVM** | P05, P05b, P38–P40 | 5 |
| **Capstone** | P10 | 1 |
| **Domain** | P11–P13, P46–P48 | 6 |
| **Advanced UI** | P20, P42–P44 | 4 |
| **Advanced async/charts** | P21, P45 | 2 |
| **Integrations** | P14–P19, P22–P28, P49–P52 | 16 |

---

## How to use this doc

1. Follow **phases in order**; do not skip Phase 1–3.
2. Each **mini project** has: goal, FA file/pattern, skills, done criteria.
3. On real FA: after each project, find **one** matching screen and trace SP → grid → save.
4. Target **.NET Framework 4.8 WPF** on FA; PracticeFA may use newer SDK — concepts are identical.

---

## Stack map → mini projects (master table)

| FA layer | Technology | Mini project # | Phase |
|----------|------------|----------------|-------|
| Core | C# basics | P00 Console + types | 0 |
| Core | .NET / assemblies | P01 (same app) | 0 |
| UI | WPF XAML Window | **P01** Clock-in board ✅ | 1 |
| UI | Frame + Pages | **P02** Hub navigation 🔲 | 1 |
| UI | Resource dictionaries | **P03** Shared theme | 1 |
| UI | Feature Windows | **P04** Module launcher | 1 |
| UI | DataGrid | **P09** Grid from DataTable | 2 |
| UI | Extended toolkit / busy | **P20** Loading overlay | 5 |
| UI | LiveCharts | **P21** Simple bar chart | 5 |
| UI | WebView2 | **P22** Embed local HTML | 6 |
| Pattern | MVVM | **P05** Attendance MVVM | 3 |
| Pattern | Behaviors | **P23** Watermark TextBox | 5 |
| Pattern | Hub → View | **P10** Mini FA shell | 4 |
| Data | SQL Server + SSMS | **P06** DB + login SP | 2 |
| Data | ADO.NET SqlConnection | **P06–P08** | 2 |
| Data | DataTable / DataSet | **P09** | 2 |
| Data | ExecSP / DAL | **P08** CRUD procs | 2 |
| Data | INI config (Profile.dll) | **P24** App.config + conn string | 2 |
| Auth | Session UserInfo | **P06** map DataRow | 2 |
| Auth | ModuleId / CheckAuth | **P07** Role menu | 2 |
| BG work | BackgroundWorker / async | **P20** | 5 |
| Domain | Header/detail save | **P11** Bagging lite | 4 |
| Domain | Quotation / commercial | **P12** Quotation lite | 4 |
| Domain | MIS read-only | **P13** MIS report | 4 |
| SAP | RFC x86 (mock first) | **P14** IErpService | 6 |
| Print | Labels / raw print | **P15** Printer picker | 6 |
| Print | BarTender (read-only learn) | — use FA Utility; no clone | — |
| Hardware | Scale (Mettler pattern) | **P16** Weight simulator | 6 |
| Reports | ClosedXML Excel | **P17** Export grid to xlsx | 6 |
| Reports | PDF (iText idea) | **P18** Simple PDF receipt | 6 |
| Reports | Crystal / SSRS | Read FA only; no mini clone | — |
| AI | OpenAI + DAL service | **P19** Chat service | 6 |
| AI | Webhook / n8n JSON | **P25** Design insights mock | 6 |
| Integrations | Email / SMS | **P26** SMTP send CSV | 6 |
| Ops | ClickOnce / deploy | **P27** Publish folder read | 7 |
| Ops | Tray / WinForms dialog | **P28** NotifyIcon + print dialog | 7 |
| Legacy | VB scLibrary (read-only) | Trace LOV popup in FA | 7 |
| QA | Trace one real SP | Lab: `PChk_M04_010_EmployeeDetails` | 2 |

---

## Phase 0 — C# language (3–5 days)

Not WPF yet. Removes friction when reading FA.

### P00 — Console “floor ticket”

| | |
|--|--|
| **Build** | Console app: read PO number, work center code, qty; print a fake routing line. |
| **Learn** | `class`, properties, `List<T>`, `Dictionary`, `switch`, namespaces, `string` interpolation. |
| **FA link** | Every screen is classes + lists. |
| **Done** | No `dynamic`; use typed models. |

**From JS:** TypeScript interfaces ≈ C# classes with properties.

---

## Phase 1 — WPF UI shell (1–2 weeks)

Matches: `App.xaml`, `MainWindow`, `Pages/`, `Views/`, `Assets/`.

### P01 — Employee clock-in board ✅

| | |
|--|--|
| **Build** | `PracticeFA.App` — fake login, `ListBox` of on-floor employees. |
| **Learn** | `Window`, `Grid`, `StackPanel`, `TextBox`, `Button`, code-behind, `MessageBox`. |
| **FA link** | Simplest screen before `SignIn_New`. |
| **Done** | Explain XAML vs `.xaml.cs` like JSX vs handlers. |

### P02 — Frame + hub pages 🔲

| | |
|--|--|
| **Build** | `MainWindow`: left buttons (Master, Reports); right `Frame` → `Pages/MasterPage.xaml`, `ReportsPage.xaml`. |
| **Learn** | `Frame.Navigate`, `Page`, URI navigation. |
| **FA link** | `MainWindowNew` + `Frame`. |
| **Done** | Draw diagram: shell vs page vs window. |

### P03 — Resource dictionary theme

| | |
|--|--|
| **Build** | `Assets/Theme.xaml`: colors, button style; merge in `App.xaml`. |
| **Learn** | `ResourceDictionary`, `{StaticResource}`, implicit styles. |
| **FA link** | `Assets/*.xaml` buttons, DataGrid styles. |
| **Done** | One styled `DataGrid` header without inline colors on every control. |

### P04 — Module launcher grid

| | |
|--|--|
| **Build** | Hub page: grid of buttons → each opens `Views/FeatureWindow.xaml` modal. |
| **Learn** | `Show()`, `ShowDialog()`, `DialogResult`, owner window. |
| **FA link** | `Pages/*.xaml` launchers → `Views/*.xaml`. |
| **Done** | 3 fake modules (Style, Bagging, MIS) open 3 windows. |

**Phase 1 checkpoint:** Explain **Frame (page)** vs **Window (feature)** without notes.

---

## Phase 2 — SQL Server + ADO.NET (1 week)

Matches: `DAL/`, `clsAccess`, `dbConnect`, `DataTable`, stored procedures only.

### P06 — Real login + UserInfo

| | |
|--|--|
| **Build** | SQL DB `PracticeFA`: tables `Users`, `Modules`, `UserAccess`. `spLogin`, `spGetUserModules`. SignIn window → fill `UserInfo` from `DataRow`. |
| **Learn** | `SqlConnection`, `SqlCommand`, `CommandType.StoredProcedure`, `SqlDataAdapter.Fill`, parameters. |
| **FA link** | `SignIn_New`, `m_UserInfo`, `PChk_M04_010_EmployeeDetails`. |
| **Lab** | Run same SP in SSMS; column names = C# properties. |
| **Done** | Zero inline SQL strings in C#. |

### P07 — Role-based menu

| | |
|--|--|
| **Build** | After login, `spGetUserModules` → enable/disable hub buttons by `ModuleId`. |
| **Learn** | Auth as data, not hard-coded `if (admin)`. |
| **FA link** | `CheckAuth`, numeric module IDs. |
| **Done** | Two test users see different menus. |

### P08 — Employee CRUD (SP only)

| | |
|--|--|
| **Build** | `DataAccess.ExecSp(name, params)` → `DataTable`. List + Add/Edit/Delete via `spGetEmployees`, `spInsEmployee`, etc. |
| **Learn** | DAL single entry point; output parameters optional. |
| **FA link** | `clsAccess.ExecSP`. |
| **Done** | One class `DataAccess`, all SQL through it. |

### P09 — DataGrid ← DataTable

| | |
|--|--|
| **Build** | Same list as P08: version A `ItemsSource = dt.DefaultView`; version B map to `ObservableCollection<EmployeeRow>`. |
| **Learn** | Both patterns exist in FA (legacy vs newer). |
| **FA link** | Most `Views/` grids. |
| **Done** | Can add a column in SSMS and see it in grid without C# model change (version A). |

### P24 — Connection config

| | |
|--|--|
| **Build** | `App.config` connection string; small class reads server/database (FA uses Profile.dll INI — same idea). |
| **Learn** | QA vs PRD connection switching. |
| **FA link** | `dbConnect`, profile INI. |
| **Done** | Change config only to point to LocalDB vs named instance. |

**Phase 2 checkpoint:** Write a proc in SSMS morning, call from WPF afternoon.

---

## Phase 3 — MVVM + modern screen style (1 week)

Matches: `ViewModels/`, `Models/`, `INotifyPropertyChanged`, RelayCommand.

### P05 — Attendance list (pure MVVM)

| | |
|--|--|
| **Build** | `AttendanceViewModel`: `ObservableCollection`, `INotifyPropertyChanged`, `RelayCommand` Refresh/Save; View = bindings only, empty `.xaml.cs`. |
| **Learn** | `DataContext`, `{Binding}`, commands vs `Click`. |
| **FA link** | Mounting inventory style screens. |
| **Done** | No business logic in click handlers. |

### P05b — Settings to JSON

| | |
|--|--|
| **Build** | Plant, printer, theme → `%AppData%/PracticeFA/settings.json`. |
| **Learn** | User prefs without SQL. |
| **FA link** | Settings / XML config patterns. |

**Phase 3 checkpoint:** Refactor P01 clock-in to MVVM (optional homework).

---

## Phase 4 — Mini Floor Assistant shell + one domain (2 weeks)

Matches: full app flow + one business vertical.

### P10 — Mini Floor Assistant

| | |
|--|--|
| **Build** | Combine P02+P06+P07+P08: Splash (fake delay + DB ping), Login, `MainWindow`+Frame, 2 hubs, status bar “SQL: green”, 1–2 child windows. |
| **Learn** | End-to-end lifecycle. |
| **FA link** | App → Splash → SignIn → MainWindow → Page → View. |
| **Done** | Portfolio demo #1. |

### P11 — Bagging lite (pick ONE domain)

| | |
|--|--|
| **Build** | Order # → line grid (SKU, qty, weight); save header+lines via procs; label string in `TextBox` preview. |
| **Learn** | Header/detail, validation messages. |
| **FA link** | Bagging entry, diamond bags. |

### P12 — Quotation lite (alternative to P11)

| | |
|--|--|
| **Build** | Customer dropdown from SQL; line grid; total; Export CSV. |
| **FA link** | Sales quotation. |

### P13 — MIS report lite (alternative)

| | |
|--|--|
| **Build** | Date range → `spReport_SalesSummary` → read-only grid; optional “email” = save CSV + `Process.Start` mailto. |
| **FA link** | Floor MIS read-only. |

**Phase 4 checkpoint:** Draw flow Menu → Page → Window → SP → grid for your module.

---

## Phase 5 — UI polish & background work (1 week)

Matches: Extended.Wpf.Toolkit, async UI, error handling.

### P20 — Async + busy overlay

| | |
|--|--|
| **Build** | `async` load 3s fake data; disable buttons; busy indicator (Toolkit or simple `IsEnabled` + text). |
| **Learn** | `async/await`, UI thread, `Task.Run` for heavy work. |
| **FA link** | `BackgroundWorker` in FA (same problem, older API). |
| **Done** | UI never freezes on slow SP. |

### P21 — LiveCharts bar chart

| | |
|--|--|
| **Build** | Bind chart to daily output fake data. |
| **FA link** | Analytics / MIS charts. |

### P23 — Attached behavior

| | |
|--|--|
| **Build** | `Microsoft.Xaml.Behaviors`: watermark on `TextBox`. |
| **FA link** | `Attached_Properties/`. |

---

## Phase 6 — Integrations (2–4 weeks, one at a time)

Matches: SAP, printers, scales, AI, Excel, webhooks.

| # | Project | FA stack | Done |
|---|---------|----------|------|
| P14 | `IErpService` + SQL view + JSON mock | SAP NCo, `SAPRFCHandler` | VM only talks to interface |
| P15 | Printer dropdown + print text/PDF | BarTender, `RawPrinterHelper` | Print “HELLO” to PDF or printer |
| P16 | “Read scale” → random or serial | Mettler | Weight fills textbox |
| P17 | ClosedXML export grid | ClosedXML in FA | `.xlsx` opens in Excel |
| P18 | Simple PDF (QuestPDF or iText pattern) | iText 8 | One-page report |
| P19 | `ChatService` + HttpClient → OpenAI/mock | OpenAI SDK, `DAL/AIServices` | No HTTP in View |
| P25 | POST webhook → show JSON | Design insights / n8n | DTO + cancellation token |
| P26 | SMTP attach CSV | Email patterns in FA | Test mailbox only |

**Do not build:** full Crystal Reports, SSRS, SAP NCo x86, BarTender — **use FA QA** to observe; replicate only interfaces/mocks locally.

---

## Phase 7 — Read real FA + ops (ongoing)

| Activity | FA target | Outcome |
|----------|-----------|---------|
| Build FA solution | `FloorAssistant_V01.sln`, **x86 Debug** | Clean build |
| Trace SignIn | `SignIn_New`, SP name in code | Map columns ↔ `UserInfo` |
| Read one Page | e.g. `Pages/BaggingPage.xaml.cs` | One button → which View |
| Read one View save | Grep `ExecSP` in that View | Param list ↔ SP |
| LOV popup | scLovPop usage (VB) | Understand legacy picker |
| ClickOnce | Publish settings in .csproj | Know update URL concept |
| P28 | `NotifyIcon` + `PrintDialog` (WinForms) | Tray + print dialog pattern |

---

## Fastest path (if time-boxed)

```
P00 → P01✅ → P02 → P05 → P06 → P07 → P10 → P11 or P12 → P14 → P15 → P19
```

Defer: P03, P04, P08–P09 (until stuck on grids), P13, P16–P18, P21–P28 until job needs them.

---

## Weekly schedule (12-week example)

| Week | Projects | Also on real FA |
|------|----------|-----------------|
| 1 | P00, P01✅, P02, P03 | Open solution; run x86 |
| 2 | P04, start P06 | Find `MainWindowNew`, one Page |
| 3 | P06, P07, P24 | Trace `SignIn_New` SP |
| 4 | P08, P09 | One `ExecSP` in a View you use |
| 5 | P05, P05b | Compare Mounting ViewModel vs legacy View |
| 6 | P10 | Draw startup diagram on paper |
| 7 | P11 or P12 | Read Bagging or Quotation menu only |
| 8 | P13 or finish domain | MIS screen: read-only only |
| 9 | P20, P21 | Find `BackgroundWorker` in FA |
| 10 | P14, P15 | Skim `SAPRFCHandler`, printer helper |
| 11 | P17, P19 | `AiOrchestrationService` grep |
| 12 | P27–P28 + checklist below | Fix one small QA bug with mentor |

---

## “Ready for FA QA” checklist

- [ ] Build **FloorAssistantQA** x86 Debug in Visual Studio
- [ ] Explain: App → Splash → MainWindow → SignIn → Page → View
- [ ] Run one SP in SSMS and find same fields in code
- [ ] Read one `Pages/*.xaml.cs` launcher
- [ ] Read one `Views/*.xaml.cs` save handler
- [ ] Explain `DataTable` vs ViewModel collection binding
- [ ] Locate `clsDictionary.UserInfo` (or equivalent session)
- [ ] Know where AI calls belong (`AiOrchestrationService` / DAL)
- [ ] Never put SQL connection string in a View file

---

## Practice repo folder (target structure)

```
PracticeFA/
  src/PracticeFA.App/
    App.xaml
    MainWindow.xaml
    Pages/           # P02, P10
    Views/           # P04, P11–P13
    ViewModels/      # P05
    Models/          # UserInfo, rows
    Services/        # DataAccess, Chat, Erp  # P08, P14, P19
    Assets/          # P03
    Attached/        # P23
  database/
    scripts/         # P06–P08
  docs/
    ROADMAP.md
    LEARNING_PATHWAY.md  ← this file
```

---

## What FA uses that you should **not** clone early

| Technology | Why defer |
|------------|-----------|
| SAP .NET Connector x86 | Needs SAP install + x86 discipline; mock P14 first |
| Crystal Reports / SSRS | Designer/licensing; read reports in FA only |
| BarTender | Commercial; learn print abstraction P15 |
| VB scLibrary / scLovPop | Legacy; read when you hit LOV screens |
| Office Interop Excel | COM pain; use ClosedXML P17 |
| IronOCR / Selenium | Niche; on demand |

---

## Next action in this repo

1. Complete **P02** (Frame + two pages) — see `docs/ROADMAP.md`.
2. Create `database/scripts/001_PracticeFA.sql` when starting **P06**.
3. Keep a log: `docs/my-progress.md` (optional) — SP names you traced in real FA.
