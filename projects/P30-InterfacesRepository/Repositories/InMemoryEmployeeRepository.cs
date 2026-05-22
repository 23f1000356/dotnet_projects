using PracticeFA.P30.Models;

namespace PracticeFA.P30.Repositories;

public sealed class InMemoryEmployeeRepository : IEmployeeRepository
{
    private readonly List<Employee> _store =
    [
        new() { BadgeId = "E101", DisplayName = "Anil — Wax", ProcessCenter = "WAXINJET", IsActive = true },
        new() { BadgeId = "E102", DisplayName = "Meena — Casting", ProcessCenter = "CASTING", IsActive = true },
        new() { BadgeId = "E103", DisplayName = "Joel — FSK", ProcessCenter = "FSK", IsActive = true },
        new() { BadgeId = "E104", DisplayName = "Sara — QC", ProcessCenter = "FINALQC", IsActive = false },
    ];

    public IReadOnlyList<Employee> GetAll(bool activeOnly = true)
    {
        var query = _store.AsEnumerable();
        if (activeOnly)
            query = query.Where(e => e.IsActive);
        return query.OrderBy(e => e.BadgeId).ToList();
    }

    public Employee? GetByBadge(string badgeId) =>
        _store.FirstOrDefault(e => e.BadgeId.Equals(badgeId, StringComparison.OrdinalIgnoreCase));

    public void Add(Employee employee)
    {
        if (GetByBadge(employee.BadgeId) is not null)
            throw new InvalidOperationException($"Badge {employee.BadgeId} already exists.");

        _store.Add(employee);
    }

    public void Update(Employee employee)
    {
        var index = _store.FindIndex(e =>
            e.BadgeId.Equals(employee.BadgeId, StringComparison.OrdinalIgnoreCase));

        if (index < 0)
            throw new InvalidOperationException($"Badge {employee.BadgeId} not found.");

        _store[index] = employee;
    }
}
