using System.Collections.ObjectModel;
using System.Data;
using PracticeFA.P09.Models;

namespace PracticeFA.P09.Services;

public static class EmployeeRowMapper
{
    public static ObservableCollection<EmployeeRowViewModel> ToObservableCollection(DataTable table)
    {
        var rows = new ObservableCollection<EmployeeRowViewModel>();
        foreach (DataRow row in table.Rows)
            rows.Add(FromRow(row));

        return rows;
    }

    public static EmployeeRowViewModel FromRow(DataRow row) =>
        new()
        {
            EmployeeId = Convert.ToInt32(row["EmployeeId"]),
            BadgeId = Convert.ToString(row["BadgeId"]) ?? "",
            DisplayName = Convert.ToString(row["DisplayName"]) ?? "",
            ProcessCenter = row.Table.Columns.Contains("ProcessCenter")
                ? Convert.ToString(row["ProcessCenter"])
                : null,
            Email = row.Table.Columns.Contains("Email")
                ? Convert.ToString(row["Email"]) ?? ""
                : "",
            IsActive = row.Table.Columns.Contains("IsActive") && Convert.ToBoolean(row["IsActive"]),
        };
}
