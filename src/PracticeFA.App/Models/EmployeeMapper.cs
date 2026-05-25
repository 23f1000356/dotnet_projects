using System.Data;

namespace PracticeFA.App.Models;

public static class EmployeeMapper
{
    public static EmployeeRecord FromRow(DataRow row) =>
        new()
        {
            EmployeeId = Convert.ToInt32(row["EmployeeId"]),
            BadgeId = Convert.ToString(row["BadgeId"]) ?? "",
            DisplayName = Convert.ToString(row["DisplayName"]) ?? "",
            ProcessCenter = row["ProcessCenter"] == DBNull.Value
                ? null
                : Convert.ToString(row["ProcessCenter"]),
            IsActive = row["IsActive"] is bool b ? b : Convert.ToBoolean(row["IsActive"]),
            CreatedBy = GetOptionalString(row, "CreatedBy"),
            CreatedAt = GetOptionalDateTime(row, "CreatedAt"),
            ModifiedBy = GetOptionalString(row, "ModifiedBy"),
            ModifiedAt = GetOptionalDateTime(row, "ModifiedAt"),
        };

    public static List<EmployeeRecord> FromTable(DataTable table)
    {
        var list = new List<EmployeeRecord>(table.Rows.Count);
        foreach (DataRow row in table.Rows)
            list.Add(FromRow(row));
        return list;
    }

    private static string? GetOptionalString(DataRow row, string column) =>
        row.Table.Columns.Contains(column) && row[column] != DBNull.Value
            ? Convert.ToString(row[column])
            : null;

    private static DateTime? GetOptionalDateTime(DataRow row, string column) =>
        row.Table.Columns.Contains(column) && row[column] != DBNull.Value
            ? row[column] is DateTime dt ? dt : Convert.ToDateTime(row[column])
            : null;
}
