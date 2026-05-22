using PracticeFA.P31.Repositories;
using PracticeFA.P31.Services;

var logger = new FileLogger();
var jsonStore = new JsonEmployeeStore();
IEmployeeRepository repository = new InMemoryEmployeeRepository();
var service = new EmployeeService(repository);

logger.LogInfo("P31 app started.");

Console.WriteLine("=== Practice FA · P31 · Exceptions, logging, JSON ===\n");
Console.WriteLine($"Log file: {logger.TodayLogPath}");
Console.WriteLine($"Export file: {jsonStore.ExportFilePath}\n");

try
{
    RunMenu(service, logger, jsonStore, repository);
}
catch (Exception ex)
{
    logger.LogError(ex, "Unhandled exception in main menu");
    Console.WriteLine();
    Console.WriteLine("Something went wrong. Details were written to the log file.");
    Console.WriteLine($"Log: {logger.TodayLogPath}");
    Console.WriteLine($"Message: {ex.Message}");
}

Console.WriteLine("\nPress Enter to exit.");
Console.ReadLine();

static void RunMenu(
    EmployeeService service,
    FileLogger logger,
    JsonEmployeeStore jsonStore,
    IEmployeeRepository repository)
{
    while (true)
    {
        try
        {
            Console.WriteLine("--- Menu ---");
            Console.WriteLine("  1  List employees");
            Console.WriteLine("  2  Add employee");
            Console.WriteLine("  3  Export employees → JSON");
            Console.WriteLine("  4  Import employees ← JSON (replaces in-memory list)");
            Console.WriteLine("  5  Force error (test log + catch)");
            Console.WriteLine("  0  Exit");
            Console.Write("Choice: ");

            var choice = Console.ReadLine()?.Trim();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    PrintEmployees(service.ListEmployees(activeOnly: false));
                    break;
                case "2":
                    AddEmployee(service, logger);
                    break;
                case "3":
                    ExportJson(service, jsonStore, logger);
                    break;
                case "4":
                    ImportJson(service, jsonStore, repository, logger);
                    break;
                case "5":
                    ForceError(logger);
                    break;
                case "0":
                    logger.LogInfo("User exit.");
                    return;
                default:
                    Console.WriteLine("Unknown option.\n");
                    break;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Menu action failed (choice handled in inner catch)");
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("(Full stack trace saved to log.)\n");
        }
    }
}

static void PrintEmployees(IReadOnlyList<PracticeFA.P31.Models.Employee> employees)
{
    if (employees.Count == 0)
    {
        Console.WriteLine("(no employees)\n");
        return;
    }

    foreach (var e in employees)
        Console.WriteLine($"  {e.ListLine}");
    Console.WriteLine();
}

static void AddEmployee(EmployeeService service, FileLogger logger)
{
    Console.Write("Badge ID (must start with E, e.g. E105): ");
    var badge = Console.ReadLine()?.Trim() ?? "";
    Console.Write("Display name: ");
    var name = Console.ReadLine()?.Trim() ?? "";
    Console.Write("Process center (optional): ");
    var pc = Console.ReadLine()?.Trim();

    if (!service.TryAdd(badge, name, pc, out var userMessage))
    {
        Console.WriteLine($"Could not add: {userMessage}");
        logger.LogInfo($"Add rejected: {userMessage}");
    }
    else
    {
        Console.WriteLine($"Added {badge}.");
        logger.LogInfo($"Added employee {badge}");
    }

    Console.WriteLine();
}

static void ExportJson(EmployeeService service, JsonEmployeeStore jsonStore, FileLogger logger)
{
    var all = service.ListEmployees(activeOnly: false);
    jsonStore.Export(all);
    Console.WriteLine($"Exported {all.Count} employee(s) to:");
    Console.WriteLine($"  {jsonStore.ExportFilePath}\n");
    logger.LogInfo($"Exported {all.Count} employees to JSON");
}

static void ImportJson(
    EmployeeService service,
    JsonEmployeeStore jsonStore,
    IEmployeeRepository repository,
    FileLogger logger)
{
    var imported = jsonStore.Import();
    repository.ReplaceAll(imported);
    Console.WriteLine($"Imported {imported.Count} employee(s) from JSON.\n");
    PrintEmployees(service.ListEmployees(activeOnly: false));
    logger.LogInfo($"Imported {imported.Count} employees from JSON");
}

static void ForceError(FileLogger logger)
{
    try
    {
        throw new InvalidOperationException("Deliberate test error for P31 logging lab.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "ForceError menu option");
        throw;
    }
}
