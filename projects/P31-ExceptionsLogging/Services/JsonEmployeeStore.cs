using System.Text.Json;
using PracticeFA.P31.Models;

namespace PracticeFA.P31.Services;

public sealed class JsonEmployeeStore
{
    private readonly string _exportDirectory;
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public JsonEmployeeStore(string? baseDirectory = null)
    {
        _exportDirectory = Path.Combine(baseDirectory ?? AppContext.BaseDirectory, "export");
        Directory.CreateDirectory(_exportDirectory);
    }

    public string ExportFilePath => Path.Combine(_exportDirectory, "employees.json");

    public void Export(IReadOnlyList<Employee> employees)
    {
        var json = JsonSerializer.Serialize(employees, JsonOptions);
        File.WriteAllText(ExportFilePath, json);
    }

    public List<Employee> Import()
    {
        if (!File.Exists(ExportFilePath))
            throw new FileNotFoundException($"No export file at {ExportFilePath}. Run Export first.");

        var json = File.ReadAllText(ExportFilePath);
        return JsonSerializer.Deserialize<List<Employee>>(json, JsonOptions)
               ?? throw new InvalidOperationException("JSON file was empty or invalid.");
    }
}
