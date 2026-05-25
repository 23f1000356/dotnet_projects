using System.Data;
using Microsoft.Data.SqlClient;
using PracticeFA.App.Models;

namespace PracticeFA.App.Services;

/// <summary>P04 — style code + description saved via dbo.spSaveStyle (insert or update by code).</summary>
public static class StyleService
{
    public static DataTable GetStyles() =>
        DataAccess.ExecSp("dbo.spGetStyles");

    public static StyleSaveResult Save(string styleCode, string description, string? createdBy)
    {
        var validation = Validate(styleCode, description);
        if (!validation.Success)
            return validation;

        try
        {
            var table = DataAccess.ExecSp(
                "dbo.spSaveStyle",
                new SqlParameter("@StyleCode", styleCode.Trim()),
                new SqlParameter("@Description", description.Trim()),
                new SqlParameter("@CreatedBy", (object?)createdBy?.Trim() ?? DBNull.Value));

            if (table.Rows.Count == 0)
                return StyleSaveResult.Fail("Style was not saved.");

            var style = StyleMapper.FromRow(table.Rows[0]);
            var action = style.UpdatedUtc.HasValue ? "updated" : "saved";
            return StyleSaveResult.Ok($"Style {action}: {style.StyleCode}", style);
        }
        catch (SqlException)
        {
            return StyleSaveResult.Fail(
                "Database error while saving style.\n\nRun database/scripts/004_P04_Styles.sql on PracticeFA.");
        }
    }

    private static StyleSaveResult Validate(string styleCode, string description)
    {
        if (string.IsNullOrWhiteSpace(styleCode))
            return StyleSaveResult.Fail("Style code is required.");
        if (styleCode.Trim().Length < 3)
            return StyleSaveResult.Fail("Style code must be at least 3 characters.");
        if (string.IsNullOrWhiteSpace(description))
            return StyleSaveResult.Fail("Description is required.");
        if (description.Trim().Length > 500)
            return StyleSaveResult.Fail("Description must be 500 characters or less.");
        return StyleSaveResult.Ok();
    }
}

public sealed class StyleSaveResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = "";
    public StyleRecord? Style { get; init; }

    public static StyleSaveResult Ok(string message = "", StyleRecord? style = null) =>
        new() { Success = true, Message = message, Style = style };

    public static StyleSaveResult Fail(string message) =>
        new() { Success = false, Message = message };
}
