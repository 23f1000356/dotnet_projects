# P41 — Audit columns & soft delete

**Prerequisites:** P08 (`002`) · optional P35 (`005`)  
**App:** `EmployeeService` passes `AppState.CurrentUser.UserId` on insert/update/delete

## Run script

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\007_P41_EmployeeAudit.sql
```

Adds to `dbo.Employees`: `CreatedBy`, `CreatedAt`, `ModifiedBy`, `ModifiedAt`  
Updates all employee procs to read/write audit fields.

## Prove P41

### Soft delete
1. Employee maintenance → select **E103** → **Delete** → confirm
2. Grid (Active only) → E103 gone
3. SSMS: `SELECT * FROM dbo.Employees WHERE BadgeId = N'E103'` → `IsActive = 0`, `ModifiedBy = manager1`

### CreatedBy on insert
1. **Add** employee `E199` / name → Save
2. SSMS: `CreatedBy` = `manager1` for that row

### Include inactive (P35)
Uncheck **Active only** → Search → E103 and E104 visible again
