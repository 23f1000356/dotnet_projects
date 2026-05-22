namespace PracticeFA.P31.Models;

public sealed class Employee
{
    public required string BadgeId { get; init; }
    public required string DisplayName { get; init; }
    public string? ProcessCenter { get; init; }
    public bool IsActive { get; init; } = true;

    public string ListLine =>
        $"{BadgeId,-8} {DisplayName,-22} {(ProcessCenter ?? "-"),-12} {(IsActive ? "Active" : "Inactive")}";
}
