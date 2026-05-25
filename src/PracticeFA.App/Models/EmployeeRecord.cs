namespace PracticeFA.App.Models;

/// <summary>Row from Employees table / CRUD SP result sets (P08 + P41 audit).</summary>
public sealed class EmployeeRecord
{
    public int EmployeeId { get; init; }
    public string BadgeId { get; init; } = "";
    public string DisplayName { get; init; } = "";
    public string? ProcessCenter { get; init; }
    public bool IsActive { get; init; } = true;
    public string? CreatedBy { get; init; }
    public DateTime? CreatedAt { get; init; }
    public string? ModifiedBy { get; init; }
    public DateTime? ModifiedAt { get; init; }
}
