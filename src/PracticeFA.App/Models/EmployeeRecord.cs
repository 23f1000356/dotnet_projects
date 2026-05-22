namespace PracticeFA.App.Models;

/// <summary>Row from Employees table / CRUD SP result sets (P08).</summary>
public sealed class EmployeeRecord
{
    public int EmployeeId { get; init; }
    public string BadgeId { get; init; } = "";
    public string DisplayName { get; init; } = "";
    public string? ProcessCenter { get; init; }
    public bool IsActive { get; init; } = true;
}
