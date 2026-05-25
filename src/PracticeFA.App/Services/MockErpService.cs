using PracticeFA.App.Models;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>
/// P45 — simulates a 5s SAP RFC (ZKS_FM_* style) with progress ticks and cancellation.
/// </summary>
public sealed class MockErpService : IErpService
{
    private static readonly string[] ProgressSteps =
    [
        "Connecting to SAP (mock)…",
        "Calling ZKS_FM_GET_STOCK (mock)…",
        "Reading return table…",
        "Mapping materials to FA grid…",
        "Done.",
    ];

    public int SimulatedCallDurationMs => 5000;

    public async Task<ErpStockResult> GetStockAsync(
        string sku,
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sku))
            return ErpStockResult.Fail("Enter a SKU / material number.");

        var material = sku.Trim().ToUpperInvariant();
        var plant = AppState.CurrentUser?.PlantCode ?? "P01";

        return await Task.Run(async () =>
        {
            var stepDelay = SimulatedCallDurationMs / ProgressSteps.Length;

            for (var i = 0; i < ProgressSteps.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                progress?.Report(ProgressSteps[i]);
                await Task.Delay(stepDelay, cancellationToken).ConfigureAwait(false);
            }

            cancellationToken.ThrowIfCancellationRequested();

            var lines = new List<ErpStockLine>
            {
                new()
                {
                    Sku = material,
                    Plant = plant,
                    Quantity = 120,
                    Unit = "EA",
                },
                new()
                {
                    Sku = material,
                    Plant = "P02",
                    Quantity = 45,
                    Unit = "EA",
                },
            };

            return ErpStockResult.Ok(
                $"SAP mock returned {lines.Count} plant row(s) for {material}.",
                lines);
        }, cancellationToken).ConfigureAwait(false);
    }
}
