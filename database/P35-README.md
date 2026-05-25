# P35 — Search & filter stored procedure

**Prerequisites:** P08 (`002_P08_Employees.sql`)  
**App:** `EmployeeListWindow` + `EmployeeService.Search`

## Run script

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\005_P35_SearchEmployees.sql
```

## SSMS tests (match WPF row counts)

```sql
USE PracticeFA;
EXEC dbo.spSearchEmployees;
EXEC dbo.spSearchEmployees @BadgeFragment = N'E10';
EXEC dbo.spSearchEmployees @ProcessCenter = N'CAST';
EXEC dbo.spSearchEmployees @ActiveOnly = 0;
```

## WPF

1. Sign in as **manager1** / **pass1** (module 6001).
2. **Master → Employee maintenance (6001)**.
3. Use **Badge contains**, **Process center contains**, **Active only**, then **Search**.
4. Empty filters → broader set; filled filters → narrower; zero rows → **No records** message.
