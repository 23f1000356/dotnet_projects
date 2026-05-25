using System.Data;
using Microsoft.Data.SqlClient;
using PracticeFA.App.Models;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>P39 — IAttendanceService backed by IDataAccess (injectable for tests).</summary>
public sealed class AttendanceServiceImpl : IAttendanceService
{
    private readonly IDataAccess _dataAccess;

    public AttendanceServiceImpl(IDataAccess dataAccess) => _dataAccess = dataAccess;

    public IReadOnlyList<AttendanceRecord> GetAll()
    {
        try
        {
            var table = _dataAccess.ExecSp("dbo.spGetAttendance");
            return AttendanceMapper.FromTable(table);
        }
        catch (SqlException ex) when (ex.Number == 2812)
        {
            throw new InvalidOperationException(
                "dbo.spGetAttendance not found. Run database/scripts/008_P05_Attendance.sql",
                ex);
        }
    }

    public AttendanceSaveResult Save(
        int attendanceId,
        string badgeId,
        DateTime workDate,
        DateTime clockInAt,
        DateTime? clockOutAt,
        string? notes,
        string? savedBy,
        decimal? weightGm = null)
    {
        try
        {
            var table = _dataAccess.ExecSp(
                "dbo.spSaveAttendance",
                new SqlParameter("@AttendanceId", attendanceId),
                new SqlParameter("@BadgeId", badgeId.Trim()),
                new SqlParameter("@WorkDate", workDate.Date),
                new SqlParameter("@ClockInAt", clockInAt),
                new SqlParameter("@ClockOutAt", (object?)clockOutAt ?? DBNull.Value),
                new SqlParameter("@Notes", (object?)notes?.Trim() ?? DBNull.Value),
                new SqlParameter("@SavedBy", (object?)savedBy?.Trim() ?? DBNull.Value),
                new SqlParameter("@WeightGm", (object?)weightGm ?? DBNull.Value));

            if (table.Rows.Count == 0)
                return AttendanceSaveResult.Fail("Attendance was not saved.");

            var record = AttendanceMapper.FromRow(table.Rows[0]);
            return AttendanceSaveResult.Ok(
                attendanceId == 0
                    ? $"Added attendance for {record.BadgeId}."
                    : $"Updated attendance #{record.AttendanceId}.",
                record);
        }
        catch (SqlException ex) when (ex.Number == 2812)
        {
            return AttendanceSaveResult.Fail(
                "dbo.spSaveAttendance not found. Run database/scripts/008_P05_Attendance.sql");
        }
        catch (SqlException ex)
        {
            return AttendanceSaveResult.Fail(ex.Message);
        }
    }
}
