using System.Data;
using Microsoft.Data.SqlClient;
using PracticeFA.App.Models;

namespace PracticeFA.App.Services;

public sealed class LoginResult
{
    public bool Success { get; init; }
    public UserInfo? User { get; init; }
    public string Message { get; init; } = "";

    public static LoginResult Ok(UserInfo user) =>
        new() { Success = true, User = user, Message = "Login successful." };

    public static LoginResult Fail(string message) =>
        new() { Success = false, Message = message };
}

public static class LoginService
{
    public static LoginResult TryLogin(string userId, string password)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(password))
            return LoginResult.Fail("Enter User ID and password.");

        try
        {
            var table = DataAccess.ExecSp(
                "dbo.spLogin",
                new SqlParameter("@UserId", userId.Trim()),
                new SqlParameter("@Password", password));

            if (table.Rows.Count == 0)
                return LoginResult.Fail("Invalid User ID or password.");

            return LoginResult.Ok(UserInfoMapper.FromRow(table.Rows[0]));
        }
        catch (SqlException)
        {
            return LoginResult.Fail(
                "Cannot connect to database PracticeFA.\n\n" +
                "1. Install SQL Server Express or LocalDB\n" +
                "2. Run database/scripts/001_PracticeFA.sql in SSMS\n" +
                "3. Check App.config connection string");
        }
        catch (Exception)
        {
            return LoginResult.Fail("Sign-in failed. Contact IT support.");
        }
    }

    /// <summary>Loads module list for P07 — cached after login if needed.</summary>
    public static DataTable GetUserModules(string userId) =>
        DataAccess.ExecSp("dbo.spGetUserModules", new SqlParameter("@UserId", userId));
}
