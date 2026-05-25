using System.Data;
using Microsoft.Data.SqlClient;

namespace PracticeFA.App.Services.Abstractions;

/// <summary>P39 — DAL abstraction for tests (swap MockDataAccess in composition root).</summary>
public interface IDataAccess
{
    DataTable ExecSp(string procName, params SqlParameter[] parameters);
}
