using System.Data;
using Microsoft.Data.SqlClient;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>P39 — production IDataAccess (stored procedures via SQL Client).</summary>
public sealed class SqlDataAccess : IDataAccess
{
    public DataTable ExecSp(string procName, params SqlParameter[] parameters)
    {
        var connectionString = DbSettings.ConnectionString;

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
