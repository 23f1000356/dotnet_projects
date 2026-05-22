using PracticeFA.P30.Repositories;
using PracticeFA.P30.Services;

Console.WriteLine("=== Practice FA · P30 · Interfaces + repository ===\n");

// Swap repository here — service code does not change (DI-ready pattern).
IEmployeeRepository repository = new InMemoryEmployeeRepository();
// IEmployeeRepository repository = new SqlEmployeeRepository();  // P08

var service = new EmployeeService(repository);
Console.WriteLine($"Storage: {repository.GetType().Name}\n");

RunMenu(service);

static void RunMenu(EmployeeService service)
{
    while (true)
    {
        Console.WriteLine("--- Menu ---");
        Console.WriteLine("  1  List active employees");
        Console.WriteLine("  2  List all (incl. inactive)");
        Console.WriteLine("  3  Find by badge");
        Console.WriteLine("  4  Add employee");
        Console.WriteLine("  5  Update employee");
        Console.WriteLine("  6  Demo SQL stub (NotImplemented)");
        Console.WriteLine("  0  Exit");
        Console.Write("Choice: ");

        var choice = Console.ReadLine()?.Trim();
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                PrintEmployees(service.ListEmployees());
                break;
            case "2":
                PrintEmployees(service.ListEmployees(activeOnly: false));
                break;
            case "3":
                FindEmployee(service);
                break;
            case "4":
                AddEmployee(service);
                break;
            case "5":
                UpdateEmployee(service);
                break;
            case "6":
                DemoSqlStub();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Unknown option.\n");
                break;
        }
    }
}

static void PrintEmployees(IReadOnlyList<PracticeFA.P30.Models.Employee> employees)
{
    if (employees.Count == 0)
    {
        Console.WriteLine("(no employees)\n");
        return;
    }

    Console.WriteLine($"  {"Badge",-8} {"Name",-22} {"Process",-12} Status");
    foreach (var e in employees)
        Console.WriteLine($"  {e.ListLine}");
    Console.WriteLine();
}

static void FindEmployee(EmployeeService service)
{
    Console.Write("Badge ID: ");
    var badge = Console.ReadLine()?.Trim() ?? "";
    var emp = service.Find(badge);
    if (emp is null)
        Console.WriteLine($"Not found: {badge}\n");
    else
        Console.WriteLine($"  {emp.ListLine}\n");
}

static void AddEmployee(EmployeeService service)
{
    Console.Write("Badge ID: ");
    var badge = Console.ReadLine()?.Trim() ?? "";
    Console.Write("Display name: ");
    var name = Console.ReadLine()?.Trim() ?? "";
    Console.Write("Process center (optional): ");
    var pc = Console.ReadLine()?.Trim();

    if (!service.TryAdd(badge, name, pc, out var error))
        Console.WriteLine($"Error: {error}\n");
    else
        Console.WriteLine($"Added {badge}.\n");
}

static void UpdateEmployee(EmployeeService service)
{
    Console.Write("Badge ID: ");
    var badge = Console.ReadLine()?.Trim() ?? "";
    var existing = service.Find(badge);
    if (existing is null)
    {
        Console.WriteLine($"Not found: {badge}\n");
        return;
    }

    Console.WriteLine($"Current: {existing.ListLine}");
    Console.Write("New display name: ");
    var name = Console.ReadLine()?.Trim() ?? "";
    Console.Write("New process center (optional): ");
    var pc = Console.ReadLine()?.Trim();
    Console.Write("Active? (y/n): ");
    var active = !Console.ReadLine()?.Trim().Equals("n", StringComparison.OrdinalIgnoreCase) ?? true;

    if (!service.TryUpdate(badge, name, pc, active, out var error))
        Console.WriteLine($"Error: {error}\n");
    else
        Console.WriteLine($"Updated {badge}.\n");
}

static void DemoSqlStub()
{
    var sqlService = new EmployeeService(new SqlEmployeeRepository());
    try
    {
        _ = sqlService.ListEmployees();
        Console.WriteLine("Unexpected: SQL repo returned data.\n");
    }
    catch (NotImplementedException ex)
    {
        Console.WriteLine($"Expected: {ex.Message}\n");
        Console.WriteLine("In P08 you replace SqlEmployeeRepository — EmployeeService stays the same.\n");
    }
}
