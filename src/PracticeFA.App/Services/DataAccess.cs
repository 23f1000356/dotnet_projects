using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PracticeFA.App.Services;

/// <summary>
/// DAL — stored procedures only (FA: clsAccess.ExecSP / clsFA_DBC).
/// </summary>
public static class DataAccess
{
    public static DataTable ExecSp(string procName, params SqlParameter[] parameters)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["PracticeFA"]?.ConnectionString
            ?? throw new InvalidOperationException(
                "Missing connection string 'PracticeFA' in App.config.");

        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(procName, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        if (parameters.Length > 0)
            command.Parameters.AddRange(parameters);

        using var adapter = new SqlDataAdapter(command);
        var table = new DataTable();
        adapter.Fill(table);
        return table;
    }
}
