namespace PracticeFA.P09.Models;

/// <summary>P09 Version B — one row for ObservableCollection binding (explicit columns in XAML).</summary>
public sealed class EmployeeRowViewModel
{
    public int EmployeeId { get; init; }
    public string BadgeId { get; init; } = "";
    public string DisplayName { get; init; } = "";
    public string? ProcessCenter { get; init; }
    public string Email { get; init; } = "";
    public bool IsActive { get; init; }
}
