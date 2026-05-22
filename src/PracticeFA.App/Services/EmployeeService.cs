using System.Data;
using Microsoft.Data.SqlClient;
using PracticeFA.App.Models;

namespace PracticeFA.App.Services;

/// <summary>P08 — employee CRUD via stored procedures only (no SQL in Views).</summary>
public static class EmployeeService
{
    public static DataTable GetEmployees(bool activeOnly = true) =>
        DataAccess.ExecSp(
            "dbo.spGetEmployees",
            new SqlParameter("@ActiveOnly", activeOnly));

    public static EmployeeRecord? GetByBadge(string badgeId)
    {
        var table = DataAccess.ExecSp(
            "dbo.spGetEmployeeByBadge",
            new SqlParameter("@BadgeId", badgeId.Trim()));

        return table.Rows.Count == 0 ? null : EmployeeMapper.FromRow(table.Rows[0]);
    }

    public static OperationResult Insert(string badgeId, string displayName, string? processCenter)
    {
        var validation = Validate(badgeId, displayName);
        if (!validation.Success)
            return validation;

        try
        {
            var table = DataAccess.ExecSp(
                "dbo.spInsEmployee",
                new SqlParameter("@BadgeId", badgeId.Trim()),
                new SqlParameter("@DisplayName", displayName.Trim()),
                new SqlParameter("@ProcessCenter", (object?)processCenter?.Trim() ?? DBNull.Value));

            return OperationResult.Ok("Employee added.", EmployeeMapper.FromRow(table.Rows[0]));
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            return OperationResult.Fail("Badge ID already exists.");
        }
        catch (SqlException)
        {
            return OperationResult.Fail("Database error while adding employee.");
        }
    }

    public static OperationResult Update(EmployeeRecord employee)
    {
        var validation = Validate(employee.BadgeId, employee.DisplayName);
        if (!validation.Success)
            return validation;

        try
        {
            var table = DataAccess.ExecSp(
                "dbo.spUpdEmployee",
                new SqlParameter("@EmployeeId", employee.EmployeeId),
                new SqlParameter("@BadgeId", employee.BadgeId.Trim()),
                new SqlParameter("@DisplayName", employee.DisplayName.Trim()),
                new SqlParameter("@ProcessCenter", (object?)employee.ProcessCenter?.Trim() ?? DBNull.Value),
                new SqlParameter("@IsActive", employee.IsActive));

            if (table.Rows.Count == 0)
                return OperationResult.Fail("Employee not found.");

            return OperationResult.Ok("Employee updated.", EmployeeMapper.FromRow(table.Rows[0]));
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            return OperationResult.Fail("Badge ID already used by another employee.");
        }
        catch (SqlException)
        {
            return OperationResult.Fail("Database error while updating employee.");
        }
    }

    public static OperationResult SoftDelete(int employeeId)
    {
        try
        {
            DataAccess.ExecSp("dbo.spDelEmployee", new SqlParameter("@EmployeeId", employeeId));
            return OperationResult.Ok("Employee deactivated (soft delete).");
        }
        catch (SqlException)
        {
            return OperationResult.Fail("Database error while deleting employee.");
        }
    }

    private static OperationResult Validate(string badgeId, string displayName)
    {
        if (string.IsNullOrWhiteSpace(badgeId))
            return OperationResult.Fail("Badge ID is required.");
        if (badgeId.Trim().Length < 4)
            return OperationResult.Fail("Badge ID must be at least 4 characters.");
        if (!badgeId.Trim().StartsWith("E", StringComparison.OrdinalIgnoreCase))
            return OperationResult.Fail("Badge ID must start with E.");
        if (string.IsNullOrWhiteSpace(displayName))
            return OperationResult.Fail("Display name is required.");
        return OperationResult.Ok();
    }
}

public sealed class OperationResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = "";
    public EmployeeRecord? Employee { get; init; }

    public static OperationResult Ok(string message = "", EmployeeRecord? employee = null) =>
        new() { Success = true, Message = message, Employee = employee };

    public static OperationResult Fail(string message) =>
        new() { Success = false, Message = message };
}
