using System.Data;

namespace PracticeFA.P09.Services;

/// <summary>In-memory employee table (includes Email for Version A demo).</summary>
public static class SampleEmployeeData
{
    public static DataTable CreateActiveEmployees()
    {
        var table = new DataTable("Employees");
        table.Columns.Add("EmployeeId", typeof(int));
        table.Columns.Add("BadgeId", typeof(string));
        table.Columns.Add("DisplayName", typeof(string));
        table.Columns.Add("ProcessCenter", typeof(string));
        table.Columns.Add("Email", typeof(string));
        table.Columns.Add("IsActive", typeof(bool));

        table.Rows.Add(1, "E101", "Sara Chen", "CASTING", "sara.chen@practice.local", true);
        table.Rows.Add(2, "E102", "Raj Patel", "FSK", "raj.patel@practice.local", true);
        table.Rows.Add(3, "E103", "Mia Lopez", "GRINDING", "mia.lopez@practice.local", true);
        table.Rows.Add(4, "E104", "Inactive User", "CASTING", "inactive@practice.local", false);

        return table;
    }
}
