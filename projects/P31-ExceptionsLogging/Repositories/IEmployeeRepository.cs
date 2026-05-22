using PracticeFA.P31.Models;

namespace PracticeFA.P31.Repositories;

public interface IEmployeeRepository
{
    IReadOnlyList<Employee> GetAll(bool activeOnly = true);
    Employee? GetByBadge(string badgeId);
    void Add(Employee employee);
    void Update(Employee employee);
    void ReplaceAll(IEnumerable<Employee> employees);
}
