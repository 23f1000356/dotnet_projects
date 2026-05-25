using System.Data;
using Microsoft.Data.SqlClient;
using PracticeFA.App.Models;

namespace PracticeFA.App.Services;

/// <summary>P08 CRUD · P35 search · P41 audit columns + soft delete with session user id.</summary>
public static class EmployeeService
{
    public static string? CurrentUserId => AppState.CurrentUser?.UserId;

    public static DataTable GetEmployees(bool activeOnly = true) =>
        DataAccess.ExecSp(
            "dbo.spGetEmployees",
            new SqlParameter("@ActiveOnly", activeOnly));

    public static DataTable Search(string? badgeFragment, string? processCenter, bool activeOnly)
    {
        try
        {
            return DataAccess.ExecSp(
                "dbo.spSearchEmployees",
                new SqlParameter("@BadgeFragment", ToDbParam(badgeFragment)),
                new SqlParameter("@ProcessCenter", ToDbParam(processCenter)),
                new SqlParameter("@ActiveOnly", activeOnly));
        }
        catch (SqlException ex) when (ex.Number == 2812)
        {
            throw new InvalidOperationException(
                "dbo.spSearchEmployees not found. Run database/scripts/005_P35_SearchEmployees.sql",
                ex);
        }
    }

    private static object ToDbParam(string? value) =>
        string.IsNullOrWhiteSpace(value) ? DBNull.Value : value.Trim();

    public static EmployeeRecord? GetByBadge(string badgeId)
    {
        var table = DataAccess.ExecSp(
            "dbo.spGetEmployeeByBadge",
            new SqlParameter("@BadgeId", badgeId.Trim()));

        return table.Rows.Count == 0 ? null : EmployeeMapper.FromRow(table.Rows[0]);
    }

    public static OperationResult Insert(string badgeId, string displayName, string? processCenter, string? createdBy = null)
    {
        var validation = Validate(badgeId, displayName);
        if (!validation.Success)
            return validation;

        var userId = createdBy ?? CurrentUserId;

        try
        {
            var table = DataAccess.ExecSp(
                "dbo.spInsEmployee",
                new SqlParameter("@BadgeId", badgeId.Trim()),
                new SqlParameter("@DisplayName", displayName.Trim()),
                new SqlParameter("@ProcessCenter", (object?)processCenter?.Trim() ?? DBNull.Value),
                new SqlParameter("@CreatedBy", (object?)userId?.Trim() ?? DBNull.Value));

            var employee = EmployeeMapper.FromRow(table.Rows[0]);
            return OperationResult.Ok(
                $"Employee added. Created by {employee.CreatedBy ?? "(unknown)"}.",
                employee);
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            return OperationResult.Fail("Badge ID already exists.");
        }
        catch (SqlException ex) when (ex.Number == 2812)
        {
            return OperationResult.Fail(
                "Employee procedures need P41 audit columns. Run database/scripts/007_P41_EmployeeAudit.sql");
        }
        catch (SqlException)
        {
            return OperationResult.Fail("Database error while adding employee.");
        }
    }

    public static OperationResult Update(EmployeeRecord employee, string? modifiedBy = null)
    {
        var validation = Validate(employee.BadgeId, employee.DisplayName);
        if (!validation.Success)
            return validation;

        var userId = modifiedBy ?? CurrentUserId;

        try
        {
            var table = DataAccess.ExecSp(
                "dbo.spUpdEmployee",
                new SqlParameter("@EmployeeId", employee.EmployeeId),
                new SqlParameter("@BadgeId", employee.BadgeId.Trim()),
                new SqlParameter("@DisplayName", employee.DisplayName.Trim()),
                new SqlParameter("@ProcessCenter", (object?)employee.ProcessCenter?.Trim() ?? DBNull.Value),
                new SqlParameter("@IsActive", employee.IsActive),
                new SqlParameter("@ModifiedBy", (object?)userId?.Trim() ?? DBNull.Value));

            if (table.Rows.Count == 0)
                return OperationResult.Fail("Employee not found.");

            var updated = EmployeeMapper.FromRow(table.Rows[0]);
            return OperationResult.Ok(
                $"Employee updated. Last modified by {updated.ModifiedBy ?? "(unknown)"}.",
                updated);
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            return OperationResult.Fail("Badge ID already used by another employee.");
        }
        catch (SqlException ex) when (ex.Number == 2812)
        {
            return OperationResult.Fail(
                "Employee procedures need P41 audit columns. Run database/scripts/007_P41_EmployeeAudit.sql");
        }
        catch (SqlException)
        {
            return OperationResult.Fail("Database error while updating employee.");
        }
    }

    public static OperationResult SoftDelete(int employeeId, string? modifiedBy = null)
    {
        var userId = modifiedBy ?? CurrentUserId;

        try
        {
            DataAccess.ExecSp(
                "dbo.spDelEmployee",
                new SqlParameter("@EmployeeId", employeeId),
                new SqlParameter("@ModifiedBy", (object?)userId?.Trim() ?? DBNull.Value));

            return OperationResult.Ok(
                $"Employee deactivated (soft delete). Hidden from active lists; row remains in database. Modified by {userId ?? "(unknown)"}.");
        }
        catch (SqlException ex) when (ex.Number == 2812)
        {
            return OperationResult.Fail(
                "dbo.spDelEmployee needs P41 update. Run database/scripts/007_P41_EmployeeAudit.sql");
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
