# P24 — Connection config (detailed)

**App:** `src/PracticeFA.App/`  
**Prerequisites:** [P06](README.md) (`App.config` + `DataAccess`)  
**FA analog:** Profile.dll INI — server/database per environment (IT_QA, IT_PRD)

---

## What is P24? (simple)

**P24 moves the database address to one place** so you can point the app at **LocalDB** or **QA** without rebuilding.

- All SQL goes through `DataAccess` → `DbSettings.ConnectionString`
- Sign-in shows **which database** is active
- **Test connection** button before login

---

## Files

| File | Role |
|------|------|
| `App.config` | Named connection strings `PracticeFA` + `PracticeFA_QA` |
| `Services/DbSettings.cs` | Picks active string, parses server/database for UI |
| `Services/DataAccess.cs` | Uses `DbSettings.ConnectionString` (not hard-coded name) |
| `Views/SignInWindow` | Shows target + Test connection |
| `MainWindow` | Help → Database connection; status bar shows DB |

---

## Connection strings (`App.config`)

```xml
<add name="PracticeFA"     ... Server=(localdb)\MSSQLLocalDB;Database=PracticeFA; ...
<add name="PracticeFA_QA"  ... Server=.;Database=PracticeFA; ...
```

Edit `PracticeFA_QA` to your real QA server when IT gives you one.

---

## Switch environment (no recompile)

| Method | Effect |
|--------|--------|
| Default | Uses `PracticeFA` (Local dev) |
| `PRACTICE_FA_ENV=QA` | Uses `PracticeFA_QA` |
| `PRACTICE_FA_CONNECTION=PracticeFA_QA` | Explicit connection name |

### PowerShell (current session)

```powershell
$env:PRACTICE_FA_ENV = "QA"
dotnet run --project src/PracticeFA.App/PracticeFA.App.csproj
```

### Visual Studio

Project Properties → Debug → Environment variables → add `PRACTICE_FA_ENV` = `QA`

---

## Acceptance checklist

- [ ] `DataAccess` uses `DbSettings` only
- [ ] Sign-in shows `DB [Local]` or `DB [QA]` line
- [ ] Test connection succeeds after running `001` + `002` scripts
- [ ] Help → Database connection shows same info

---

## FA homework

Find where real FA reads SQL server (Profile / INI) — same purpose as P24, do not copy production secrets.

---

## Learning path

```text
P06 (login + first connection string) → P24 (multi-environment) → P07/P08/P09 (all use same DataAccess)
```
