# P01 — Employee clock-in board (WPF)

**Prerequisites:** [P00](../P00-ConsoleFloorTicket/) · C# track P29–P31 optional but helpful

## What this is

Your **first WPF window** — like a small React page with:

- XAML layout (`.xaml` = markup)
- Code-behind (`.xaml.cs` = event handlers)
- In-memory state (`List<>`, no database)

| Area | FA later |
|------|----------|
| Sign in box | `SignIn_New` |
| Session text | `clsDictionary.UserInfo` |
| Floor list | Simple list before `DataGrid` + SQL |

## Run

```powershell
dotnet run --project projects/P01-ClockInBoard/P01.App.csproj
```

Visual Studio: set **P01.App** as startup → **F5**.

## Try it

1. Sign in: `E001` or `E002` (or any badge for guest).
2. Clock in: `E101`–`E104` (or unknown badge).
3. Try duplicate clock-in → message.
4. Clear list → confirm dialog.

## Files

| File | Role |
|------|------|
| `App.xaml` | Application entry |
| `MainWindow.xaml` | UI layout |
| `MainWindow.xaml.cs` | Click handlers, list state |
| `Models/UserSession.cs` | Logged-in user |
| `Models/FloorEmployee.cs` | One person on floor |

## Next

**P02** — Frame + Master/Reports pages (will extend `src/PracticeFA.App` or this project).
