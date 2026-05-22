using PracticeFA.P30.Models;

namespace PracticeFA.P30.Repositories;

/// <summary>
/// Data access contract — FA uses similar patterns for DAL / IErpService facades.
/// </summary>
public interface IEmployeeRepository
{
    IReadOnlyList<Employee> GetAll(bool activeOnly = true);
    Employee? GetByBadge(string badgeId);
    void Add(Employee employee);
    void Update(Employee employee);
}
