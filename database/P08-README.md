# P08 — Employee CRUD via stored procedures (detailed)

**App:** `src/PracticeFA.App/Views/EmployeeListWindow`  
**Prerequisites:** [P06](README.md) · [P07](P07-README.md) · run `002_P08_Employees.sql`

---

## What is P08? (simple)

**P08 is the core Floor Assistant data loop:** list data from SQL → edit in a dialog → save with a **stored procedure** → refresh the grid.

No `SELECT` / `INSERT` strings in View code-behind — only `EmployeeService` → `DataAccess.ExecSp`.

---

## Database

### Script

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\002_P08_Employees.sql
```

### Table `dbo.Employees`

| Column | Type |
|--------|------|
| EmployeeId | INT IDENTITY PK |
| BadgeId | NVARCHAR(20) UNIQUE |
| DisplayName | NVARCHAR(100) |
| ProcessCenter | NVARCHAR(50) |
| IsActive | BIT (soft delete) |

### Stored procedures

| Procedure | Purpose |
|-----------|---------|
| `spGetEmployees` | List (`@ActiveOnly`) |
| `spGetEmployeeByBadge` | Single row for search |
| `spInsEmployee` | Insert |
| `spUpdEmployee` | Update |
| `spDelEmployee` | Soft delete (`IsActive = 0`) |

### Module access (P07)

Module **6001** Employee Maintenance — granted to **operator1** and **manager1** in `002` script.

---

## C# layers

```text
EmployeeListWindow.xaml.cs
    → EmployeeService (validation + friendly errors)
        → DataAccess.ExecSp("dbo.spGetEmployees", ...)
            → SQL Server
```

| File | Role |
|------|------|
| `Models/EmployeeRecord.cs` | Row shape |
| `Models/EmployeeMapper.cs` | `DataRow` → `EmployeeRecord` (explicit columns) |
| `Services/EmployeeService.cs` | All CRUD calls |
| `Views/EmployeeListWindow` | Grid + Add/Edit/Delete/Refresh |
| `Views/EmployeeEditWindow` | Add/Edit dialog |

---

## UI flow

1. Login → **Master** → **Employee maintenance (6001)**
2. Grid loads via `spGetEmployees` → `ItemsSource = DataTable.DefaultView`
3. **Add** → `EmployeeEditWindow` → `spInsEmployee` → **Refresh**
4. **Edit** → selected row → `spUpdEmployee` → **Refresh**
5. **Delete** → confirm → `spDelEmployee` → row disappears from active list
6. **Search** on Master uses `spGetEmployeeByBadge`

---

## Validation (before SP)

- Badge required, starts with `E`, min 4 chars  
- Display name required  
- Duplicate badge → friendly message (SQL 2627)

---

## SSMS / Profiler acceptance

You should only see:

```sql
EXEC dbo.spGetEmployees @ActiveOnly=1
EXEC dbo.spInsEmployee ...
EXEC dbo.spUpdEmployee ...
EXEC dbo.spDelEmployee ...
```

---

## Run

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\001_PracticeFA.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\002_P08_Employees.sql
dotnet run --project src/PracticeFA.App/PracticeFA.App.csproj
```

Login **operator1** / **pass1** → Master → **Employee maintenance (6001)**.

---

## Acceptance checklist

- [ ] Grid shows seed employees E101–E103 (not E104 inactive)
- [ ] Add new badge saves via SP and appears after Refresh
- [ ] Edit changes match SSMS row
- [ ] Delete removes from active grid (soft delete)
- [ ] No SQL strings in `EmployeeListWindow.xaml.cs`

---

## FA mapping

| P08 | Floor Assistant |
|-----|-----------------|
| `ExecSp` | `clsAccess.ExecSP` |
| Grid refresh after save | Standard maintenance screens |
| SP parameters | Must match exactly |

---

## Next

**P09** — compare `DataTable.DefaultView` vs `ObservableCollection` binding.
