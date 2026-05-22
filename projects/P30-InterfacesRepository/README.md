# P30 — Interfaces + repository (in-memory)

**Stack:** C# · Interfaces · Repository pattern · Console menu  
**Prerequisites:** [P00](../P00-ConsoleFloorTicket/) · [P29](../P29-LinqCollectionsLab/)  
**Next:** [P31](../P31-ExceptionsLogging/) · **P08** (real SQL repo)

---

## What is P30? (simple)

**P30 teaches layers** — the pattern FA uses so screens do not talk to SQL directly.

```text
Program (menu UI)
    ↓
EmployeeService (rules: validate, TryAdd, TryUpdate)
    ↓
IEmployeeRepository (contract)
    ↓
InMemoryEmployeeRepository   OR   SqlEmployeeRepository (stub)
```

**Key rule:** `EmployeeService` never sees `List<>` or `SqlConnection` — only the **interface**.

---

## Web analogy

| P30 | Node/React |
|-----|------------|
| `IEmployeeRepository` | `interface EmployeeRepo` |
| `InMemoryEmployeeRepository` | Mock / in-memory impl |
| `SqlEmployeeRepository` | Real DB impl (P08) |
| `EmployeeService` | Service layer / API module |

---

## Folder structure

```text
projects/P30-InterfacesRepository/
  Models/Employee.cs
  Repositories/
    IEmployeeRepository.cs           → contract
    InMemoryEmployeeRepository.cs    → List<> store
    SqlEmployeeRepository.cs         → NotImplementedException (P08)
  Services/EmployeeService.cs        → business logic
  Program.cs                         → menu + wire-up
```

---

## Composition root (`Program.cs`)

```csharp
IEmployeeRepository repository = new InMemoryEmployeeRepository();
var service = new EmployeeService(repository);
```

Swap one line to SQL stub — **service code unchanged**:

```csharp
IEmployeeRepository repository = new SqlEmployeeRepository();
```

That is the main acceptance test for this project.

---

## `IEmployeeRepository`

```csharp
GetAll(bool activeOnly = true)
GetByBadge(string badgeId)
Add(Employee employee)
Update(Employee employee)
```

**Interface** = promise only; no implementation body.

---

## `InMemoryEmployeeRepository`

- Seed data: E101–E104 (Sara inactive)
- `GetAll` uses LINQ `.Where(e => e.IsActive)` when `activeOnly`
- `Add` throws if duplicate badge
- `Update` replaces item in list by index

Only this class knows about `_store` (`List<Employee>`).

---

## `SqlEmployeeRepository`

Every method throws `NotImplementedException` with hint to P08 SP names:

- `spGetEmployees`, `spGetEmployeeByBadge`, `spInsEmployee`, `spUpdEmployee`

Menu option **6** demos this — proves service layer does not care which impl is wired.

---

## `EmployeeService`

| Method | Behavior |
|--------|----------|
| `ListEmployees` | Delegates to `GetAll` |
| `Find` | `GetByBadge` |
| `TryAdd` | Validate name/badge; duplicate check; `Add` |
| `TryUpdate` | Must exist; build new `Employee`; `Update` |

Constructor injection:

```csharp
public EmployeeService(IEmployeeRepository repository)
```

**DI-ready** — P39 will register this in a container instead of `new` in Program.

---

## Menu workflow

| # | Action |
|---|--------|
| 1 | List active employees |
| 2 | List all (incl. inactive) |
| 3 | Find by badge |
| 4 | Add employee |
| 5 | Update employee |
| 6 | Demo SQL stub error |
| 0 | Exit |

---

## Floor Assistant mapping

| P30 | Floor Assistant |
|-----|-----------------|
| `IEmployeeRepository` | DAL boundaries, `IErpService` |
| `EmployeeService` | Logic before/after `ExecSP` |
| In-memory repo | Rare; tests/mocks |
| SQL repo | `clsAccess.ExecSP` in P08 |
| Swap implementation | QA DB vs mock SAP |

---

## Run

```powershell
dotnet run --project projects/P30-InterfacesRepository/P30.Console.csproj
```

---

## Acceptance checklist

- [ ] Service constructor takes `IEmployeeRepository`
- [ ] Swap in-memory vs SQL stub without changing service
- [ ] CRUD works on in-memory store
- [ ] Menu 6 shows expected `NotImplementedException`

## FA homework

- [ ] Grep FA for `interface I` in `Helpers/` or `DAL/`

---

## Dependency rule (do not break)

```text
Program → Service → IRepository ← Implementations
   ✓         ✓           ✓
Repository → Service   ✗  (never)
Service → concrete InMemory only in Program wiring   ✓
```

---

## Next

**P31** — add `ValidationException`, file logs, JSON export on top of this pattern.  
**P08** — implement `SqlEmployeeRepository` with `DataAccess.ExecSp`.
