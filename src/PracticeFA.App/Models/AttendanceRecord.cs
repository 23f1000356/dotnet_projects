namespace PracticeFA.App.Models;

/// <summary>P05 — row from dbo.Attendance / spGetAttendance.</summary>
public sealed class AttendanceRecord
{
    public int AttendanceId { get; init; }
    public string BadgeId { get; init; } = "";
    public DateTime WorkDate { get; init; }
    public DateTime ClockInAt { get; init; }
    public DateTime? ClockOutAt { get; init; }
    public string? Notes { get; init; }
    public string? SavedBy { get; init; }
    public DateTime SavedAt { get; init; }
    public decimal? WeightGm { get; init; }

    public string ClockInDisplay => ClockInAt.ToLocalTime().ToString("g");
    public string ClockOutDisplay => ClockOutAt.HasValue
        ? ClockOutAt.Value.ToLocalTime().ToString("g")
        : "—";
}
