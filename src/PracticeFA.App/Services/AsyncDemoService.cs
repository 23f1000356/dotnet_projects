using PracticeFA.App.Models;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>
/// P20 — runs dbo.spGetOrderHeaders on a background thread after a 3s delay so the UI stays responsive.
/// </summary>
public sealed class AsyncDemoService : IAsyncDemoService
{
    private readonly IDataAccess _dataAccess;

    public AsyncDemoService(IDataAccess dataAccess) => _dataAccess = dataAccess;

    public int SimulatedDelayMs => 3000;

    public async Task<IReadOnlyList<AsyncDemoRow>> LoadOrderHeadersSlowAsync(
        CancellationToken cancellationToken = default)
    {
        return await Task.Run(async () =>
        {
            await Task.Delay(SimulatedDelayMs, cancellationToken).ConfigureAwait(false);

            var table = _dataAccess.ExecSp("dbo.spGetOrderHeaders");
            var headers = OrderMapper.HeadersFromTable(table);
            return headers
                .Select(h => new AsyncDemoRow
                {
                    OrderId = h.OrderId,
                    BagTag = h.BagTag,
                    TotalQuantity = h.TotalQuantity,
                })
                .ToList();
        }, cancellationToken).ConfigureAwait(false);
    }
}
