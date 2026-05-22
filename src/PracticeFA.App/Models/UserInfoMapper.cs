using System.Data;

namespace PracticeFA.App.Models;

public static class UserInfoMapper
{
    public static UserInfo FromRow(DataRow row) =>
        new()
        {
            UserId = Convert.ToString(row["UserId"]) ?? "",
            DisplayName = Convert.ToString(row["DisplayName"]) ?? "",
            PlantCode = Convert.ToString(row["PlantCode"]) ?? "",
        };
}
