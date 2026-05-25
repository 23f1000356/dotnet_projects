using PracticeFA.App.Models;

namespace PracticeFA.App.Services.Abstractions;

/// <summary>
/// P14/P45 — ERP facade (FA: SAPRFCHandler / PULLFromSAP). ViewModels depend on this only.
/// </summary>
public interface IErpService
{
    /// <summary>Simulated SAP RFC duration (P45 demo).</summary>
    int SimulatedCallDurationMs { get; }

    Task<ErpStockResult> GetStockAsync(
        string sku,
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default);
}
