namespace PracticeFA.App.Services.Abstractions;

/// <summary>P43 — file logging for IT (full exception detail); operators see friendly UI only.</summary>
public interface IAppLogger
{
    string LogDirectory { get; }

    string TodayLogPath { get; }

    void LogInfo(string message);

    void LogError(Exception exception, string context);
}
