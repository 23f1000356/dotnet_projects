using PracticeFA.P29;
using PracticeFA.P29.Models;

var tickets = SampleData.CreateTickets();

Console.WriteLine("=== Practice FA · P29 · LINQ & collections lab ===\n");
Console.WriteLine($"Sample data: {tickets.Count} floor ticket rows.\n");
PrintHeader();

foreach (var t in tickets)
    Console.WriteLine(t);

Console.WriteLine("\n" + new string('=', 60));

// Query 1 — method syntax: all CASTING
Console.WriteLine("\n1) CASTING only  (method syntax: .Where(...))");
var castingOnly = tickets
    .Where(t => t.WorkCenterCode.Equals("CASTING", StringComparison.OrdinalIgnoreCase));

PrintRows(castingOnly);

// Query 2 — query syntax: quantity > 5
Console.WriteLine("\n2) Quantity > 5  (query syntax: from ... where ...)");
var highQty =
    from t in tickets
    where t.Quantity > 5
    select t;

PrintRows(highQty);

// Query 3 — method syntax: order by date
Console.WriteLine("\n3) All tickets by date  (method syntax: .OrderBy(...))");
var byDate = tickets.OrderBy(t => t.Date);

PrintRows(byDate);

// Query 4 — method syntax: GroupBy work center → count
Console.WriteLine("\n4) Count per work center  (method syntax: .GroupBy(...))");
var perWorkCenter = tickets
    .GroupBy(t => t.WorkCenterCode)
    .OrderBy(g => g.Key);

Console.WriteLine($"  {"WorkCenter",-12} {"Count",5} {"TotalQty",8}");
foreach (var group in perWorkCenter)
{
    var totalQty = group.Sum(t => t.Quantity);
    Console.WriteLine($"  {group.Key,-12} {group.Count(),5} {totalQty,8}");
}

// Bonus — combined filter (method + OrderByDescending)
Console.WriteLine("\n5) CASTING with qty > 5, newest first  (bonus: chained LINQ)");
var castingBigNewest = tickets
    .Where(t => t.WorkCenterCode == "CASTING" && t.Quantity > 5)
    .OrderByDescending(t => t.Date);

PrintRows(castingBigNewest);

Console.WriteLine("\nDone. Press Enter to exit.");
Console.ReadLine();

static void PrintHeader() =>
    Console.WriteLine($"  {"Date",-10} | {"PO",-14} | {"WC",-10} | Qty");

static void PrintRows(IEnumerable<FloorTicketRecord> rows)
{
    var list = rows.ToList();
    if (list.Count == 0)
    {
        Console.WriteLine("  (no rows)");
        return;
    }

    foreach (var t in list)
        Console.WriteLine($"  {t}");
}
