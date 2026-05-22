# P07 — Role-based menu (detailed)

**App:** `src/PracticeFA.App/` · **Requires:** [P06](README.md) login + `001_PracticeFA.sql`  
**Stored procedure:** `dbo.spGetUserModules`

---

## What P07 does

After login, the app loads **which modules this user may open** from SQL — not `if (user == "admin")`.

| Step | Code |
|------|------|
| Login success | `LoginService.LoadUserModulesIntoSession(userId)` |
| Cache | `AppState.AllowedModuleIds` (`HashSet<int>`) |
| Master hub | Hide buttons whose `Tag` / module id is not in the set |
| Open feature | `ModuleAuth.CanAccess(moduleId)` before `ShowDialog` |

---

## Seed data — who sees what

| User | Password | Modules (ids) | Hidden on Master |
|------|----------|-----------------|------------------|
| **operator1** | pass1 | 1001, 2001, 4001 | **3001 MIS** hidden |
| **manager1** | pass1 | 1001, 2001, 3001, 4001, 5001 | (all three launcher buttons visible) |
| **operator2** | pass2 | 2001, 4001 only | **1001 Style**, **3001 MIS** hidden · **2001 Bagging visible** |

### Acceptance demo

1. Login **operator1** → see Style + Bagging, **no** MIS Productivity button  
2. Logout, login **operator2** → see **Bagging only** (no Style)  
3. Login **manager1** → see all three launcher buttons  

**operator2** does not see Style; **operator1** sees Bagging — matches “different users, different menus” without username `if` checks.

---

## UI on Master page

- **Your modules (from database)** — read-only text from `AppState.ModuleListDisplay`  
  Example: `1001 Style Creation · 2001 Bagging Entry · 4001 Reports Hub`
- Launcher buttons use `Visibility=Collapsed` when not allowed

---

## `ModuleAuth` (FA: CheckAuth)

```csharp
public static bool CanAccess(int moduleId) =>
    AppState.AllowedModuleIds.Contains(moduleId);
```

Call before opening a feature window — same idea as `CheckAuth(1001)` in FA.

---

## SSMS test

```sql
USE PracticeFA;
EXEC dbo.spGetUserModules @UserId = N'operator2';
```

Compare rows to visible buttons in the app.

---

## Run

```powershell
dotnet run --project src/PracticeFA.App/PracticeFA.App.csproj
```

Try three users above on the **Master** page.

---

## Acceptance checklist

- [ ] Module list loaded once at login (cached in `AppState`)  
- [ ] No `if (userId == "...")` in `MasterPage.xaml.cs`  
- [ ] operator2 sees Bagging, not Style  
- [ ] operator1 does not see MIS (3001)  

---

## FA homework

- [ ] Grep FA for `CheckAuth` — note module id passed  
- [ ] Map FA module id for one screen you care about  

---

## Next

**P08** — Employee CRUD via `ExecSp` on feature screens.
