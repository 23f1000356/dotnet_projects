using PracticeFA.P30.Models;

namespace PracticeFA.P30.Repositories;

/// <summary>
/// Placeholder for P08 (SQL + stored procedures). Service code stays unchanged when this is implemented.
/// </summary>
public sealed class SqlEmployeeRepository : IEmployeeRepository
{
    public IReadOnlyList<Employee> GetAll(bool activeOnly = true) =>
        throw new NotImplementedException("Wire to DataAccess.ExecSp in P08.");

    public Employee? GetByBadge(string badgeId) =>
        throw new NotImplementedException("Wire to spGetEmployeeByBadge in P08.");

    public void Add(Employee employee) =>
        throw new NotImplementedException("Wire to spInsEmployee in P08.");

    public void Update(Employee employee) =>
        throw new NotImplementedException("Wire to spUpdEmployee in P08.");
}
