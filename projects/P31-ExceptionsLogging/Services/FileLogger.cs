namespace PracticeFA.P31.Services;

public sealed class FileLogger
{
    private readonly string _logDirectory;

    public FileLogger(string? baseDirectory = null)
    {
        _logDirectory = Path.Combine(baseDirectory ?? AppContext.BaseDirectory, "logs");
        Directory.CreateDirectory(_logDirectory);
    }

    public string TodayLogPath =>
        Path.Combine(_logDirectory, $"practice-{DateTime.Now:yyyy-MM-dd}.txt");

    public void LogInfo(string message) => Append("INFO", message);

    public void LogError(Exception ex, string context) =>
        Append("ERROR", $"{context}{Environment.NewLine}{ex}");

    private void Append(string level, string message)
    {
        var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {level} {message}{Environment.NewLine}";
        File.AppendAllText(TodayLogPath, line);
    }
}
