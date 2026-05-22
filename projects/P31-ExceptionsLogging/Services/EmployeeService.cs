using PracticeFA.P31.Exceptions;
using PracticeFA.P31.Models;
using PracticeFA.P31.Repositories;

namespace PracticeFA.P31.Services;

public sealed class EmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository) => _repository = repository;

    public IReadOnlyList<Employee> ListEmployees(bool activeOnly = true) =>
        _repository.GetAll(activeOnly);

    public Employee? Find(string badgeId) => _repository.GetByBadge(badgeId);

    public bool TryAdd(string badgeId, string displayName, string? processCenter, out string userMessage)
    {
        userMessage = "";
        try
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                userMessage = "Display name is required.";
                return false;
            }

            _repository.Add(new Employee
            {
                BadgeId = badgeId.Trim(),
                DisplayName = displayName.Trim(),
                ProcessCenter = string.IsNullOrWhiteSpace(processCenter) ? null : processCenter.Trim(),
                IsActive = true,
            });
            return true;
        }
        catch (ValidationException ex)
        {
            userMessage = ex.Message;
            return false;
        }
        catch (Exception ex)
        {
            userMessage = "Could not add employee. IT has been notified (see log).";
            throw new InvalidOperationException(userMessage, ex);
        }
    }

    public bool TryUpdate(string badgeId, string displayName, string? processCenter, bool isActive, out string userMessage)
    {
        userMessage = "";
        try
        {
            var existing = _repository.GetByBadge(badgeId);
            if (existing is null)
            {
                userMessage = $"Employee {badgeId} not found.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                userMessage = "Display name is required.";
                return false;
            }

            _repository.Update(new Employee
            {
                BadgeId = existing.BadgeId,
                DisplayName = displayName.Trim(),
                ProcessCenter = string.IsNullOrWhiteSpace(processCenter) ? null : processCenter.Trim(),
                IsActive = isActive,
            });
            return true;
        }
        catch (ValidationException ex)
        {
            userMessage = ex.Message;
            return false;
        }
    }

    public void ReplaceAllFromImport(IReadOnlyList<Employee> employees) =>
        _repository.ReplaceAll(employees);
}
