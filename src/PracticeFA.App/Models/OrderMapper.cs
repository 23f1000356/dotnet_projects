using System.Data;

namespace PracticeFA.App.Models;

public static class OrderMapper
{
    public static OrderHeaderSummary HeaderFromRow(DataRow row) =>
        new()
        {
            OrderId = Convert.ToInt32(row["OrderId"]),
            BagTag = Convert.ToString(row["BagTag"]) ?? "",
            OperatorBadge = Convert.ToString(row["OperatorBadge"]) ?? "",
            PlantCode = Convert.ToString(row["PlantCode"]) ?? "",
            CreatedBy = row["CreatedBy"] == DBNull.Value ? null : Convert.ToString(row["CreatedBy"]),
            CreatedUtc = row["CreatedUtc"] is DateTime dt
                ? dt
                : Convert.ToDateTime(row["CreatedUtc"]),
            LineCount = Convert.ToInt32(row["LineCount"]),
            TotalQuantity = Convert.ToInt32(row["TotalQuantity"]),
        };

    public static OrderLineRecord LineFromRow(DataRow row) =>
        new()
        {
            OrderLineId = Convert.ToInt32(row["OrderLineId"]),
            OrderId = Convert.ToInt32(row["OrderId"]),
            LineNumber = Convert.ToInt32(row["LineNumber"]),
            SkuOrStyle = Convert.ToString(row["SkuOrStyle"]) ?? "",
            Quantity = Convert.ToInt32(row["Quantity"]),
        };

    public static List<OrderHeaderSummary> HeadersFromTable(DataTable table)
    {
        var list = new List<OrderHeaderSummary>(table.Rows.Count);
        foreach (DataRow row in table.Rows)
            list.Add(HeaderFromRow(row));
        return list;
    }

    public static List<OrderLineRecord> LinesFromTable(DataTable table)
    {
        var list = new List<OrderLineRecord>(table.Rows.Count);
        foreach (DataRow row in table.Rows)
            list.Add(LineFromRow(row));
        return list;
    }
}
