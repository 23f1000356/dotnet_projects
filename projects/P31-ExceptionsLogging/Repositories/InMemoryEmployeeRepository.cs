using PracticeFA.P31.Exceptions;
using PracticeFA.P31.Models;

namespace PracticeFA.P31.Repositories;

public sealed class InMemoryEmployeeRepository : IEmployeeRepository
{
    private readonly List<Employee> _store =
    [
        new() { BadgeId = "E101", DisplayName = "Anil — Wax", ProcessCenter = "WAXINJET", IsActive = true },
        new() { BadgeId = "E102", DisplayName = "Meena — Casting", ProcessCenter = "CASTING", IsActive = true },
        new() { BadgeId = "E103", DisplayName = "Joel — FSK", ProcessCenter = "FSK", IsActive = true },
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
        ValidateBadge(employee.BadgeId);

        if (GetByBadge(employee.BadgeId) is not null)
            throw new InvalidOperationException($"Badge {employee.BadgeId} already exists.");

        _store.Add(employee);
    }

    public void Update(Employee employee)
    {
        ValidateBadge(employee.BadgeId);

        var index = _store.FindIndex(e =>
            e.BadgeId.Equals(employee.BadgeId, StringComparison.OrdinalIgnoreCase));

        if (index < 0)
            throw new InvalidOperationException($"Badge {employee.BadgeId} not found.");

        _store[index] = employee;
    }

    public void ReplaceAll(IEnumerable<Employee> employees)
    {
        _store.Clear();
        foreach (var e in employees)
        {
            ValidateBadge(e.BadgeId);
            _store.Add(e);
        }
    }

    private static void ValidateBadge(string badgeId)
    {
        if (string.IsNullOrWhiteSpace(badgeId))
            throw new ValidationException("Badge ID is required.");

        if (!badgeId.StartsWith('E') || badgeId.Length < 4)
            throw new ValidationException("Badge ID must start with E and have at least 4 characters (e.g. E105).");
    }
}
