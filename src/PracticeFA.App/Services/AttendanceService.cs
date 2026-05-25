using PracticeFA.App.Models;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>P05 facade — delegates to IAttendanceService from DI (P39).</summary>
public static class AttendanceService
{
    public static IReadOnlyList<AttendanceRecord> GetAll() => Get().GetAll();

    public static AttendanceSaveResult Save(
        int attendanceId,
        string badgeId,
        DateTime workDate,
        DateTime clockInAt,
        DateTime? clockOutAt,
        string? notes,
        string? savedBy,
        decimal? weightGm = null) =>
        Get().Save(attendanceId, badgeId, workDate, clockInAt, clockOutAt, notes, savedBy, weightGm);

    private static IAttendanceService Get() => App.GetRequiredService<IAttendanceService>();
}
