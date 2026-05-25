using PracticeFA.App.Models;

namespace PracticeFA.App.Services.Abstractions;

/// <summary>P20 — simulates a slow FA stored-procedure call (Task.Run + minimum delay).</summary>
public interface IAsyncDemoService
{
    /// <summary>Minimum simulated latency in milliseconds (FA: long-running SP / SAP).</summary>
    int SimulatedDelayMs { get; }

    Task<IReadOnlyList<AsyncDemoRow>> LoadOrderHeadersSlowAsync(CancellationToken cancellationToken = default);
}
