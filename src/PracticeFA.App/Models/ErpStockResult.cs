namespace PracticeFA.App.Models;

/// <summary>P45 — result of IErpService.GetStockAsync (FA: PULLFromSAP return).</summary>
public sealed class ErpStockResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = "";
    public IReadOnlyList<ErpStockLine> Lines { get; init; } = [];

    public static ErpStockResult Ok(string message, IReadOnlyList<ErpStockLine> lines) =>
        new() { Success = true, Message = message, Lines = lines };

    public static ErpStockResult Fail(string message) =>
        new() { Success = false, Message = message };
}
