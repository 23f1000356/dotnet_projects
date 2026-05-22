# P30 — Interfaces + repository (in-memory)

**Prerequisites:** [P00](../P00-ConsoleFloorTicket/) · [P29](../P29-LinqCollectionsLab/)

## What this teaches

| Piece | Role | FA later |
|-------|------|----------|
| `IEmployeeRepository` | **Contract** — what data layer must do | `IDataAccess`, `IErpService` |
| `InMemoryEmployeeRepository` | **Implementation** with `List<>` | Test / dev without SQL |
| `SqlEmployeeRepository` | **Stub** for P08 | `ExecSp` + `DataTable` |
| `EmployeeService` | **Business logic** — uses interface only | ViewModel / screen logic |

**Key idea:** `EmployeeService` never knows if data is in RAM or SQL. Swap the constructor argument.

## Run

```powershell
dotnet run --project projects/P30-InterfacesRepository/P30.Console.csproj
```

Menu: list, find, add, update. Option **6** demos `SqlEmployeeRepository` throwing `NotImplementedException`.

## To use SQL stub as default

In `Program.cs`, swap the two lines:

```csharp
// IEmployeeRepository repository = new InMemoryEmployeeRepository();
IEmployeeRepository repository = new SqlEmployeeRepository();
```

Service methods unchanged — that is the acceptance test for this project.

## Next

**P31** — exceptions + file logging · **P08** — real `SqlEmployeeRepository`
