using PracticeFA.App.Models;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>P36/P40 facade — delegates to IOrderService (P39 DI).</summary>
public static class OrderService
{
    public static IReadOnlyList<OrderHeaderSummary> GetHeaders() => Get().GetHeaders();

    public static IReadOnlyList<OrderLineRecord> GetLines(int orderId) => Get().GetLines(orderId);

    public static OrderSaveResult Save(
        string bagTag,
        string operatorBadge,
        string plantCode,
        string? createdBy,
        IReadOnlyList<OrderLineInput> lines,
        bool simulateLine2Failure = false) =>
        Get().Save(bagTag, operatorBadge, plantCode, createdBy, lines, simulateLine2Failure);

    private static IOrderService Get() => App.GetRequiredService<IOrderService>();
}
