namespace PracticeFA.P00.Models;

/// <summary>
/// One stage on the factory floor (wax, casting, FSK, etc.).
/// </summary>
public sealed class WorkCenter
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? NextCode { get; init; }

    public string NextDisplay =>
        string.IsNullOrEmpty(NextCode) ? "(finished on floor)" : NextCode;
}
