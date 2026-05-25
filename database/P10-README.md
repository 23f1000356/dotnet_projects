# P10 — Mini Floor Assistant (capstone)

**Prerequisites:** P02, P06, P07, P08 (P05 recommended)  
**App:** `src/PracticeFA.App/`

## Startup flow (FA lifecycle)

```text
App.OnStartup
  → DI host + P43 exception handler + theme
  → SplashWindow (~1.5s + DB ping)
  → SignInWindow (P06 spLogin)
  → MainWindow (P02 shell + P07 modules + status bar)
  → Master / Reports hubs → P04 feature windows (Employees, Bagging, …)
  → Exit clears AppState
```

## What P10 adds

| Step | Screen | Behavior |
|------|--------|----------|
| 1 | `SplashWindow` | Delay + `TryTestConnection` + optional `dbo.spPing` |
| 2 | `SignInWindow` | SQL login (demo fallback if DB down) |
| 3 | `MainWindow` | Menu, sidebar, **footer SQL green/red**, user/plant |
| 4 | Hubs | Master + Reports |
| 5 | Features | Module buttons → `ShowDialog` CRUD (e.g. Employees 6001) |
| 6 | Exit | `AppState.Clear()` |

## SQL scripts (clean machine)

Run on `(localdb)\MSSQLLocalDB`, database **PracticeFA**:

| Order | Script |
|-------|--------|
| 1 | `001_PracticeFA.sql` |
| 2 | `002_P08_Employees.sql` |
| 3 | `004_P04_Styles.sql` (optional modules) |
| 4 | `005_P35_SearchEmployees.sql` |
| 5 | `011_P10_Ping.sql` (splash `spPing`) |

Sign in: `operator1` / `pass1` or `manager1` / `pass1` (more modules).

## 5-minute demo script

1. Start app — splash shows **SQL: OK** (green) or **Failed** (red)
2. Login as `manager1` / `pass1`
3. **Master** → **Employee maintenance (6001)** → search → edit → save
4. **View → Orders (P40)** or **Hourly chart (P21)**
5. Footer: user + SQL status; **Exit**

## Acceptance checks

- [ ] Cold start → splash → login → Master → employee window → exit
- [ ] Wrong connection string → splash red; demo login may still work
- [ ] Footer SQL indicator matches splash result
- [ ] Draw diagram: App → Splash → SignIn → Main → Page → View

## FA mapping

| Practice FA | Floor Assistant QA |
|-------------|-------------------|
| `SplashWindow` | FA splash / load |
| `SignIn_New` | Sign-in |
| `MainWindowNew` + Frame | Main shell |
| Status footer | SQL / user / version |
