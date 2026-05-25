using PracticeFA.App.Models;

namespace PracticeFA.App.Services.Abstractions;

/// <summary>P36 save + P40 master-detail read.</summary>
public interface IOrderService
{
    IReadOnlyList<OrderHeaderSummary> GetHeaders();
    IReadOnlyList<OrderLineRecord> GetLines(int orderId);
    OrderSaveResult Save(
        string bagTag,
        string operatorBadge,
        string plantCode,
        string? createdBy,
        IReadOnlyList<OrderLineInput> lines,
        bool simulateLine2Failure = false);
}
