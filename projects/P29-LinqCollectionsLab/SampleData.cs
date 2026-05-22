using PracticeFA.P29.Models;

namespace PracticeFA.P29;

public static class SampleData
{
    public static List<FloorTicketRecord> CreateTickets() =>
    [
        new() { PoNumber = "PO-2026-001", WorkCenterCode = "CASTING", Quantity = 12, Date = new DateOnly(2026, 5, 1) },
        new() { PoNumber = "PO-2026-002", WorkCenterCode = "FSK", Quantity = 4, Date = new DateOnly(2026, 5, 2) },
        new() { PoNumber = "PO-2026-003", WorkCenterCode = "CASTING", Quantity = 3, Date = new DateOnly(2026, 5, 2) },
        new() { PoNumber = "PO-2026-004", WorkCenterCode = "GRINDING", Quantity = 8, Date = new DateOnly(2026, 5, 3) },
        new() { PoNumber = "PO-2026-005", WorkCenterCode = "WAXINJET", Quantity = 20, Date = new DateOnly(2026, 5, 3) },
        new() { PoNumber = "PO-2026-006", WorkCenterCode = "POL", Quantity = 6, Date = new DateOnly(2026, 5, 4) },
        new() { PoNumber = "PO-2026-007", WorkCenterCode = "CASTING", Quantity = 15, Date = new DateOnly(2026, 5, 5) },
        new() { PoNumber = "PO-2026-008", WorkCenterCode = "FKIT", Quantity = 2, Date = new DateOnly(2026, 5, 5) },
        new() { PoNumber = "PO-2026-009", WorkCenterCode = "FSK", Quantity = 9, Date = new DateOnly(2026, 5, 6) },
        new() { PoNumber = "PO-2026-010", WorkCenterCode = "RFD", Quantity = 1, Date = new DateOnly(2026, 5, 7) },
        new() { PoNumber = "PO-2026-011", WorkCenterCode = "GRINDING", Quantity = 7, Date = new DateOnly(2026, 5, 8) },
        new() { PoNumber = "PO-2026-012", WorkCenterCode = "CASTING", Quantity = 5, Date = new DateOnly(2026, 5, 9) },
    ];
}
