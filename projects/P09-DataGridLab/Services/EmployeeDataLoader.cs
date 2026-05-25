using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PracticeFA.P09.Services;

public sealed class LoadResult
{
    public required DataTable Table { get; init; }
    public required string SourceDescription { get; init; }
}

public static class EmployeeDataLoader
{
    public static LoadResult Load(bool activeOnly = true)
    {
        var sql = TryLoadFromSql(activeOnly);
        if (sql is not null)
            return sql;

        return new LoadResult
        {
            Table = SampleEmployeeData.CreateActiveEmployees(),
            SourceDescription = "In-memory sample (run 001+002+003 SQL scripts to load from PracticeFA DB)",
        };
    }

    private static LoadResult? TryLoadFromSql(bool activeOnly)
    {
        try
        {
            var connectionName = Environment.GetEnvironmentVariable("PRACTICE_FA_CONNECTION");
            if (string.IsNullOrWhiteSpace(connectionName))
            {
                var env = Environment.GetEnvironmentVariable("PRACTICE_FA_ENV");
                connectionName = string.Equals(env, "QA", StringComparison.OrdinalIgnoreCase)
                    ? "PracticeFA_QA"
                    : "PracticeFA";
            }

            var connectionString = ConfigurationManager.ConnectionStrings[connectionName.Trim()]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
                return null;

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("dbo.spGetEmployees", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.Add(new SqlParameter("@ActiveOnly", activeOnly));

            using var adapter = new SqlDataAdapter(command);
            var table = new DataTable();
            adapter.Fill(table);

            if (table.Rows.Count == 0)
                return null;

            return new LoadResult
            {
                Table = table,
                SourceDescription = activeOnly
                    ? "SQL — dbo.spGetEmployees (@ActiveOnly=1)"
                    : "SQL — dbo.spGetEmployees (@ActiveOnly=0)",
            };
        }
        catch
        {
            return null;
        }
    }
}
