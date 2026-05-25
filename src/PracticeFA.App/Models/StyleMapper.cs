using System.Data;

namespace PracticeFA.App.Models;

public static class StyleMapper
{
    public static StyleRecord FromRow(DataRow row) =>
        new()
        {
            StyleId = Convert.ToInt32(row["StyleId"]),
            StyleCode = Convert.ToString(row["StyleCode"]) ?? "",
            Description = Convert.ToString(row["Description"]) ?? "",
            CreatedBy = row["CreatedBy"] == DBNull.Value
                ? null
                : Convert.ToString(row["CreatedBy"]),
            CreatedUtc = row["CreatedUtc"] is DateTime created
                ? created
                : Convert.ToDateTime(row["CreatedUtc"]),
            UpdatedUtc = row["UpdatedUtc"] == DBNull.Value
                ? null
                : row["UpdatedUtc"] is DateTime updated
                    ? updated
                    : Convert.ToDateTime(row["UpdatedUtc"]),
        };
}
