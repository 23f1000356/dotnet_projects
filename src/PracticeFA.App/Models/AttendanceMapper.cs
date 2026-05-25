using System.Data;

namespace PracticeFA.App.Models;

public static class AttendanceMapper
{
    public static AttendanceRecord FromRow(DataRow row) =>
        new()
        {
            AttendanceId = Convert.ToInt32(row["AttendanceId"]),
            BadgeId = Convert.ToString(row["BadgeId"]) ?? "",
            WorkDate = row["WorkDate"] is DateTime wd
                ? wd.Date
                : Convert.ToDateTime(row["WorkDate"]).Date,
            ClockInAt = row["ClockInAt"] is DateTime ci
                ? ci
                : Convert.ToDateTime(row["ClockInAt"]),
            ClockOutAt = row["ClockOutAt"] == DBNull.Value
                ? null
                : row["ClockOutAt"] is DateTime co
                    ? co
                    : Convert.ToDateTime(row["ClockOutAt"]),
            Notes = row["Notes"] == DBNull.Value ? null : Convert.ToString(row["Notes"]),
            SavedBy = row.Table.Columns.Contains("SavedBy") && row["SavedBy"] != DBNull.Value
                ? Convert.ToString(row["SavedBy"])
                : null,
            SavedAt = row.Table.Columns.Contains("SavedAt") && row["SavedAt"] != DBNull.Value
                ? row["SavedAt"] is DateTime sa ? sa : Convert.ToDateTime(row["SavedAt"])
                : DateTime.UtcNow,
            WeightGm = row.Table.Columns.Contains("WeightGm") && row["WeightGm"] != DBNull.Value
                ? Convert.ToDecimal(row["WeightGm"])
                : null,
        };

    public static List<AttendanceRecord> FromTable(DataTable table)
    {
        var list = new List<AttendanceRecord>(table.Rows.Count);
        foreach (DataRow row in table.Rows)
            list.Add(FromRow(row));
        return list;
    }
}
