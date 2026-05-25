# P09 — DataGrid ← DataTable lab

**Stack:** WPF · `DataTable` · `DataView` · `ObservableCollection` · `DataGrid`  
**Prerequisites:** [P08](../../database/P08-README.md) (employee SPs)  
**Next:** P10 capstone · compare with [PracticeFA.App](../../src/PracticeFA.App/) `EmployeeListWindow`

---

## What is P09?

Two tabs in one window — **same employee data**, two FA binding styles:

| Tab | Style | ItemsSource |
|-----|--------|-------------|
| **A — Legacy FA** | `DataTable.DefaultView` + `AutoGenerateColumns` | SQL column changes show in grid without new C# properties |
| **B — Modern** | `ObservableCollection<EmployeeRowViewModel>` + explicit columns | Sort, filter, MVVM-friendly |

---

## Run

```powershell
dotnet run --project projects/P09-DataGridLab/P09.App.csproj
```

Optional SQL (same data as P08 + **Email** column):

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\001_PracticeFA.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\002_P08_Employees.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\003_P09_EmployeeEmail.sql
```

Without SQL, the lab uses an in-memory `DataTable` that already includes **Email**.

---

## Folder structure

```text
projects/P09-DataGridLab/
  MainWindow.xaml(.cs)     → Tab A + Tab B
  Services/
    EmployeeDataLoader.cs  → SQL or sample fallback
    SampleEmployeeData.cs
    EmployeeRowMapper.cs   → DataRow → ViewModel (Version B)
  Models/EmployeeRowViewModel.cs
database/scripts/003_P09_EmployeeEmail.sql
```

---

## Acceptance checklist

- [ ] Tab A: **Email** column visible (sample or after script 003)
- [ ] Tab B: **Sort by name** reorders grid
- [ ] Tab B: **Filter** narrows rows by display name
- [ ] Explain when FA uses A (`DefaultView`) vs B (`ObservableCollection`)

---

## FA mapping

| P09 | Floor Assistant |
|-----|-----------------|
| `dt.DefaultView` | Older `Views/*` grids |
| `ObservableCollection` | Newer inventory / VM screens |
| `AutoGenerateColumns` | Metadata-driven columns |

**PracticeFA.App:** `EmployeeListWindow` already uses **Version A** (`table.DefaultView`).
