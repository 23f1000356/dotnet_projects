# Debug P30 — step by step

## Setup

- Cursor: **P30 — Interfaces + repository** in Run and Debug → **F5**
- Visual Studio: set **P30.Console** startup → **F5**

## Keys

| Key | Action |
|-----|--------|
| F5 | Start / Continue |
| F10 | Step Over |
| F11 | Step Into |
| Shift+F5 | Stop |

## Breakpoint map

| Step | File | Line | Watch |
|------|------|------|-------|
| A | Program.cs | 7-10 | `repository.GetType().Name` |
| B | Program.cs | 34-35 | menu choice `1` → `ListEmployees` |
| C | InMemoryEmployeeRepository.cs | 15-20 | `_store`, `activeOnly` |
| D | EmployeeService.cs | 40 | duplicate badge check |
| E | InMemoryEmployeeRepository.cs | 26-31 | `_store.Add` |
| F | Program.cs | 126-136 | SQL stub exception |
