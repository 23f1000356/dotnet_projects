using System.Configuration;
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
    /// <summary>Offline accounts when SQL is not installed (P02/P06 learning).</summary>
    private static readonly Dictionary<string, (string Password, UserInfo User, int[] Modules)> DemoAccounts =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["operator1"] = ("pass1",
                new UserInfo { UserId = "operator1", DisplayName = "Floor Operator One", PlantCode = "P01" },
                [ModuleIds.StyleCreation, ModuleIds.BaggingEntry, ModuleIds.AttendanceList]),
            ["manager1"] = ("pass1",
                new UserInfo { UserId = "manager1", DisplayName = "Plant Manager", PlantCode = "P01" },
                [ModuleIds.StyleCreation, ModuleIds.BaggingEntry, ModuleIds.MisProductivity,
                    ModuleIds.EmployeeMaintenance, ModuleIds.AttendanceList]),
            ["operator2"] = ("pass2",
                new UserInfo { UserId = "operator2", DisplayName = "Floor Operator Two", PlantCode = "P02" },
                [ModuleIds.BaggingEntry, ModuleIds.MisProductivity]),
        };

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

            var user = UserInfoMapper.FromRow(table.Rows[0]);
            LoadUserModulesIntoSession(user.UserId);
            return LoginResult.Ok(user);
        }
        catch (SqlException)
        {
            var demo = TryDemoLogin(userId, password);
            if (demo is not null)
                return demo;

            return LoginResult.Fail(
                "Cannot connect to database PracticeFA.\n\n" +
                "For P02 without SQL, use offline demo:\n" +
                "  operator1 / pass1  ·  manager1 / pass1  ·  operator2 / pass2\n\n" +
                "Or install LocalDB and run database/scripts/001_PracticeFA.sql");
        }
        catch (Exception ex) when (ex is InvalidOperationException or ConfigurationErrorsException)
        {
            var demo = TryDemoLogin(userId, password);
            if (demo is not null)
                return demo;

            return LoginResult.Fail(
                "Database is not configured (App.config).\n\n" +
                "Use offline demo: operator1 / pass1");
        }
        catch (Exception)
        {
            return LoginResult.Fail("Sign-in failed. Contact IT support.");
        }
    }

    private static LoginResult? TryDemoLogin(string userId, string password)
    {
        var id = userId.Trim();
        if (!DemoAccounts.TryGetValue(id, out var account) ||
            !string.Equals(account.Password, password, StringComparison.Ordinal))
            return null;

        var moduleLabels = account.Modules.Select(m => m.ToString()).ToArray();
        AppState.SetAllowedModules(account.Modules, string.Join(" · ", moduleLabels) + " (offline demo)");
        return LoginResult.Ok(account.User);
    }

    /// <summary>P07 — load once per login, cache in AppState.</summary>
    public static void LoadUserModulesIntoSession(string userId)
    {
        var table = GetUserModules(userId);
        var ids = new List<int>();
        var displayParts = new List<string>();

        foreach (DataRow row in table.Rows)
        {
            var moduleId = Convert.ToInt32(row["ModuleId"]);
            var moduleName = Convert.ToString(row["ModuleName"]) ?? "";
            ids.Add(moduleId);
            displayParts.Add($"{moduleId} {moduleName}");
        }

        AppState.SetAllowedModules(ids, string.Join(" · ", displayParts));
    }

    public static DataTable GetUserModules(string userId) =>
        DataAccess.ExecSp("dbo.spGetUserModules", new SqlParameter("@UserId", userId));
}
