namespace PracticeFA.App.Models;

public sealed class AttendanceSaveResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = "";
    public AttendanceRecord? Record { get; init; }

    public static AttendanceSaveResult Ok(string message, AttendanceRecord record) =>
        new() { Success = true, Message = message, Record = record };

    public static AttendanceSaveResult Fail(string message) =>
        new() { Success = false, Message = message };
}
