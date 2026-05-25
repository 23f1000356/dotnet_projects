using System.IO;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>P43 — daily log under %AppData%/PracticeFA/logs (P31-style, WPF app path).</summary>
public sealed class FileAppLogger : IAppLogger
{
    public FileAppLogger()
    {
        LogDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "PracticeFA",
            "logs");
        Directory.CreateDirectory(LogDirectory);
    }

    public string LogDirectory { get; }

    public string TodayLogPath =>
        Path.Combine(LogDirectory, $"practice-{DateTime.Now:yyyy-MM-dd}.txt");

    public void LogInfo(string message) => Append("INFO", message);

    public void LogError(Exception exception, string context) =>
        Append("ERROR", $"{context}{Environment.NewLine}{exception}");

    private void Append(string level, string message)
    {
        var line =
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {level} {message}{Environment.NewLine}";
        File.AppendAllText(TodayLogPath, line);
    }
}
