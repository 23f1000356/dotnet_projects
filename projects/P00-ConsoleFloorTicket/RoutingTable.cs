using PracticeFA.P00.Models;

namespace PracticeFA.P00;

/// <summary>
/// In-memory routing — later this data lives in SQL / FA AppVariables.WCList.
/// </summary>
public static class RoutingTable
{
    private static readonly List<WorkCenter> Centers =
    [
        new() { Code = "FKIT", Name = "Full Kit", NextCode = "WAXINJET" },
        new() { Code = "WAXINJET", Name = "Wax injection", NextCode = "CASTING" },
        new() { Code = "CASTING", Name = "Casting", NextCode = "GRINDING" },
        new() { Code = "GRINDING", Name = "Grinding", NextCode = "FSK" },
        new() { Code = "FSK", Name = "Floor stone setting", NextCode = "POL" },
        new() { Code = "POL", Name = "Polish", NextCode = "RFD" },
        new() { Code = "RFD", Name = "Ready for dispatch", NextCode = null },
    ];

    public static WorkCenter? Find(string code) =>
        Centers.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

    public static void PrintAvailableCodes()
    {
        Console.WriteLine("Known work centers:");
        foreach (var c in Centers)
            Console.WriteLine($"  {c.Code,-10} {c.Name}  →  {c.NextDisplay}");
    }
}
