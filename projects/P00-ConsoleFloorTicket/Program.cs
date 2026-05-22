using PracticeFA.P00;
using PracticeFA.P00.Models;

Console.WriteLine("=== Practice FA · P00 · Floor ticket ===");
Console.WriteLine("Track a production order at one work center (console only).\n");

RoutingTable.PrintAvailableCodes();
Console.WriteLine();

var po = ReadRequired("Production order (e.g. PO-2026-001): ");
var wcCode = ReadRequired("Work center code (e.g. CASTING): ");
var qty = ReadQuantity();

if (!TryBuildTicket(po, wcCode, qty, out var ticket, out var nextStep, out var error))
{
    Console.WriteLine();
    Console.WriteLine($"Error: {error}");
    return 1;
}

Console.WriteLine();
Console.WriteLine("--- Routing line ---");
Console.WriteLine(ticket.FormatRoutingLine(nextStep));
Console.WriteLine();
Console.WriteLine("Done. Press Enter to exit.");
Console.ReadLine();
return 0;

static string ReadRequired(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var value = Console.ReadLine()?.Trim() ?? "";
        if (!string.IsNullOrEmpty(value))
            return value;
        Console.WriteLine("  (required — try again)");
    }
}

static int ReadQuantity()
{
    while (true)
    {
        Console.Write("Quantity (whole number > 0): ");
        var text = Console.ReadLine()?.Trim() ?? "";
        if (int.TryParse(text, out var qty) && qty > 0)
            return qty;
        Console.WriteLine("  (enter a number greater than 0)");
    }
}

static bool TryBuildTicket(
    string po,
    string wcCode,
    int qty,
    out ProductionTicket ticket,
    out string nextStep,
    out string error)
{
    ticket = null!;
    nextStep = "";
    error = "";

    if (string.IsNullOrWhiteSpace(po))
    {
        error = "PO number is required.";
        return false;
    }

    var center = RoutingTable.Find(wcCode);
    if (center is null)
    {
        error = $"Unknown work center '{wcCode}'. Use a code from the list above.";
        return false;
    }

    ticket = new ProductionTicket
    {
        PoNumber = po,
        WorkCenterCode = center.Code,
        Quantity = qty,
    };

    nextStep = center.NextDisplay;
    return true;
}
