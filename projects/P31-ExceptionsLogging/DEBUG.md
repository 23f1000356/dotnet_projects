# Debug P31 — step by step

## Setup

- Cursor: **P31 — Exceptions + JSON** → **F5**
- VS: **P31.Console** startup → **F5**

Log/export paths (after run):  
`bin/Debug/net10.0/logs/practice-YYYY-MM-DD.txt`  
`bin/Debug/net10.0/export/employees.json`

## Keys

F5 Continue · F10 Step Over · F11 Step Into · Shift+F5 Stop

## Breakpoint map

| # | File | Line | Goal |
|---|------|------|------|
| A | InMemoryEmployeeRepository | ValidateBadge | Badge BAD |
| B | EmployeeService | catch ValidationException | userMessage only |
| C | JsonEmployeeStore | Serialize / Deserialize | See json string |
| D | Program | ForceError | Log + rethrow |
| E | Program | catch in RunMenu | Inner handler |
