# PracticeFA — detailed project catalog

Complete build specs for every mini project. Finish these in order and you will understand how to **read, debug, and add features** to **Floor Assistant QA** (WPF, .NET Framework 4.8, SQL Server, stored procedures, partial MVVM).

**Repo:** `Sample_dotnet` / `PracticeFA.slnx`  
**Companion docs:** [LEARNING_PATHWAY.md](LEARNING_PATHWAY.md) (schedule), [ROADMAP.md](ROADMAP.md) (web → WPF map)

**Legend:** ✅ done in repo | 🔲 to build

---

## Weekly plan (12 weeks — example)

Use this calendar alongside the [Project index](#project-index) below: **PracticeFA** mini projects during the week, **Floor Assistant QA** homework on the real solution.

| Week | Focus | PracticeFA projects | Floor Assistant QA |
|------|--------|---------------------|-------------------|
| **1–2** | C# + `DataSet` / `DataTable` | P00, P29, P30, P31 · start P09 (DataGrid + `DataTable`) | Build solution x86 Debug; grep `DataTable` / `DataSet` in one View |
| **3** | SQL + SSMS + **Profile.dll** meaning | P06, P24, P07 · SSMS lab on sign-in SP | Trace `dbConnect` / Profile INI → server & DB; run `PChk_M04_010_EmployeeDetails` (or your sign-in SP) in SSMS |
| **4–5** | WPF + trace **Bagging** menu → one View | P01→P02→P32→P03→P04→P33→P34 · P08, P09 | `Pages/Bagging*` → pick one `Views/*` · draw Menu → Page → View; F5 through launcher |
| **6** | Debugging F5, **x86**, breakpoints on SAP call | P10 (mini shell) · P14 (ERP mock) | Set breakpoint before `SAPRFCHandler` / `ExecSP`; confirm **x86** platform; watch locals + Output window |
| **7–8** | SAP flow + **2–3 bagging screens** end-to-end | P11, P36, P47 · P14, P16 | Bagging entry + issue/return or stock scan: SP → grid → Save for **3** screens |
| **9** | Threading + auth / modules | P07, P20, P45 · P05, P38 | `CheckAuth`, `ModuleId`, `clsDictionary.UserInfo`; find `BackgroundWorker` or `async` in FA |
| **10** | **Production Entry** or **Sales** (one module) | P46 or P12 · P13 optional | Read one module only: Production Entry *or* Quotation/Sales — one Page + two Views |
| **11** | MVVM screen (**MountingMatching**) | P05, P38, P39, P40 · compare P44 | Open Mounting inventory / matching ViewModel screen; contrast with legacy bagging View |
| **12** | ClickOnce, deployment, team release process | P27, P28, P52 · P49 | Read ClickOnce publish settings; attend or document QA release; P52 FA trace doc for one feature |

### Notes for this plan

- **Profile.dll** — FA uses INI-style config for SQL server/database per environment (IT_QA, IT_PRD); PracticeFA uses `App.config` (P24) for the same idea.
- **Weeks 1–2** — Learn C# first, but touch `DataTable` early (P09) so week 3 SQL feels familiar.
- **Weeks 7–8** — On FA, “end-to-end” = open screen → load proc → edit → save proc → messagebox; document SP names in [Progress tracker](#progress-tracker).
- Adjust pace: add a buffer week if x86 build or VPN/DB access blocks FA homework.

### Combined FA developer + SAP (recommended)

Run **P-projects** (code) and **S-projects** (SAP GUI / RFC) in parallel. SAP GUI weeks align with mentor access to **QA SAP**.

| Week | FA / code focus | SAP focus (S-projects) |
|------|-----------------|-------------------------|
| 1 | P00, P29 · FA build x86 | S00 ERP + FA vs SAP |
| 2 | P30, P31 · DataTable grep | S01 QA logon, Easy Access |
| 3 | P06, P24 · Profile.dll | S02 `/nTCODE`, 3 favorites |
| 4 | P01, P02 | S03 MM basics (material, plant, sloc, batch) |
| 5 | P08, P09 · Bagging Page trace | S04 SFLR, LOOSE1 meaning |
| 6 | P10, P14 · debug RFC breakpoint | S05 YPACK + map to FA Bagging |
| 7 | P11 | S06 PP production order (concept) |
| 8 | P11, P36 | S07 cheat sheet template · S08 export FA rows |
| 9 | P07, P20 | S09 verify stock in SAP |
| 10 | P46 or P12 | S10 one documented FA → SAP flow |
| 11 | P05, P40 · Mounting MVVM | S11–S13 RFC, SAPSettings, return tables |
| 12 | P27, P52 | S14–S17 ping vs RFC, debug PULLFromSAP |
| 13–14 | P16, P17 | S15–S17 finish IT_ERR workflow |
| 15+ | Integrations | S18–S21 optional (RPA, SE37, ABAP path) |

**SAP-only visual roadmap:** S00 → S01–S06 → S07–S10 → S11–S17 → S18–S20 (optional) → S21 (optional ABAP role).

---

## Project index

### P-projects — PracticeFA code (53)

| Project | Title | Stack |
|---------|-------|-------|
| P00 | Console floor ticket ([`projects/P00-ConsoleFloorTicket`](../projects/P00-ConsoleFloorTicket/)) | C# |
| P01 | Employee clock-in board ✅ ([`projects/P01-ClockInBoard`](../projects/P01-ClockInBoard/)) | WPF |
| P02 | Frame + hub pages ✅ ([`src/PracticeFA.App`](../src/PracticeFA.App/)) | WPF |
| P03 | Resource dictionary theme | WPF |
| P04 | Module launcher → feature windows | WPF |
| P05 | Attendance list (pure MVVM) | MVVM |
| P05b | Settings screen (JSON) | MVVM |
| P06 | Real login + UserInfo | SQL + ADO.NET |
| P07 | Role-based menu | SQL + ADO.NET |
| P08 | Employee CRUD via stored procedures | SQL + ADO.NET |
| P09 | DataGrid ← DataTable | SQL + ADO.NET |
| P10 | Mini Floor Assistant | Capstone |
| P11 | Bagging lite | Domain |
| P12 | Quotation lite | Domain |
| P13 | MIS report lite | Domain |
| P14 | ERP service mock | Integration |
| P15 | Label / printer lite | Integration |
| P16 | Scale simulator | Integration |
| P17 | Excel export | Integration |
| P18 | PDF receipt | Integration |
| P19 | Chat assistant | Integration |
| P20 | Async + busy overlay | Advanced UI |
| P21 | LiveCharts | Advanced |
| P22 | WebView2 | Integration |
| P23 | Attached behavior | Integration |
| P24 | Connection config | SQL + ADO.NET |
| P25 | Webhook insights | Integration |
| P26 | Email report | Integration |
| P27 | ClickOnce (study) | Integration |
| P28 | Tray + print dialog | Integration |
| P29 | LINQ & collections lab ([`projects/P29-LinqCollectionsLab`](../projects/P29-LinqCollectionsLab/)) | C# |
| P30 | Interfaces + repository ([`projects/P30-InterfacesRepository`](../projects/P30-InterfacesRepository/)) | C# |
| P31 | Exceptions, file logging, JSON export ([`projects/P31-ExceptionsLogging`](../projects/P31-ExceptionsLogging/)) | C# |
| P32 | TwoWay binding lab | WPF |
| P33 | Validation & ErrorTemplate | WPF |
| P34 | UserControl + menu/toolbar | WPF |
| P35 | Search & filter stored procedure | SQL + ADO.NET |
| P36 | Transaction: header + lines save | SQL + ADO.NET |
| P38 | Value converters & multi-binding | MVVM |
| P39 | Dependency injection in WPF | MVVM |
| P40 | Master-detail MVVM | MVVM |
| P41 | Audit columns & soft delete | SQL + ADO.NET |
| P42 | TabControl wizard | Advanced UI |
| P43 | Global exception handler | Advanced UI |
| P44 | ListView master-detail (code-behind) | Advanced UI |
| P45 | Async SAP-style mock | Advanced |
| P46 | Work center routing screen | Domain |
| P47 | Stock scan simulation | Domain |
| P48 | Multi-plant switch | Domain |
| P49 | Structured logging (Serilog) | Integration |
| P50 | QR code on label | Integration |
| P51 | HTTP retry (Polly) | Integration |
| P52 | FA trace capstone (documentation) | Integration |

### S-projects — SAP GUI & FA↔SAP integration (21)

| Project | Title | SAP phase |
|---------|-------|-----------|
| S00 | ERP + FA vs SAP GUI (overview) | 0 |
| S01 | QA logon pad, client, language | 1 |
| S02 | Easy Access, command field, favorites | 1 |
| S03 | MM basics: material, plant, sloc, batch, UoM | 1 |
| S04 | Site meaning: SFLR, LOOSE1 | 1 |
| S05 | YPACK + map to FA Bagging screens | 1 |
| S06 | PP: production order concept | 1 |
| S07 | Cheat sheet: FA function module ↔ SAP | 2 |
| S08 | FA Bagging export 2–3 stock rows | 2 |
| S09 | Same stock verification in SAP | 2 |
| S10 | Documented flow: FA screen → SAP steps | 2 |
| S11 | RFC / BAPI concepts (study) | 3 |
| S12 | SAPSettings profiles (IT_QA, SAP_PRD) | 3 |
| S13 | Return tables IT_STK_QTY, IT_ERR | 3 |
| S14 | Ping vs RFC (status icon ≠ FM success) | 3 |
| S15 | Authorization for function modules | 3 |
| S16 | Debug PULLFromSAP (Bagging floor stock) | 3 |
| S17 | IT_ERR TYPE E → SAP functional handoff | 3 |
| S18 | RPA + SAP intro (optional) | 4 |
| S19 | SE37 display FM (read-only) | 4 |
| S20 | SE16 tables + ST22 dumps (read-only) | 4 |
| S21 | ABAP developer path (study only) | 5 |

Folder: [`projects/SAP/`](../projects/SAP/) · Template: [`sap-cheat-sheet-template.md`](sap-cheat-sheet-template.md)

**74 projects total** (53 P + 21 S) · P01 ✅ · S-projects are mostly FA/SAP homework, not console apps.

---

## How to use this document

1. Build projects in **pillar order** (C# → WPF → SQL → MVVM → capstone → domain → extras).
2. For each project: complete **Acceptance criteria**, then do **FA homework** on the real solution.
3. Tick boxes in [Progress tracker](#progress-tracker) at the bottom.
4. Do **not** skip SQL before MVVM — FA grids are driven by `DataTable` from procs.

### Projects per tech stack (minimum 3–4 each; more for advanced)

| Tech stack | Count | Project IDs |
|------------|-------|-------------|
| **1 — C#** | **4** | P00, P29, P30, P31 |
| **2 — WPF** | **7** | P01, P02, P03, P04, P32, P33, P34 |
| **3 — SQL + ADO.NET** | **8** | P06, P07, P08, P09, P24, P35, P36, P41 |
| **4 — MVVM** | **5** | P05, P05b, P38, P39, P40 |
| **Advanced UI (WPF)** | **4** | P42, P43, P44, P20 |
| **Advanced C# / async** | **2** | P45, P21 |
| **Capstone** | **1** | P10 |
| **Domain / factory** | **6** | P11, P12, P13, P46, P47, P48 |
| **Integrations & ops** | **16** | P14–P19, P22, P23, P25–P28, P49–P52 |
| **SAP GUI & FA↔SAP** | **21** | S00–S21 (parallel track) |

**Grand total: 74** (53 P + 21 S) · P01 ✅ done.

### Four pillars — build order

| Order | Pillar | Projects |
|-------|--------|----------|
| 1 | **C#** | P00 → P29 → P30 → P31 |
| 2 | **WPF** | P01 → P02 → P32 → P03 → P04 → P33 → P34 |
| 3 | **SQL + ADO.NET** | P06 → P24 → P07 → P08 → P09 → P35 → P36 → P41 |
| 4 | **MVVM** | P05 → P38 → P05b → P39 → P40 |
| 5 | **Capstone** | P10 |
| 6 | **Domain** | Pick 2+: P11 / P12 / P13 + P46–P48 |
| 7 | **Advanced + integrations** | As job requires (see tables below) |

### Recommended sequence

```text
P00 → P29 → P30 → P31
  → P01 → P02 → P32 → P03 → P04 → P33 → P34
  → P06 → P24 → P07 → P08 → P09 → P35 → P36 → P41
  → P05 → P38 → P05b → P39 → P40
  → P10 → P11 → P46
  → (P14–P52 as needed)
```

**Fast path (core FA only):** `P00 → P29 → P01 → P02 → P06 → P07 → P08 → P05 → P10 → P11`

---

# SAP learning track (S-projects)

Study exercises on **SAP GUI** and how **Floor Assistant** calls SAP. Do **S00–S06** with a mentor on **QA SAP**; do **S11–S17** when debugging `SAPRFCHandler` / `PULLFromSAP` in FA (x86).

| Priority | Focus | Time |
|----------|--------|------|
| 1 | FA: C#, WPF, Bagging → Views → RFC (P-projects) | Ongoing |
| 2 | SAP GUI Phase 1–2 (S01–S10) | ~2 months part-time |
| 3 | SAP Phase 3 RFC/errors (S11–S17) | ~1 month |
| 4 | RPA (S18) | Only if job requires |

**Resources:** Kama internal SAP user guides (best) · SAP openSAP “Introduction to SAP” · YouTube “SAP GUI navigation for beginners”.

---

## S00 — ERP + FA vs SAP GUI 🔲

| | |
|--|--|
| **Phase** | 0 |
| **Time** | 2–3 hours |
| **Goal** | Know what SAP is vs what FA does. |

**Learn:** ERP = back office (materials, finance, stock). FA = shop floor (scan, bag, label, timing). FA **reads/writes** SAP stock via RFC for some screens, not all data.

**Practice:** Draw two boxes: SAP | FA | SQL Server — what lives where?

**Done:** Explain to mentor: “YPACK is SAP; Bagging Floor Stock is FA calling ZKS_FM_*.”

---

## S01 — QA SAP logon 🔲

| | |
|--|--|
| **Phase** | 1 · Week 2 |
| **Learn** | Logon pad, client, language; land in Easy Access. |
| **Practice at Kama** | QA system logon with mentor credentials. |
| **Done** | Log in 3 days in a row without help. |

---

## S02 — Command field & favorites 🔲

| | |
|--|--|
| **Phase** | 1 · Week 3 |
| **Learn** | Enter, Save, Back, Exit; `/nTCODE`; favorites bar. |
| **Practice** | Add **3 favorites** (e.g. YPACK, one MM report, one menu you use). |
| **Done** | Open YPACK from command field `/nYPACK` (code as mentor confirms). |

---

## S03 — MM basics 🔲

| | |
|--|--|
| **Phase** | 1 · Week 4 |
| **Learn** | Material master, plant (WERKS), storage location (LGORT), batch (CHARG), UoM. |
| **Practice** | MM03 or equivalent — find any diamond/material row fields. |
| **Done** | Define plant, sloc, batch, material in your own words (one paragraph in notes). |

---

## S04 — SFLR & LOOSE1 at your site 🔲

| | |
|--|--|
| **Phase** | 1 · Week 4–5 |
| **Learn** | What **SFLR** storage location and **LOOSE1** batch mean at Kama. |
| **Practice** | Ask functional/consultant; note in cheat sheet. |
| **Done** | Written definition + screenshot or field list from SAP. |

---

## S05 — YPACK + FA Bagging map 🔲

| | |
|--|--|
| **Phase** | 1 · Week 5 |
| **Learn** | Packing/bagging T-codes: YPACK, related Z/Y codes. |
| **Practice** | Open YPACK with guide; list FA screens: Bagging entry, floor stock, issue/return. |
| **Done** | Table: SAP T-code | FA screen name | same data? Y/N |

---

## S06 — PP production order (concept) 🔲

| | |
|--|--|
| **Phase** | 1 · Week 6 |
| **Learn** | PP order = what factory makes; link to casting/bagging (not full PP config). |
| **Practice** | CO03 or mentor T-code — view one PO header. |
| **Done** | Link diagram: Customer PO → internal PO → wax/cast/bag (from FA glossary). |

**Phase 1 outcome:** Open QA SAP, run **YPACK** and one stock T-code team names.

---

## S07 — FA FM ↔ SAP cheat sheet 🔲

| | |
|--|--|
| **Phase** | 2 · Week 7 |
| **Learn** | Custom `ZKS_FM_*` = custom ABAP; FA only calls via RFC. |
| **Practice** | Copy [`sap-cheat-sheet-template.md`](sap-cheat-sheet-template.md) → `docs/sap/bagging-floor-stock.md`. |
| **Done** | Template filled for one FM name (even if incomplete). |

---

## S08 — Export FA bagging rows 🔲

| | |
|--|--|
| **Phase** | 2 · Week 8 |
| **Learn** | Grid columns: MATERIAL, quality, size, qty → maps to SAP material/stock. |
| **Practice** | FA **Bagging Floor Stock Taking** (or status screen) — note **2–3 rows** (screenshot/export). |
| **Done** | 3 rows with plant, sloc, batch, material, qty documented. |

---

## S09 — Verify same stock in SAP 🔲

| | |
|--|--|
| **Phase** | 2 · Week 8–9 |
| **Learn** | `ZKS_FM_STK_QTY` → `IT_STK_QTY` concept. |
| **Practice** | With colleague, find same stock in SAP (MM or custom Z report). |
| **Done** | Match or explain mismatch for all 3 rows. |

---

## S10 — One documented FA → SAP flow 🔲

| | |
|--|--|
| **Phase** | 2 · Week 10 |
| **Learn** | End-to-end verification discipline. |
| **Practice** | Finalize cheat sheet: steps 1–2–3–4 for one screen. |
| **Done** | Mentor sign-off or dated note “verified on QA”. |

**Phase 2 outcome:** One flow: **FA screen → SAP verification steps** (document in repo `docs/sap/`).

---

## S11 — RFC / BAPI concepts 🔲

| | |
|--|--|
| **Phase** | 3 · Week 11 |
| **Learn** | RFC = remote function call; FA `PULLFromSAP` = call SAP from C#. |
| **Practice** | Read wrapper `SAPConnector` / `SAPRFCHandler` overview (no code change). |
| **Done** | Explain RFC in one sentence; name one FM FA calls. |

---

## S12 — SAPSettings profiles 🔲

| | |
|--|--|
| **Phase** | 3 |
| **Learn** | `SAPSettings` XML: host, client, user per **IT_QA** / **SAP_PRD**. |
| **Practice** | Find file in FA deploy; identify QA vs PRD (no passwords in git). |
| **Done** | List which profile FA uses in QA Debug. |

---

## S13 — Return tables IT_STK_QTY, IT_ERR 🔲

| | |
|--|--|
| **Phase** | 3 |
| **Learn** | Tables returned from FM; TYPE **E** = error, **S** = success. |
| **Practice** | Watch table in debugger during RFC call. |
| **Done** | Screenshot or sketch of table shapes in notebook. |

---

## S14 — Ping vs RFC success 🔲

| | |
|--|--|
| **Phase** | 3 |
| **Learn** | Green SAP icon / ping ≠ FM executed successfully. |
| **Practice** | Compare ping handler vs `PULLFromSAP` in FA status bar. |
| **Done** | Describe two failure modes: network down vs FM returned E. |

---

## S15 — Authorization for function modules 🔲

| | |
|--|--|
| **Phase** | 3 |
| **Learn** | SAP user needs auth object for each FM/T-code. |
| **Practice** | Ask functional what happens when auth missing (message pattern). |
| **Done** | Note “auth error” example text for handoff. |

---

## S16 — Debug PULLFromSAP (Bagging floor stock) 🔲

| | |
|--|--|
| **Phase** | 3 · Week 12 |
| **Learn** | Breakpoint in `BaggingFloorStockStatus` (or equivalent) before/after `PULLFromSAP`. |
| **Practice** | x86 Debug, F5, inspect parameters and return tables. |
| **Done** | One successful debug session notes in `my-progress.md`. |

---

## S17 — IT_ERR TYPE E handoff 🔲

| | |
|--|--|
| **Phase** | 3 |
| **Learn** | Copy SAP message to functional; you don’t fix ABAP as WPF dev. |
| **Practice** | Force or capture one E message; email/ ticket to SAP support. |
| **Done** | Ticket template used once. |

**Phase 3 outcome:** Debug SAP failures in FA **without SE38**.

---

## S18 — RPA + SAP intro (optional) 🔲

| | |
|--|--|
| **Phase** | 4 |
| **Learn** | UiPath SAP activities, selectors, retry, queues; RPA_Titan if at Kama. |
| **Done** | Watch one recorded flow or document one simple GUI automation idea. |

---

## S19 — SE37 display FM (read-only) 🔲

| | |
|--|--|
| **Phase** | 4 |
| **Learn** | SE37: see import/export tables for `ZKS_FM_*` (display only). |
| **Done** | Print/export FM interface screenshot for one FM from cheat sheet. |

---

## S20 — SE16 & ST22 (read-only) 🔲

| | |
|--|--|
| **Phase** | 4 |
| **Learn** | SE16 table browse; ST22 short dump when SAP errors. |
| **Done** | Find table name for stock (mentor hint); open one ST22 dump description. |

---

## S21 — ABAP path (study only) 🔲

| | |
|--|--|
| **Phase** | 5 · 6+ months |
| **Learn** | SE38, SE11, ABAP debugger — **SAP ABAP consultant role**, not required for FA WPF dev. |
| **Done** | Decision note: stay FA dev vs pursue ABAP (with mentor). |

---

### SAP weekly checklist (first 4 weeks)

| Week | Task | S-project |
|------|------|-----------|
| 1 | Get QA SAP access; log in; explore Easy Access | S00, S01 |
| 2 | Command field; run YPACK with guide | S02, S05 |
| 3 | Document plant, storage loc, batch for your site | S03, S04 |
| 4 | Match one FA bagging screen to one SAP check | S08, S09 |

---

# Pillar 1 — C#

## P00 — Console floor ticket 🔲

| Field | Detail |
|-------|--------|
| **Time** | 3–5 days |
| **Prerequisites** | None (you know JS/Python; SQL helps later) |
| **Project type** | Separate console app or `src/PracticeFA.Console` |
| **FA stack** | C# language used in every `.cs` file |

### Goal

Learn C# syntax without WPF noise. Model factory data as **typed classes**, not strings everywhere.

### What to build

1. Console app that prompts (or args):
   - Production order number (e.g. `PO-2026-001`)
   - Work center code (e.g. `CASTING`, `FSK`)
   - Quantity (int)
2. Validate: qty > 0, PO not empty.
3. Print a **routing line**, e.g.  
   `PO-2026-001 | CASTING | Qty 12 | Next: GRINDING`
4. Optional: in-memory `List<WorkCenter>` with codes and “next” WC (dictionary or small class).

### Files to create

```
src/PracticeFA.Console/
  Program.cs
  Models/ProductionTicket.cs
  Models/WorkCenter.cs
```

### Core C# topics

- `namespace`, `class`, auto-properties (`public string PoNumber { get; set; }`)
- `List<T>`, `Dictionary<string, string>`
- `switch` / `if`, `string interpolation` (`$"..."`)
- Methods returning `bool` for validation
- `Main` entry point

### FA mapping

| Practice | Floor Assistant |
|----------|-----------------|
| `ProductionTicket` class | Row models, `m_UserInfo`, parameter DTOs |
| Validation before print | Validation before `ExecSP` save |
| List of work centers | `AppVariables.WCList`, routing |

### Acceptance criteria

- [ ] Builds with `dotnet build` with zero warnings you care about
- [ ] Invalid qty shows message and does not print ticket
- [ ] At least two work center codes resolve to a “next” step
- [ ] No `dynamic`; all data in named types

### FA homework

- [ ] Open any `Views/*.xaml.cs` — find one `class` and three properties on a model or variable
- [ ] Grep FA for `List<` in one file you will use later

### Stretch

- Read PO + WC from a JSON file
- Unit test validation with xUnit (optional)

---

## P29 — LINQ & collections lab 🔲

| Field | Detail |
|-------|--------|
| **Time** | 2 days |
| **Prerequisites** | P00 |
| **FA stack** | LINQ in reports, in-memory filtering before grid bind |

### Goal

Filter and sort factory lists the way FA code uses **LINQ** on collections and sometimes `DataTable.AsEnumerable()`.

### What to build

Console or small WPF-less class library:

1. `List<ProductionTicket>` with 10+ sample rows (PO, WC, qty, date).
2. Queries: all `CASTING`; qty > 5; order by date; `GroupBy` WC → count.
3. Print results; use **method syntax** and **query syntax** at least once each.

### Acceptance criteria

- [ ] 4 different LINQ queries with correct output
- [ ] No SQL — in-memory only

### FA homework

- [ ] Grep FA for `.Where(` or `from ` in one ViewModel

---

## P30 — Interfaces + repository (in-memory) 🔲

| Field | Detail |
|-------|--------|
| **Time** | 2–3 days |
| **Prerequisites** | P29 |
| **FA stack** | Prep for `IDataAccess`, testable services, SAP/AI facades |

### Goal

**Program to an interface** — same pattern as `IErpService`, `DataAccess` abstraction.

### What to build

1. `IEmployeeRepository` — `GetAll`, `GetByBadge`, `Add`, `Update`.
2. `InMemoryEmployeeRepository` — `List<Employee>` backing store.
3. `EmployeeService` — calls interface only; console menu drives CRUD.
4. Optional: `SqlEmployeeRepository` stub throwing `NotImplementedException` (filled in P08).

### Acceptance criteria

- [ ] Service constructor takes `IEmployeeRepository` (DI-ready)
- [ ] Swap in-memory vs SQL stub without changing service methods

### FA homework

- [ ] Find one `interface I*` in FA `Helpers/` or `DAL/`

---

## P31 — Exceptions, file logging, JSON export 🔲

| Field | Detail |
|-------|--------|
| **Time** | 2 days |
| **Prerequisites** | P30 |
| **FA stack** | `try/catch`, user-friendly errors, debug logs |

### Goal

Handle failures like production apps: log detail, show simple message to user.

### What to build

1. Wrap P30 menu in `try/catch` — log exception to `logs/practice-{date}.txt`.
2. `JsonSerializer.Serialize` employees to `export/employees.json` on demand.
3. Custom exception `ValidationException` for bad badge with clear message.

### Acceptance criteria

- [ ] Forced error appends to log file with stack trace
- [ ] JSON round-trip: export then import into new list

### FA homework

- [ ] Find `MyMessageBox` or catch block in one FA View save handler

---

# Pillar 2 — WPF

## P01 — Employee clock-in board ✅

| Field | Detail |
|-------|--------|
| **Time** | 1–2 days |
| **Prerequisites** | P00 recommended |
| **Location** | `src/PracticeFA.App/` (already in repo) |
| **FA stack** | WPF `Window`, XAML, code-behind |

### Goal

First WPF screen: markup + C# handlers, in-memory state (no database).

### What is built

- Fake sign-in (badge → `UserSession`)
- Clock-in adds `Employee` to `ListBox`
- Duplicate badge blocked; clear list button

### Key files

- `MainWindow.xaml`, `MainWindow.xaml.cs`
- `Models/Employee.cs`, `Models/UserSession.cs`

### FA mapping

| Practice | Floor Assistant |
|----------|-----------------|
| `MainWindow` | Any feature `Window` / `Views/*` |
| `ListBox` + code-behind | Simpler than `DataGrid` + `DataTable` |
| Session text on screen | `clsDictionary.UserInfo` after login |

### Acceptance criteria

- [x] F5 runs app
- [ ] You can explain XAML vs `.xaml.cs` to someone else
- [ ] You can add a third demo user without help

### FA homework

- [ ] Find `SignIn_New.xaml` (or equivalent) in FA — list controls used (TextBox, Button, …)

---

## P02 — Frame + hub pages 🔲

| Field | Detail |
|-------|--------|
| **Time** | 2–3 days |
| **Prerequisites** | P01 |
| **FA stack** | `Frame`, `Page`, shell navigation |

### Goal

Reproduce FA shell: **one main window**, content area swaps **pages** (not new app instances).

### What to build

1. Refactor shell:
   - `MainWindow`: left column = menu buttons **Master**, **Reports**, **Exit**
   - Right column = `<Frame x:Name="MainFrame" />`
2. `Pages/MasterPage.xaml` — `Page` with title + short description + placeholder buttons
3. `Pages/ReportsPage.xaml` — `Page` with “reports go here”
4. Navigation:
   - `MainFrame.Navigate(new MasterPage())` or URI `Navigate(new Uri("/Pages/MasterPage.xaml", UriKind.Relative))`
5. **Exit** closes app; optional: confirm if list unsaved (skip for now).

### Files to create

```
src/PracticeFA.App/
  MainWindow.xaml          (shell only)
  Pages/MasterPage.xaml
  Pages/MasterPage.xaml.cs
  Pages/ReportsPage.xaml
  Pages/ReportsPage.xaml.cs
```

### WPF topics

- `Page` vs `Window`
- `Frame.Navigate`, navigation history (optional Back button)
- `Grid` column layout

### FA mapping

| Practice | Floor Assistant |
|----------|-----------------|
| `MainWindow` + `Frame` | `MainWindowNew` + menu hub |
| `Pages/MasterPage` | `Pages/*Page.xaml` launchers (Master, Bagging, …) |
| Menu buttons | Module menu / tree |

### Acceptance criteria

- [ ] Master and Reports swap in same window without closing app
- [ ] Can draw on paper: **Shell → Page → (later) Window**
- [ ] Window title stays on main shell

### FA homework

- [ ] Open `MainWindowNew.xaml` — locate `Frame`
- [ ] Open one `Pages/BaggingPage.xaml.cs` — find `Navigate` or `new SomeView()`

---

## P03 — Resource dictionary theme ✅

| Field | Detail |
|-------|--------|
| **Time** | 1–2 days |
| **Prerequisites** | P02 |
| **FA stack** | `Assets/*.xaml`, shared styles |

### Goal

Centralize look-and-feel like FA resource dictionaries (buttons, colors, DataGrid header).

### What to build

1. `Assets/Theme.xaml`:
   - Colors: `PrimaryBrush`, `SurfaceBrush`, `ErrorBrush`
   - `Style TargetType="Button"` — padding, corner radius, template optional
   - `Style TargetType="DataGridColumnHeader"` — background, font weight
2. Merge in `App.xaml`:
   ```xml
   <Application.Resources>
     <ResourceDictionary>
       <ResourceDictionary.MergedDictionaries>
         <ResourceDictionary Source="Assets/Theme.xaml"/>
       </ResourceDictionary.MergedDictionaries>
     </ResourceDictionary>
   </Application.Resources>
   ```
3. Apply to P02 pages: buttons use implicit style; one `DataGrid` on Reports page with 3 fake columns.

### FA mapping

| Practice | Floor Assistant |
|----------|-----------------|
| `Assets/Theme.xaml` | `Assets/*.xaml` button/tab/grid themes |
| `{StaticResource Key}` | Shared styles across hundreds of screens |

### Acceptance criteria

- [ ] Change primary color in **one** place → all buttons update
- [ ] DataGrid headers styled without per-column XAML
- [ ] No copy-paste of same `Background="#..."` on every button

### FA homework

- [ ] Open FA `App.xaml` — list merged dictionaries
- [ ] Find one `{StaticResource ...}` in a FA View

---

## P04 — Module launcher → feature windows ✅

| Field | Detail |
|-------|--------|
| **Time** | 2–3 days |
| **Prerequisites** | P02, P03 helpful |
| **FA stack** | `Pages/` → `Views/`, modal dialogs |

### Goal

Hub page opens **feature windows** — the most common FA pattern after menu click.

### What to build

1. On `MasterPage`: `UniformGrid` or `WrapPanel` with buttons:
   - **Style Creation** (Module 1001 fake)
   - **Bagging Entry** (2001)
   - **MIS Productivity** (3001)
2. Each opens `Views/StyleWindow.xaml`, `BaggingWindow.xaml`, `MisWindow.xaml`:
   - `new StyleWindow { Owner = mainWindow }.ShowDialog();`
   - Simple content: title + TextBox + Save/Cancel
   - Cancel sets `DialogResult = false`
3. Pass **module id** via constructor or property if you want extra practice.

### Files

```
Views/StyleWindow.xaml(.cs)
Views/BaggingWindow.xaml(.cs)
Views/MisWindow.xaml(.cs)
```

### FA mapping

| Practice | Floor Assistant |
|----------|-----------------|
| `Pages/*` launcher | `Pages/BaggingPage`, `MasterPage`, … |
| `Views/*` feature | Hundreds of `Views/*.xaml` screens |
| `ShowDialog()` | Modal edits, confirmations |

### Acceptance criteria

- [ ] Three modules open three different windows
- [ ] Dialog blocks hub until closed (modal)
- [ ] Owner set so dialogs center on main window

### FA homework

- [ ] From one FA Page, grep `.Show` / `ShowDialog` / `new *View`
- [ ] Trace: button click → which View class opens

---

## P32 — TwoWay binding lab ✅

| Field | Detail |
|-------|--------|
| **Time** | 1–2 days |
| **Prerequisites** | P02 |
| **FA stack** | Form fields bound to object properties before MVVM toolkit |

### Goal

Bind UI to a **data object** in code-behind using `INotifyPropertyChanged` (manual, no toolkit yet).

### What to build

1. `Models/EmployeeEditModel.cs` implements `INotifyPropertyChanged`.
2. Window: TextBoxes `Text="{Binding DisplayName, UpdateSourceTrigger=PropertyChanged}"`, etc.
3. Set `DataContext` in code-behind; live preview `TextBlock` shows bound values.
4. “Revert” button resets model from saved snapshot.

### Acceptance criteria

- [ ] Typing in TextBox updates preview without clicking Save
- [ ] You can explain `DataContext`, `Binding`, `INotifyPropertyChanged`

### FA homework

- [ ] Find `{Binding` in one FA XAML file (non-MVVM screen)

---

## P33 — Validation & ErrorTemplate ✅

| Field | Detail |
|-------|--------|
| **Time** | 2 days |
| **Prerequisites** | P32 |
| **FA stack** | Required fields before save on FA forms |

### Goal

WPF **ValidationRules** or `IDataErrorInfo` / `INotifyDataErrorInfo` — block save when invalid.

### What to build

1. Extend P32 screen: badge required, qty 1–999, numeric only.
2. Red border / error adorner on invalid fields.
3. Save disabled until `Validation.GetHasError` is false (or VM equivalent).

### Acceptance criteria

- [ ] Empty badge cannot save
- [ ] Error clears when user fixes field

### FA homework

- [ ] Find validation message or `MessageBox` on empty field in FA View

---

## P34 — UserControl + menu/toolbar ✅

| Field | Detail |
|-------|--------|
| **Time** | 2 days |
| **Prerequisites** | P03, P04 |
| **FA stack** | Reusable chunks, main menu/toolbars in `MainWindow` |

### Goal

Extract reusable UI (**UserControl**) and add **Menu** / **ToolBar** like FA shell.

### What to build

1. `Controls/EmployeeSearchBox.xaml` — badge TextBox + Search button; dependency property `BadgeText`.
2. Host control on Master page and Bagging window.
3. `MainWindow` menu: File → Exit, View → Master/Reports, Help → About.
4. Toolbar: Refresh, Save icons (text buttons OK).

### Acceptance criteria

- [ ] Same UserControl works on two parents with different `DataContext`
- [ ] Menu navigates same as left panel buttons

### FA homework

- [ ] Find `.xaml` UserControl in FA `Views/` or `Controls/`

**WPF pillar checkpoint:** 7 projects (P01–P04, P32–P34) — explain Frame vs Window vs UserControl.

---

# Pillar 3 — SQL + ADO.NET

## P06 — Real login + UserInfo ✅

| Field | Detail |
|-------|--------|
| **Time** | 3–4 days |
| **Prerequisites** | P02, SQL Server / LocalDB installed, SSMS |
| **FA stack** | `SignIn_New`, `DAL`, `SqlConnection`, `DataTable`, SPs |

### Goal

Replace fake login with **real database** and **stored procedure**, mapping result to a session object like `m_UserInfo`.

### Database schema (minimum)

```sql
-- database/scripts/001_PracticeFA.sql
CREATE TABLE Users (
  UserId       NVARCHAR(20) PRIMARY KEY,
  PasswordHash NVARCHAR(100) NOT NULL,  -- plain for practice only; FA uses real auth
  DisplayName  NVARCHAR(100) NOT NULL,
  PlantCode    NVARCHAR(10) NOT NULL
);
CREATE TABLE Modules (
  ModuleId   INT PRIMARY KEY,
  ModuleName NVARCHAR(50) NOT NULL
);
CREATE TABLE UserAccess (
  UserId   NVARCHAR(20) REFERENCES Users(UserId),
  ModuleId INT REFERENCES Modules(ModuleId),
  PRIMARY KEY (UserId, ModuleId)
);
```

### Stored procedures

| Proc | Purpose |
|------|---------|
| `spLogin` | `@UserId`, `@Password` → one row: UserId, DisplayName, PlantCode (return empty if invalid) |
| `spGetUserModules` | `@UserId` → ModuleId, ModuleName (for P07) |

Seed: 2 users, 5 modules, different `UserAccess` rows.

### What to build (C#)

1. `Services/DataAccess.cs`:
   - `DataTable ExecSp(string procName, params SqlParameter[] parms)`
   - Uses `App.config` connection string
2. `Models/UserInfo.cs` — properties match **SP result columns**
3. `Views/SignInWindow.xaml` — UserId, Password, Login button
4. `MapRow(DataRow row) => new UserInfo { ... }` — explicit column names
5. On success: store in `AppState.CurrentUser` static or small session service; open main shell

### FA mapping

| Practice | Floor Assistant |
|----------|-----------------|
| `spLogin` | `PChk_M04_010_EmployeeDetails` or sign-in SPs in `SignIn_New` |
| `UserInfo` from `DataRow` | `m_UserInfo` population |
| `DataAccess.ExecSp` | `clsAccess.ExecSP` / `clsFA_DBC` |
| No SQL in UI | Same rule: Views call DAL only |

### Acceptance criteria

- [ ] SSMS: `EXEC spLogin` returns same shape as C# expects
- [ ] Bad password shows friendly message (not stack trace to user)
- [ ] `UserInfo` lives for session until Exit
- [ ] Zero `SELECT` / `INSERT` strings in XAML.cs — only `ExecSp`

### FA homework (critical)

- [ ] Run FA sign-in SP in SSMS with QA test user
- [ ] Compare column names to `m_UserInfo` properties in `SignIn_New.xaml.cs`
- [ ] Document SP name + 5 fields in `docs/my-progress.md`

### Stretch

- Hash password with SHA256 in SP or app (still practice-only)

---

## P07 — Role-based menu 🔲

| Field | Detail |
|-------|--------|
| **Time** | 1–2 days |
| **Prerequisites** | P06 |
| **FA stack** | `CheckAuth`, numeric `ModuleId` |

### Goal

Menu visibility from **database**, not `if (username == "admin")`.

### What to build

1. After login, call `spGetUserModules(@UserId)` → `DataTable`
2. Build `HashSet<int>` of allowed module IDs
3. On `MasterPage`, each launcher button has `Tag="1001"` (module id):
   - `Visibility=Collapsed` or `IsEnabled=false` if not in set
4. Show read-only list “Your modules” in status area for debugging

### FA mapping

| Practice | Floor Assistant |
|----------|-----------------|
| Module IDs on buttons | `ModuleId` constants / enums in FA |
| `CheckAuth(1001)` | Before opening sensitive Views |
| Per-user menu | Different operators see different hubs |

### Acceptance criteria

- [ ] User A sees Bagging; User B does not (with seed data)
- [ ] No hard-coded username checks in menu code
- [ ] Module list loaded once per login (cache in session)

### FA homework

- [ ] Grep FA for `CheckAuth` — note module id passed
- [ ] Find module id constant for one screen you care about (e.g. Style Creation)

---

## P08 — Employee CRUD via stored procedures 🔲

| Field | Detail |
|-------|--------|
| **Time** | 3–4 days |
| **Prerequisites** | P06, P07 |
| **FA stack** | `clsAccess.ExecSP`, list + save screens |

### Goal

Full **list / add / edit / delete** through procs only — the core FA data loop.

### Database

```sql
CREATE TABLE Employees (
  EmployeeId   INT IDENTITY PRIMARY KEY,
  BadgeId      NVARCHAR(20) NOT NULL UNIQUE,
  DisplayName  NVARCHAR(100) NOT NULL,
  ProcessCenter NVARCHAR(50) NULL,
  IsActive     BIT NOT NULL DEFAULT 1
);
-- spGetEmployees, spGetEmployeeById, spInsEmployee, spUpdEmployee, spDelEmployee (soft delete ok)
```

### What to build

1. Extend `DataAccess.ExecSp` — handle return values / output params if needed
2. `Views/EmployeeListWindow.xaml`:
   - DataGrid, Add, Edit, Delete, Refresh
3. Add/Edit dialog → `spIns` / `spUpd` with validation (badge required)
4. Delete → confirm → `spDel` or set `IsActive=0`
5. All DB calls from code-behind **or** thin presenter calling `DataAccess` only

### FA mapping

| Practice | Floor Assistant |
|----------|-----------------|
| CRUD procs | Every maintenance screen in Master/Utilities |
| Parameters | Match SP params exactly (types, lengths) |
| Refresh after save | `Fill` grid again — FA pattern |

### Acceptance criteria

- [ ] CRUD works with SQL Profiler showing only `EXEC sp...`
- [ ] Edit shows same data as SSMS row
- [ ] Delete removes row from grid after proc succeeds
- [ ] Errors shown via `MessageBox` + log message (file optional)

### FA homework

- [ ] Pick one FA maintenance View — find Save button → `ExecSP` name → run proc in SSMS

---

## P09 — DataGrid ← DataTable 🔲

| Field | Detail |
|-------|--------|
| **Time** | 2 days |
| **Prerequisites** | P08 |
| **FA stack** | `DataTable`, `DefaultView`, legacy grids |

### Goal

Master **both** FA binding styles: direct `DataTable` and ViewModel collection.

### What to build

**Version A — Legacy FA**

```csharp
var dt = DataAccess.ExecSp("spGetEmployees");
EmployeeGrid.ItemsSource = dt.DefaultView;
```

- Add column in SQL → appears in grid without new C# property class

**Version B — Modern**

- Map `DataRow` → `EmployeeRowViewModel` in a loop
- `ObservableCollection<EmployeeRowViewModel>` as `ItemsSource`
- Two tabs or two windows in same project for comparison

### FA mapping

| Practice | Floor Assistant |
|----------|-----------------|
| `dt.DefaultView` | Most older `Views/` |
| `ObservableCollection` | Mounting inventory, newer screens |
| AutoGenerateColumns | FA grids often bind with columns from metadata |

### Acceptance criteria

- [ ] Version A: add `Email` column in SP result — grid shows it without C# model change
- [ ] Version B: sorting/filtering still works on at least one column
- [ ] You can explain when FA team would pick A vs B

### FA homework

- [ ] Find one grid bound to `DataTable` / `DefaultView` in FA
- [ ] Find one using `ObservableCollection` or ItemsSource from VM

---

## P24 — Connection config 🔲

| Field | Detail |
|-------|--------|
| **Time** | 0.5–1 day |
| **Prerequisites** | P06 (can merge with P06) |
| **FA stack** | `dbConnect`, `Profile.dll` INI |

### Goal

Switch database target without recompiling — QA vs local.

### What to build

1. `App.config`:
   ```xml
   <connectionStrings>
     <add name="PracticeFA" connectionString="Server=.;Database=PracticeFA;Trusted_Connection=True;TrustServerCertificate=True" />
   </connectionStrings>
   ```
2. `Services/DbSettings.cs` — reads `ConfigurationManager.ConnectionStrings["PracticeFA"]`
3. Optional: second named connection `PracticeFA_QA` and toggle via environment variable or build config

### FA mapping

| Practice | Floor Assistant |
|----------|-----------------|
| `App.config` / INI | Profile.dll server/database per environment |
| Plant at login | Different DB/catalog per company code |

### Acceptance criteria

- [ ] `DataAccess` uses single settings class for connection string
- [ ] Document in README how to point to LocalDB vs named instance

### FA homework

- [ ] Find where FA reads SQL server name (Profile / config) — do not copy secrets

---

## P35 — Search & filter stored procedure 🔲

| Field | Detail |
|-------|--------|
| **Time** | 2 days |
| **Prerequisites** | P08 |
| **FA stack** | List screens with optional filters (date, plant, text) |

### Goal

Pass **optional parameters** to procs (`NULL` = no filter) — common in FA report/list SPs.

### What to build

1. `spSearchEmployees @BadgeFragment, @ProcessCenter, @ActiveOnly`.
2. WPF: three filters + Search button → grid refresh.
3. Show “No records” when empty result.

### Acceptance criteria

- [ ] NULL/empty params return broader set; all filters narrow result
- [ ] SSMS test matches WPF grid row count

### FA homework

- [ ] Find SP with optional `@Plant` or nullable params in FA screen you use

---

## P36 — Transaction: header + lines save 🔲

| Field | Detail |
|-------|--------|
| **Time** | 3 days |
| **Prerequisites** | P08, P09 |
| **FA stack** | Bagging/order save — one business action, one transaction |

### Goal

Save **header + multiple lines** atomically (rollback on any line failure).

### What to build

1. Tables: `OrderHeader`, `OrderLine` (or reuse bag tables from P11 early).
2. `spSaveOrder` — `BEGIN TRAN` … insert header … lines … `COMMIT` / `ROLLBACK`.
3. UI: one Save button; deliberately break line 2 in test — header must not persist.

### Acceptance criteria

- [ ] Failed line leaves DB unchanged (prove in SSMS)
- [ ] Success returns new OrderId to UI

### FA homework

- [ ] Ask mentor which FA save uses transaction or batch proc name

---

## P41 — Audit columns & soft delete 🔲

| Field | Detail |
|-------|--------|
| **Time** | 2 days |
| **Prerequisites** | P08 |
| **FA stack** | `CreatedBy`, `CreatedDate`, `IsActive` on FA tables |

### Goal

Track **who/when** and hide rows without physical delete.

### What to build

1. Add `CreatedBy`, `CreatedAt`, `ModifiedBy`, `ModifiedAt`, `IsActive` to Employees.
2. `spDelEmployee` sets `IsActive=0`; list proc filters `IsActive=1`.
3. Pass `@UserId` from session on insert/update (from P06).

### Acceptance criteria

- [ ] Deleted employee disappears from grid but exists in SSMS
- [ ] New row has CreatedBy = logged-in user

### FA homework

- [ ] Spot audit column names in one FA SP result set

**SQL pillar checkpoint:** 8 projects — write proc in SSMS and call same day.

---

# Pillar 4 — MVVM

## P05 — Attendance list (pure MVVM) 🔲

| Field | Detail |
|-------|--------|
| **Time** | 3–4 days |
| **Prerequisites** | P08, P09 Version B helpful |
| **FA stack** | `ViewModels/`, `INotifyPropertyChanged`, commands |

### Goal

Build one screen the **modern FA way**: View = bindings only; logic in ViewModel + services.

### NuGet (suggested)

- `CommunityToolkit.Mvvm` — `[ObservableProperty]`, `RelayCommand`

### What to build

1. `Models/AttendanceRecord.cs`
2. `ViewModels/AttendanceViewModel.cs`:
   - `ObservableCollection<AttendanceRecord> Records`
   - `IsBusy`, `StatusMessage`
   - `RefreshCommand` → `DataAccess.ExecSp("spGetAttendance")` → map rows
   - `SaveCommand` → validate → `spSaveAttendance` (create simple table + proc)
3. `Views/AttendanceView.xaml` — `DataContext` set in ctor or XAML
4. `AttendanceView.xaml.cs` — **empty** or only `InitializeComponent` + `DataContext = vm`

### Bindings example

- `ItemsSource="{Binding Records}"`
- `Command="{Binding RefreshCommand}"`
- `IsEnabled="{Binding IsBusy, Converter={StaticResource InverseBool}}"` (or disable when busy)

### FA mapping

| Practice | Floor Assistant |
|----------|-----------------|
| ViewModel | `ViewModels/MountingInventoryViewModel` (example name) |
| `RelayCommand` | Button actions without Click handlers |
| `INotifyPropertyChanged` | UI updates when `IsBusy` changes |

### Acceptance criteria

- [ ] No SQL in View code-behind
- [ ] No business logic in Click events on that screen
- [ ] Refresh and Save work via commands only
- [ ] Busy state disables buttons during load

### FA homework

- [ ] Open one FA ViewModel file — list 3 properties and 2 commands
- [ ] Compare same feature’s legacy View if exists

### Stretch

- Refactor P01 clock-in to MVVM as homework

---

## P05b — Settings screen (JSON) 🔲

| Field | Detail |
|-------|--------|
| **Time** | 1–2 days |
| **Prerequisites** | P05 |
| **FA stack** | User/plant/printer settings (FA uses XML/config) |

### Goal

Persist user preferences **without SQL** — common for client-side options.

### What to build

1. `Models/AppSettings.cs` — `PlantCode`, `DefaultPrinter`, `Theme` (Light/Dark)
2. `Services/SettingsService.cs`:
   - Path: `%AppData%/PracticeFA/settings.json`
   - `Load()` / `Save()` with `System.Text.Json`
3. `ViewModels/SettingsViewModel.cs` + `Views/SettingsView.xaml`
4. Menu item opens settings; Save writes JSON; Restart app loads values

### FA mapping

| Practice | Floor Assistant |
|----------|-----------------|
| JSON settings | Local user prefs; FA may use XML/INI |
| Plant selection | Login plant + company-specific menus |

### Acceptance criteria

- [ ] Change plant → close app → reopen → same plant shown
- [ ] Invalid JSON file handled (defaults, no crash)

---

## P38 — Value converters & multi-binding 🔲

| Field | Detail |
|-------|--------|
| **Time** | 2 days |
| **Prerequisites** | P05 |
| **FA stack** | Bool ↔ Visibility, formatting dates/weights in XAML |

### Goal

**IValueConverter** for display formatting without code in every label.

### What to build

1. `BoolToVisibilityConverter`, `DecimalWeightConverter` (show 3 decimal + “g”).
2. Attendance or employee screen: bind `IsActive`, `ClockedInAt`, `WeightGm`.
3. One **MultiBinding** example: `FullLabel = Badge + " - " + Name` (optional).

### Acceptance criteria

- [ ] Converters in `Resources` and reused on 2 screens
- [ ] Inactive row styled via DataTrigger or converter

### FA homework

- [ ] Search FA XAML for `Converter=` or `BooleanToVisibilityConverter`

---

## P39 — Dependency injection in WPF 🔲

| Field | Detail |
|-------|--------|
| **Time** | 3 days |
| **Prerequisites** | P05, P30 |
| **FA stack** | Testable VMs, service registration (modern FA direction) |

### Goal

Wire **ViewModels and services** with `Microsoft.Extensions.DependencyInjection`.

### What to build

1. `Host.CreateDefaultBuilder` in `App.xaml.cs` or manual `ServiceCollection`.
2. Register `IDataAccess`, `ISettingsService`, `AttendanceViewModel`.
3. Resolve VM when opening Attendance view — no `new AttendanceViewModel()` in View.

### Acceptance criteria

- [ ] Single composition root (`App.xaml.cs` or `Startup`)
- [ ] Swap `IDataAccess` mock for tests/design time

### FA homework

- [ ] Note which FA projects still use `new` vs DI (grep `new.*ViewModel`)

---

## P40 — Master-detail MVVM 🔲

| Field | Detail |
|-------|--------|
| **Time** | 3–4 days |
| **Prerequisites** | P05, P09, P36 helpful |
| **FA stack** | Order header list + line grid (Mounting, bagging, quotation) |

### Goal

Two related ViewModels or one VM with **selected header** driving **detail lines**.

### What to build

1. `OrderListViewModel` — headers collection, `SelectedOrder`.
2. When selection changes, load lines via `spGetOrderLines @OrderId`.
3. `OrderLineViewModel` — add/remove line commands; save header+lines via P36 proc.
4. View: master `ListView` + detail `DataGrid`.

### Acceptance criteria

- [ ] Changing selection reloads lines
- [ ] Add line updates total on header (computed property)
- [ ] Save uses service layer only

### FA homework

- [ ] Open Mounting or Bagging FA screen — identify master vs detail grid

**MVVM pillar checkpoint:** 5 projects — no Click handlers with SQL inside.

---

# Capstone — all pillars

## P10 — Mini Floor Assistant 🔲

| Field | Detail |
|-------|--------|
| **Time** | 1 week |
| **Prerequisites** | P02, P06, P07, P08; P05 recommended |
| **FA stack** | Full app lifecycle |

### Goal

One cohesive app that mirrors FA startup and navigation — your **portfolio demo**.

### What to build

| Step | Screen | Behavior |
|------|--------|----------|
| 1 | `SplashWindow` | 1–2 s delay + `SELECT 1` or `spPing` — show “DB OK” / red on fail |
| 2 | `SignInWindow` | P06 login |
| 3 | `MainWindow` | P02 shell + P07 menu + status bar: user, plant, SQL green/red |
| 4 | Hubs | Master + Reports pages (P03 styles) |
| 5 | Features | P04 opens at least one real CRUD window (Employees) |
| 6 | Exit | Clear session |

### App startup flow

```text
App.xaml → StartupUri or OnStartup → Splash → SignIn → MainWindow
```

Use `App.xaml.cs` `OnStartup` to control flow instead of fixed `StartupUri` if needed.

### FA mapping

| Practice | Floor Assistant |
|----------|-----------------|
| Full flow | `App.xaml` → Splash → `SignIn_New` → `MainWindowNew` |
| Status bar | SQL/version/user in FA footer |
| Module security | P07 on hub buttons |

### Acceptance criteria

- [ ] Cold start → login → hub → employee CRUD → exit works without restart
- [ ] Wrong SQL connection shows red status on splash or main
- [ ] Can demo in 5 minutes to a teammate
- [ ] Diagram drawn: App → Splash → SignIn → Main → Page → View

### FA homework

- [ ] Build **FloorAssistantQA** x86 Debug successfully
- [ ] Side-by-side: your flow diagram vs FA’s actual file names

---

# Domain modules (pick one)

## P11 — Bagging lite 🔲

| Field | Detail |
|-------|--------|
| **Time** | 1 week |
| **Prerequisites** | P10 |
| **FA stack** | Bagging entry, header/detail, labels |

### Goal

Practice **header + detail lines** save — diamond bagging is header/lines in FA.

### Database

- `BagHeader` (BagId, OrderNo, CreatedBy, CreatedAt, Status)
- `BagLine` (BagId, LineNo, Sku, Qty, WeightGm)
- `spGetBag`, `spSaveBag` (header + lines TVP or batch), `spListBags`

### UI

- Header: Order #, employee from session
- Grid: SKU, Qty, Weight — add/remove lines
- Save → proc; validation: at least one line, qty > 0
- Label preview: `TextBox` with string like `BAG-001|SKU1|12|1.25g` (BarTender later)

### FA mapping

Bagging entry Views, diamond bag labels, floor issue screens.

### Acceptance criteria

- [ ] Save reloads same bag with lines
- [ ] Cannot save empty order number
- [ ] Label text updates from header + lines

---

## P12 — Quotation lite 🔲

| Field | Detail |
|-------|--------|
| **Time** | 1 week |
| **Prerequisites** | P10 |
| **FA stack** | Sales quotation, commercial docs |

### Database

- `Customer`, `QuotationHeader`, `QuotationLine`
- `spGetCustomers`, `spGetQuotation`, `spSaveQuotation`

### UI

- Customer `ComboBox` from SQL
- Line grid: SKU, Qty, UnitPrice, LineTotal (computed in VM or SQL)
- Footer: grand total
- **Export CSV** button → `SaveFileDialog`

### Acceptance criteria

- [ ] Total recalculates when qty/price changes
- [ ] CSV opens correctly in Excel
- [ ] Customer required before save

---

## P13 — MIS report lite 🔲

| Field | Detail |
|-------|--------|
| **Time** | 3–4 days |
| **Prerequisites** | P09, P10 |
| **FA stack** | Floor MIS, read-only reports |

### Database

- Seed fact table or view `vwSalesSummary` (Date, Plant, Amount)
- `spReport_SalesSummary @FromDate, @ToDate, @PlantCode`

### UI

- Date pickers, plant filter, Run Report
- Read-only `DataGrid` ← `DataTable`
- Export CSV; optional “Email” = save temp CSV + `mailto:` or open folder

### Acceptance criteria

- [ ] Large date range handled without UI freeze (use async P20 if needed)
- [ ] Grid is read-only (no edit)
- [ ] Export matches grid data

---

## P46 — Work center routing screen 🔲

| Field | Detail |
|-------|--------|
| **Time** | 3 days |
| **Prerequisites** | P10, P36 |
| **FA stack** | `AppVariables.WCList`, routing sequence, Z-Out posting concept |

### Goal

Model **work center pipeline** (FKIT → WAX → CASTING → FSK → RFD) in UI.

### What to build

1. Table `WorkCenter` + `RoutingStep` (PO, StepOrder, WcCode, Status).
2. Screen: enter PO → show steps; buttons Mark Complete / Move to Next.
3. `spGetRouting`, `spCompleteStep` — validate order of steps.

### Acceptance criteria

- [ ] Cannot skip a step
- [ ] Completed PO shows RFD as last step done

### FA homework

- [ ] Read FA routing fix / super routing menu item name

---

## P47 — Stock scan simulation 🔲

| Field | Detail |
|-------|--------|
| **Time** | 2 days |
| **Prerequisites** | P10 |
| **FA stack** | POS stock scan, SPO vault scan, barcode text boxes |

### Goal

**Barcode scan** UX: focus always on scan box, beep on success (optional sound).

### What to build

1. TextBox accepts scan (keyboard wedge = Enter at end).
2. Lookup `spGetStockByBarcode` — show SKU, qty, location.
3. Log last 20 scans in on-screen list.

### Acceptance criteria

- [ ] Enter key triggers lookup without clicking button
- [ ] Unknown barcode shows error, focus returns to scan box

### FA homework

- [ ] Find one FA “scan” screen with TextBox `KeyDown` or timer

---

## P48 — Multi-plant switch 🔲

| Field | Detail |
|-------|--------|
| **Time** | 2 days |
| **Prerequisites** | P06, P07 |
| **FA stack** | Login plant 1001 / 1003, company-specific DB/menu |

### Goal

**Plant code** changes connection or filtered data — mirrors FA company selection.

### What to build

1. Users table includes allowed plants; login or combo selects plant.
2. All SPs take `@PlantCode` — seed data differs per plant.
3. Status bar shows plant; switching plant reloads menu (P07) and clears caches.

### Acceptance criteria

- [ ] Same user sees different modules or data per plant (seeded)
- [ ] Connection string or catalog switches per plant (document approach)

### FA homework

- [ ] Find plant/company field on FA `SignIn` or `UserInfo`

**Domain pillar:** build **at least 2** of P11–P13 plus **one** of P46–P48.

---

# Advanced UI (WPF) — 4+ projects

| ID | Project | Time | Summary |
|----|---------|------|---------|
| **P42** | TabControl wizard | 2d | Multi-step form (header → lines → confirm) like FA wizards |
| **P43** | Global exception handler | 1d | `DispatcherUnhandledException` → log + friendly MessageBox |
| **P44** | ListView master-detail (code-behind) | 2d | Same as P40 but **without** MVVM — compare patterns |
| **P20** | Async + busy overlay | 2d | `async` SP, `IsBusy`, disable buttons (see full spec below) |

### P42 — TabControl wizard 🔲

- Three tabs: Order info → Lines → Confirm; Next/Back validates current tab; Save on last tab calls P36 proc.

### P43 — Global exception handler 🔲

- Log all unhandled UI exceptions; show “Contact IT” message; no raw stack trace to operators.

### P44 — ListView master-detail (legacy style) 🔲

- Duplicate P40 behavior in code-behind + `DataTable` only — document pros/cons vs P40.

---

# Advanced C# / async — 3 projects

| ID | Project | Time | Summary |
|----|---------|------|---------|
| **P45** | Long-running SAP-style call | 2d | `Task.Run` + `IProgress<string>` + cancel button |
| **P46** | *(listed under Domain)* | | |
| **P21** | LiveCharts dashboard | 2d | Hourly output bar chart (FA MIS style) |

### P45 — Async SAP-style mock 🔲

- Mock `IErpService` delay 5s; UI stays responsive; Cancel uses `CancellationTokenSource`.

---

# FA integrations (build when job needs them)

**Minimum 3–4 to start** (e.g. P14, P15, P17, P19); **16 total** for full integration coverage.

### Core integrations (do first)

| ID | Project | FA tech |
|----|---------|---------|
| P14 | ERP service mock | SAP NCo / `SAPRFCHandler` |
| P15 | Printer / PDF | BarTender, raw print |
| P16 | Scale simulator | Mettler |
| P17 | ClosedXML export | Excel reports |

### Extended integrations (advanced)

| ID | Project | FA tech |
|----|---------|---------|
| P18 | PDF receipt | iText 8 |
| P19 | Chat assistant | OpenAI, `AiOrchestrationService` |
| P22 | WebView2 dashboard | Analytics embedded browser |
| P23 | Attached behaviors | `Attached_Properties/` |
| P25 | Webhook JSON | n8n design insights |
| P26 | SMTP email report | MIS email |
| P27 | ClickOnce study | `System.Deployment` |
| P28 | Tray + print dialog | WinForms interop |
| **P49** | Serilog / file logging | Production diagnostics |
| **P50** | QRCoder on label | QR on bag/jewellery tags |
| **P51** | Polly HTTP retry | Resilient AI/webhook calls |
| **P52** | FA screen trace document | Pick 1 FA View — document every ExecSP + param |

### P49 — Structured logging (Serilog) 🔲

- Replace P31 plain file log with Serilog rolling files; enrich with UserId, PlantCode.

### P50 — QR code on label 🔲

- Generate QR from bag id string; show in WPF `Image` or save PNG for P15 print.

### P51 — HTTP retry (Polly) 🔲

- Wrap P19/P25 HTTP calls with retry on 503; show attempt count in status bar.

### P52 — FA trace capstone (documentation) 🔲

- No code: one FA feature folder — list XAML, SP names, tables, ModuleId; compare to your P10/P11.

---

## Integration details (P14–P28)

## P14 — ERP service mock 🔲

| **Build** | `IErpService.GetStockAsync(sku)` — impl A: SQL view; impl B: JSON file |
| **FA** | `SAPRFCHandler`, SAP NCo x86 |
| **Rule** | ViewModel depends on `IErpService` only |

## P15 — Label / printer lite 🔲

| **Build** | Printer dropdown (`LocalPrintServer`), print text or PDF |
| **FA** | BarTender, `RawPrinterHelper` |

## P16 — Scale simulator 🔲

| **Build** | Button fills weight from random or serial port |
| **FA** | Mettler integration |

## P17 — Excel export 🔲

| **Build** | ClosedXML: grid → `.xlsx` |
| **FA** | ClosedXML usage in reports |

## P18 — PDF receipt 🔲

| **Build** | One-page PDF (QuestPDF or iTextSharp pattern) |
| **FA** | iText 8 |

## P19 — Chat assistant 🔲

| **Build** | `Services/ChatService.cs` + mock HTTP JSON; UI only binds to VM |
| **FA** | OpenAI SDK, `DAL/AIServices`, `AiOrchestrationService` |

## P20 — Async + busy overlay 🔲

| **Build** | `async` SP load, disable UI, busy text/Toolkit |
| **FA** | `BackgroundWorker` |

## P21 — LiveCharts 🔲

| **Build** | Bar chart from daily totals |
| **FA** | Analytics dashboards |

## P22 — WebView2 🔲

| **Build** | Embed local `dashboard.html` |
| **FA** | Analytics WebView2 |

## P23 — Attached behavior 🔲

| **Build** | Watermark on `TextBox` via `Microsoft.Xaml.Behaviors` |
| **FA** | `Attached_Properties/` |

## P25 — Webhook insights 🔲

| **Build** | POST to webhook.site → show JSON in UI |
| **FA** | Design insights / n8n |

## P26 — Email report 🔲

| **Build** | SMTP send CSV attachment (test account only) |
| **FA** | MIS email patterns |

## P27 — ClickOnce (study) 🔲

| **Build** | No code — read FA publish profile, update URL, version |
| **FA** | `System.Deployment` |

## P28 — Tray + print dialog 🔲

| **Build** | `NotifyIcon` + WinForms `PrintDialog` sample |
| **FA** | Tray, print dialogs |

---

# Do not clone (study in FA only)

| Technology | What to do instead |
|------------|-------------------|
| Crystal Reports / SSRS | Open report from FA menu; trace `ReportPath` in DB |
| SAP .NET Connector x86 | P14 mock first; build FA x86 when IT provides access |
| BarTender | P15 generic print; watch FA label code |
| VB `scLovPop` | Trace when LOV button opens popup |
| Office Interop | Use ClosedXML in P17 |

---

# After all projects: Floor Assistant readiness

You are ready to **contribute** (with mentor review) when:

### Build & run

- [ ] `FloorAssistant_V01.sln` / **FloorAssistantQA** builds **x86 Debug**
- [ ] You know why x86 matters (SAP NCo)

### Architecture

- [ ] Explain: `App` → Splash → SignIn → `MainWindow` → `Page` → `View`
- [ ] Explain Frame vs Window vs Page
- [ ] Explain `DataTable` grid vs ViewModel collection

### Data

- [ ] Trace one screen: Button → `ExecSP` → SP name → SSMS run → grid columns
- [ ] Map `DataRow` columns to C# fields on user object
- [ ] Never put connection strings in View XAML/code

### Security

- [ ] Find `clsDictionary.UserInfo` (or session equivalent)
- [ ] Find `CheckAuth` and one `ModuleId` you use

### Modern features

- [ ] Find `AiOrchestrationService` — AI calls go through DAL/services, not View

### Domain (at least one)

- [ ] Read one module’s `Pages/*` hub only (e.g. Bagging OR Master OR Planning)
- [ ] Understand PO, work center, style/SKU at glossary level

### Practice portfolio

- [ ] P10 runs end-to-end on a clean machine with SQL scripts documented
- [ ] One domain project (P11 or P12 or P13) demonstrates header/detail or report

---

# Progress tracker

Copy to `docs/my-progress.md` and tick dates.

| ID | Project | Started | Done | FA homework done |
|----|---------|---------|------|------------------|
| P00 | Console floor ticket | | | |
| P29 | LINQ lab | | | |
| P30 | Interfaces + repository | | | |
| P31 | Exceptions + JSON log | | | |
| P01 | Clock-in board | | ✅ | |
| P02 | Frame + hubs | | | |
| P32 | TwoWay binding | ✅ | `projects/P32-TwoWayBindingLab/` | |
| P03 | Theme dictionary | ✅ | `src/PracticeFA.App/Assets/Theme.xaml` | |
| P04 | Module launcher | ✅ | `src/PracticeFA.App/Views/*` | |
| P33 | Validation | ✅ | `projects/P33-ValidationLab/` | |
| P34 | UserControl + menu | ✅ | `src/PracticeFA.App/Controls/` | |
| P06 | SQL login | ✅ | `database/scripts/001_PracticeFA.sql` · `SignInWindow` | |
| P24 | Connection config | | | |
| P07 | Role menu | | | |
| P08 | Employee CRUD | | | |
| P09 | DataGrid binding | | | |
| P35 | Search SP | | | |
| P36 | Transaction save | | | |
| P41 | Audit / soft delete | | | |
| P05 | Attendance MVVM | | | |
| P38 | Converters | | | |
| P05b | Settings JSON | | | |
| P39 | DI in WPF | | | |
| P40 | Master-detail MVVM | | | |
| P10 | Mini FA | | | |
| P11 | Bagging lite | | | |
| P12 | Quotation lite | | | |
| P13 | MIS lite | | | |
| P46 | Work center routing | | | |
| P47 | Stock scan | | | |
| P48 | Multi-plant | | | |
| P42 | Tab wizard | | | |
| P43 | Global exceptions | | | |
| P44 | ListView legacy MD | | | |
| P20 | Async busy | | | |
| P45 | Async SAP mock | | | |
| P21 | LiveCharts | | | |
| P14–P28 | Integrations (group) | | | |
| P49 | Serilog | | | |
| P50 | QRCoder | | | |
| P51 | Polly retry | | | |
| P52 | FA trace doc | | | |
| S00 | ERP vs FA vs SAP | | | |
| S01 | QA SAP logon | | | |
| S02 | T-code & favorites | | | |
| S03 | MM basics | | | |
| S04 | SFLR / LOOSE1 | | | |
| S05 | YPACK ↔ FA Bagging | | | |
| S06 | PP order concept | | | |
| S07 | Cheat sheet template | | | |
| S08 | FA export rows | | | |
| S09 | SAP stock verify | | | |
| S10 | FA→SAP flow doc | | | |
| S11 | RFC concepts | | | |
| S12 | SAPSettings | | | |
| S13 | Return tables | | | |
| S14 | Ping vs RFC | | | |
| S15 | FM authorization | | | |
| S16 | Debug PULLFromSAP | | | |
| S17 | IT_ERR handoff | | | |
| S18 | RPA intro | | | |
| S19 | SE37 FM display | | | |
| S20 | SE16 / ST22 | | | |
| S21 | ABAP path study | | | |

**FA SPs traced:**

| Screen | SP name | SSMS tested date |
|--------|---------|------------------|
| SignIn | | |
| (add rows) | | |

---

# Target repo layout (end state)

```
PracticeFA/
  PracticeFA.slnx
  src/
    PracticeFA.Console/     # P00
    PracticeFA.App/         # P01–P13, P20–P23
      App.xaml
      MainWindow.xaml
      Pages/
      Views/
      ViewModels/
      Models/
      Services/             # DataAccess, Settings, Chat, Erp
      Assets/
      Attached/
  database/scripts/
    001_PracticeFA.sql      # P06
    002_Employees.sql       # P08
    003_Bagging.sql         # P11
  docs/
    projects.md             # this file
    LEARNING_PATHWAY.md
    my-progress.md          # you create
```

---

*Last updated: practice catalog for Floor Assistant QA learning path.*
