# P06 — SQL login + UserInfo (detailed)

**Stack:** SQL Server · SSMS · Stored procedures · ADO.NET · `Microsoft.Data.SqlClient`  
**App:** `src/PracticeFA.App/` — starts at `SignInWindow`, then main shell  
**Prerequisites:** P02 shell · SQL Server Express or **LocalDB** (Visual Studio install)

---

## What is P06? (simple)

**P06 replaces fake login** with a real database call — the same pattern Floor Assistant uses for sign-in:

1. User enters **User ID** and **password** on `SignInWindow`
2. App calls **`dbo.spLogin`** through **`DataAccess.ExecSp`** (no SQL text in the UI file)
3. Result rows map to **`UserInfo`** (`UserId`, `DisplayName`, `PlantCode`)
4. Session stored in **`AppState.CurrentUser`** until Exit
5. **MainWindow** opens only after successful login

---

## The problem P06 solves

| Without P06 | With P06 |
|-------------|----------|
| Anyone opens the app | Only valid DB users sign in |
| Hard-coded user in code | User from `Users` table |
| No DAL boundary | Views → `LoginService` → `DataAccess` → SP |

**FA rule:** XAML code-behind on sign-in does **not** contain `SELECT` / `INSERT` strings — only service/DAL calls.

---

## Folder layout

```text
database/
  scripts/001_PracticeFA.sql   → DB, tables, seed, spLogin, spGetUserModules
  README.md                    → this file

src/PracticeFA.App/
  App.config                   → connection string PracticeFA
  Services/
    DataAccess.cs              → ExecSp(procName, SqlParameter[])
    LoginService.cs            → TryLogin → UserInfo
    AppState.cs                → CurrentUser session
  Models/
    UserInfo.cs                → matches SP columns
    UserInfoMapper.cs          → DataRow → UserInfo (explicit column names)
  Views/
    SignInWindow.xaml(.cs)     → Login UI only
  App.xaml.cs                  → SignIn → MainWindow startup
```

---

## Step 1 — Create the database (SSMS or sqlcmd)

### Option A — SQL Server Management Studio (recommended)

1. Open **SSMS**
2. Connect to `(localdb)\MSSQLLocalDB` or your SQL Express instance
3. **File → Open** → `database/scripts/001_PracticeFA.sql`
4. **Execute** (F5)
5. Verify:

```sql
USE PracticeFA;
EXEC dbo.spLogin @UserId = N'operator1', @Password = N'pass1';
EXEC dbo.spGetUserModules @UserId = N'operator1';
```

### Option B — Command line

```powershell
cd "c:\Users\Vishakha.Roy\Desktop\Sample_dotnet"
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i "database\scripts\001_PracticeFA.sql"
```

If `sqlcmd` is not found, install SQL Server tools or use SSMS only.

---

## Database schema

### Tables

| Table | Purpose |
|-------|---------|
| `Users` | Login accounts (practice passwords in `PasswordHash` — plain text) |
| `Modules` | Module ids (1001 Style, 2001 Bagging, …) |
| `UserAccess` | Which user can open which module (used in **P07**) |

### Seed users (practice only)

| User ID | Password | Display name | Plant |
|---------|----------|--------------|-------|
| `operator1` | `pass1` | Floor Operator One | P01 |
| `manager1` | `pass1` | Plant Manager | P01 |
| `operator2` | `pass2` | Floor Operator Two | P02 |

**Security note:** Plain passwords are for learning only. Real FA uses proper auth — never commit production secrets.

---

## Stored procedures

### `dbo.spLogin`

| Parameter | Type |
|-----------|------|
| `@UserId` | NVARCHAR(20) |
| `@Password` | NVARCHAR(100) |

**Returns (one row if valid):**

| Column | Maps to `UserInfo` |
|--------|-------------------|
| `UserId` | `UserId` |
| `DisplayName` | `DisplayName` |
| `PlantCode` | `PlantCode` |

Empty result = invalid credentials (friendly message in app, not exception).

### `dbo.spGetUserModules` (for P07)

Returns `ModuleId`, `ModuleName` for menu filtering — already in script; wired in `LoginService.GetUserModules`.

---

## Step 2 — Connection string (`App.config`)

```xml
<connectionStrings>
  <add name="PracticeFA"
       connectionString="Server=(localdb)\MSSQLLocalDB;Database=PracticeFA;Trusted_Connection=True;TrustServerCertificate=True;"
       providerName="Microsoft.Data.SqlClient" />
</connectionStrings>
```

Change `Server=` if you use named SQL Express instance, e.g. `Server=.\SQLEXPRESS`.

Copied to output folder on build (`CopyToOutputDirectory`).

---

## Step 3 — `DataAccess.ExecSp`

```csharp
public static DataTable ExecSp(string procName, params SqlParameter[] parameters)
```

| Piece | FA equivalent |
|-------|----------------|
| `SqlConnection` | Open from config |
| `CommandType.StoredProcedure` | Always SP, not ad-hoc SQL |
| `SqlDataAdapter.Fill` | Returns `DataTable` like FA grids |

**No** `SELECT * FROM Users` in C# — only procedure names.

---

## Step 4 — Map `DataRow` → `UserInfo`

```csharp
UserId = Convert.ToString(row["UserId"]) ?? "",
```

Explicit **column names** must match SSMS result — same discipline as FA `m_UserInfo` mapping in `SignIn_New.xaml.cs`.

---

## Step 5 — Sign-in UI flow

```text
App.OnStartup
  → SignInWindow.ShowDialog()
       Login_Click → LoginService.TryLogin (not SQL in .xaml.cs)
       success → AppState.CurrentUser = user; DialogResult=true
       fail → ErrorText (friendly); no stack trace to user
  → if cancelled → Shutdown
  → MainWindow.Show() with session in toolbar title
  → Exit → AppState.Clear → Shutdown
```

### Friendly errors

| Case | Message |
|------|---------|
| Empty fields | Enter User ID and password |
| Bad password | Invalid User ID or password |
| DB down | Run 001_PracticeFA.sql + check connection string |
| Other | Sign-in failed. Contact IT support. |

---

## Run the app

```powershell
dotnet run --project src/PracticeFA.App/PracticeFA.App.csproj
```

1. Sign-in window appears  
2. Login `operator1` / `pass1`  
3. Main shell shows **Signed in: operator1 · … · Plant P01**  
4. File → Exit clears session  

Wrong password → red error text, stay on sign-in.

---

## Web → FA mapping

| Practice P06 | Floor Assistant |
|--------------|-----------------|
| `dbo.spLogin` | Sign-in SPs in `SignIn_New` |
| `DataAccess.ExecSp` | `clsAccess.ExecSP` / `clsFA_DBC` |
| `UserInfo` | `m_UserInfo` fields |
| `AppState.CurrentUser` | Session / `clsDictionary.UserInfo` |
| `App.config` | `dbConnect` / Profile.dll connection |

### FA homework (critical)

1. Run FA sign-in SP in SSMS with QA test user  
2. Compare column names to `m_UserInfo` in `SignIn_New.xaml.cs`  
3. Document SP name + 5 fields in your progress notes  

---

## Acceptance checklist

- [ ] `EXEC dbo.spLogin` in SSMS returns same columns as C# expects  
- [ ] Bad password → friendly message (no stack trace on screen)  
- [ ] `UserInfo` in toolbar until Exit  
- [ ] Zero SQL strings in `SignInWindow.xaml.cs` — only `LoginService`  

---

## Troubleshooting

| Symptom | Fix |
|---------|-----|
| Cannot connect to database | Run `001_PracticeFA.sql`; confirm LocalDB installed (`sqllocaldb info`) |
| Login always fails | Re-run seed; test SP in SSMS |
| App.config not found | Rebuild project; check `bin/Debug/net10.0-windows/App.config` |
| Wrong server | Edit connection string for your instance |

```powershell
sqllocaldb start MSSQLLocalDB
sqllocaldb info MSSQLLocalDB
```

---

## P06 vs P07

| P06 | P07 |
|-----|-----|
| Login + `UserInfo` | `spGetUserModules` → hide hub buttons |
| Session user | `HashSet<int>` allowed modules |

---

## Learning path

```text
P02 (shell) → P06 (SQL login) → P07 (role menu) → P08 (CRUD SPs) → P10 (capstone)
```

---

## Experiments

1. Add a user in SSMS — login without recompiling C#  
2. Change `spLogin` to return extra column — watch mapper break (fix explicitly)  
3. Breakpoint in `DataAccess.ExecSp` — inspect `DataTable` rows  
