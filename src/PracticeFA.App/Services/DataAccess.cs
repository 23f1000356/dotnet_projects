using System.Data;
using Microsoft.Data.SqlClient;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>
/// DAL facade — delegates to IDataAccess from composition root (P39).
/// Legacy static callers (EmployeeService, LoginService, …) keep working.
/// </summary>
public static class DataAccess
{
    public static DataTable ExecSp(string procName, params SqlParameter[] parameters) =>
        App.GetRequiredService<IDataAccess>().ExecSp(procName, parameters);
}
