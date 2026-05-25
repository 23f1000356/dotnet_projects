using System.Configuration;
using Microsoft.Data.SqlClient;

namespace PracticeFA.App.Services;

/// <summary>
/// P24 — single place for SQL connection (FA: Profile.dll / dbConnect INI per environment).
/// Switch via App.config connection name or environment variable without recompiling.
/// </summary>
public static class DbSettings
{
    public const string LocalConnectionName = "PracticeFA";
    public const string QaConnectionName = "PracticeFA_QA";

    private const string EnvVarEnvironment = "PRACTICE_FA_ENV";
    private const string EnvVarConnectionName = "PRACTICE_FA_CONNECTION";

    public static string ActiveConnectionName { get; private set; } = LocalConnectionName;

    public static string EnvironmentLabel { get; private set; } = "Local (dev)";

    public static string ConnectionString { get; private set; } = "";

    public static string ServerDisplay { get; private set; } = "";

    public static string DatabaseDisplay { get; private set; } = "";

    static DbSettings() => Reload();

    /// <summary>Re-read config and environment (call after changing App.config at runtime is rare).</summary>
    public static void Reload()
    {
        ActiveConnectionName = ResolveConnectionName();
        EnvironmentLabel = ActiveConnectionName == QaConnectionName ? "QA" : "Local (dev)";

        var entry = ConfigurationManager.ConnectionStrings[ActiveConnectionName];
        ConnectionString = entry?.ConnectionString
            ?? throw new InvalidOperationException(
                $"Missing connection string '{ActiveConnectionName}' in App.config.");

        var sDisplay = string.Empty;
        var dDisplay = string.Empty;
        ParseConnectionParts(ConnectionString, out sDisplay, out dDisplay);
        ServerDisplay = sDisplay;
        DatabaseDisplay = dDisplay;
    }

    public static string SummaryLine =>
        $"DB [{EnvironmentLabel}] {ActiveConnectionName} → {ServerDisplay} / {DatabaseDisplay}";

    /// <summary>
    /// Optional pre-login check (Sign-in Test connection button).
    /// </summary>
    public static (bool Success, string Message) TryTestConnection()
    {
        try
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return (true, $"Connected to {DatabaseDisplay} on {ServerDisplay}.");
        }
        catch (Exception ex)
        {
            return (false, $"Connection failed ({ActiveConnectionName}):\n{ex.Message}");
        }
    }

    private static string ResolveConnectionName()
    {
        var explicitName = Environment.GetEnvironmentVariable(EnvVarConnectionName);
        if (!string.IsNullOrWhiteSpace(explicitName))
            return explicitName.Trim();

        var env = Environment.GetEnvironmentVariable(EnvVarEnvironment);
        if (string.Equals(env, "QA", StringComparison.OrdinalIgnoreCase))
            return QaConnectionName;

        return LocalConnectionName;
    }

    private static void ParseConnectionParts(string connectionString, out string server, out string database)
    {
        server = "(unknown)";
        database = "(unknown)";

        try
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            server = string.IsNullOrWhiteSpace(builder.DataSource) ? server : builder.DataSource;
            database = string.IsNullOrWhiteSpace(builder.InitialCatalog) ? database : builder.InitialCatalog;
        }
        catch
        {
            // Fallback for display only
            foreach (var part in connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                var kv = part.Split('=', 2, StringSplitOptions.TrimEntries);
                if (kv.Length != 2)
                    continue;

                if (kv[0].Equals("Server", StringComparison.OrdinalIgnoreCase) ||
                    kv[0].Equals("Data Source", StringComparison.OrdinalIgnoreCase))
                    server = kv[1];

                if (kv[0].Equals("Database", StringComparison.OrdinalIgnoreCase) ||
                    kv[0].Equals("Initial Catalog", StringComparison.OrdinalIgnoreCase))
                    database = kv[1];
            }
        }
    }
}
