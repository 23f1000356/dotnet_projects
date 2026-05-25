namespace PracticeFA.App.Services;

/// <summary>P10 — results from splash database check (shown on main status bar).</summary>
public static class StartupState
{
    public static bool DatabaseOk { get; private set; }

    public static string DatabaseMessage { get; private set; } = "Not checked yet.";

    public static void SetDatabaseStatus(bool ok, string message)
    {
        DatabaseOk = ok;
        DatabaseMessage = message;
    }

    public static void Clear() => SetDatabaseStatus(false, "Not checked yet.");
}
