using PracticeFA.App.Models;

namespace PracticeFA.App.Services.Abstractions;

/// <summary>P39 — attendance operations (uses IDataAccess internally).</summary>
public interface IAttendanceService
{
    IReadOnlyList<AttendanceRecord> GetAll();
    AttendanceSaveResult Save(
        int attendanceId,
        string badgeId,
        DateTime workDate,
        DateTime clockInAt,
        DateTime? clockOutAt,
        string? notes,
        string? savedBy,
        decimal? weightGm = null);
}
