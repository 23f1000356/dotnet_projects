using PracticeFA.P30.Models;
using PracticeFA.P30.Repositories;

namespace PracticeFA.P30.Services;

/// <summary>
/// Business logic — only talks to IEmployeeRepository (never List directly).
/// </summary>
public sealed class EmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyList<Employee> ListEmployees(bool activeOnly = true) =>
        _repository.GetAll(activeOnly);

    public Employee? Find(string badgeId) =>
        _repository.GetByBadge(badgeId);

    public bool TryAdd(string badgeId, string displayName, string? processCenter, out string error)
    {
        error = "";

        if (string.IsNullOrWhiteSpace(badgeId))
        {
            error = "Badge ID is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            error = "Display name is required.";
            return false;
        }

        if (_repository.GetByBadge(badgeId) is not null)
        {
            error = $"Employee {badgeId} already exists.";
            return false;
        }

        try
        {
            _repository.Add(new Employee
            {
                BadgeId = badgeId.Trim(),
                DisplayName = displayName.Trim(),
                ProcessCenter = string.IsNullOrWhiteSpace(processCenter) ? null : processCenter.Trim(),
                IsActive = true,
            });
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }

    public bool TryUpdate(string badgeId, string displayName, string? processCenter, bool isActive, out string error)
    {
        error = "";

        var existing = _repository.GetByBadge(badgeId);
        if (existing is null)
        {
            error = $"Employee {badgeId} not found.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            error = "Display name is required.";
            return false;
        }

        try
        {
            _repository.Update(new Employee
            {
                BadgeId = existing.BadgeId,
                DisplayName = displayName.Trim(),
                ProcessCenter = string.IsNullOrWhiteSpace(processCenter) ? null : processCenter.Trim(),
                IsActive = isActive,
            });
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }
}
