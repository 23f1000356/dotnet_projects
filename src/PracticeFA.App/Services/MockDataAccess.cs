using System.Data;
using Microsoft.Data.SqlClient;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>P39 — in-memory IDataAccess for design-time / tests (set PRACTICE_FA_MOCK_DB=1).</summary>
public sealed class MockDataAccess : IDataAccess
{
    public DataTable ExecSp(string procName, params SqlParameter[] parameters)
    {
        if (procName.Contains("spGetAttendance", StringComparison.OrdinalIgnoreCase))
            return CreateAttendanceTable();

        if (procName.Contains("spSaveAttendance", StringComparison.OrdinalIgnoreCase))
            return CreateAttendanceTable();

        if (procName.Contains("spGetOrderHeaders", StringComparison.OrdinalIgnoreCase))
            return CreateOrderHeadersTable();

        if (procName.Contains("spGetOrderLines", StringComparison.OrdinalIgnoreCase))
            return CreateOrderLinesTable(parameters);

        if (procName.Contains("spSaveOrder", StringComparison.OrdinalIgnoreCase))
            return CreateSaveOrderResult();

        return new DataTable();
    }

    private static DataTable CreateAttendanceTable()
    {
        var table = new DataTable();
        table.Columns.Add("AttendanceId", typeof(int));
        table.Columns.Add("BadgeId", typeof(string));
        table.Columns.Add("WorkDate", typeof(DateTime));
        table.Columns.Add("ClockInAt", typeof(DateTime));
        table.Columns.Add("ClockOutAt", typeof(DateTime));
        table.Columns.Add("Notes", typeof(string));
        table.Columns.Add("SavedBy", typeof(string));
        table.Columns.Add("SavedAt", typeof(DateTime));
        table.Columns.Add("WeightGm", typeof(decimal));

        table.Rows.Add(1, "E101", DateTime.Today, DateTime.Today.AddHours(7), DateTime.Today.AddHours(15),
            "Mock row (no SQL)", "mock", DateTime.UtcNow, 1250.500m);
        table.Rows.Add(2, "E102", DateTime.Today, DateTime.Today.AddHours(8), DBNull.Value,
            "Still on floor — mock", "mock", DateTime.UtcNow, 980.125m);

        return table;
    }

    private static DataTable CreateOrderHeadersTable()
    {
        var table = new DataTable();
        table.Columns.Add("OrderId", typeof(int));
        table.Columns.Add("BagTag", typeof(string));
        table.Columns.Add("OperatorBadge", typeof(string));
        table.Columns.Add("PlantCode", typeof(string));
        table.Columns.Add("CreatedBy", typeof(string));
        table.Columns.Add("CreatedUtc", typeof(DateTime));
        table.Columns.Add("LineCount", typeof(int));
        table.Columns.Add("TotalQuantity", typeof(int));
        table.Rows.Add(1, "BAG-MOCK-1", "E101", "P01", "mock", DateTime.UtcNow, 2, 15);
        return table;
    }

    private static DataTable CreateOrderLinesTable(SqlParameter[] parameters)
    {
        var table = new DataTable();
        table.Columns.Add("OrderLineId", typeof(int));
        table.Columns.Add("OrderId", typeof(int));
        table.Columns.Add("LineNumber", typeof(int));
        table.Columns.Add("SkuOrStyle", typeof(string));
        table.Columns.Add("Quantity", typeof(int));
        table.Rows.Add(1, 1, 1, "STYLE-A", 10);
        table.Rows.Add(2, 1, 2, "STYLE-B", 5);
        return table;
    }

    private static DataTable CreateSaveOrderResult()
    {
        var table = new DataTable();
        table.Columns.Add("OrderId", typeof(int));
        table.Columns.Add("BagTag", typeof(string));
        table.Columns.Add("OperatorBadge", typeof(string));
        table.Columns.Add("PlantCode", typeof(string));
        table.Columns.Add("CreatedBy", typeof(string));
        table.Columns.Add("CreatedUtc", typeof(DateTime));
        table.Rows.Add(99, "BAG-MOCK-NEW", "E101", "P01", "mock", DateTime.UtcNow);
        return table;
    }
}
