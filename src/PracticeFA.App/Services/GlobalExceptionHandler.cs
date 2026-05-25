using System.Windows;
using System.Windows.Threading;
using Microsoft.Data.SqlClient;
using PracticeFA.App.Exceptions;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>
/// P43 — central handler for unhandled UI and background faults: log for IT, friendly MessageBox for operators.
/// </summary>
public sealed class GlobalExceptionHandler
{
    private readonly IAppLogger _logger;
    private Application? _app;

    public GlobalExceptionHandler(IAppLogger logger) => _logger = logger;

    public void Attach(Application app)
    {
        _app = app;
        app.DispatcherUnhandledException += OnDispatcherUnhandled;
        AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandled;
        TaskScheduler.UnobservedTaskException += OnUnobservedTask;
    }

    public void Handle(Exception exception, string source, bool showDialog = true)
    {
        var referenceId = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
        var user = AppState.CurrentUser;
        var context =
            $"[{referenceId}] Source={source} " +
            $"User={user?.UserId ?? "(none)"} Plant={user?.PlantCode ?? "(none)"}";

        _logger.LogError(exception, context);

        if (!showDialog)
            return;

        ShowUserMessage(exception, referenceId);
    }

    /// <summary>Learning/demo — throws on the UI thread to exercise the global handler.</summary>
    public void ThrowDemoValidation() =>
        throw new ValidationException("Demo: bag tag is required before save.");

    /// <summary>Learning/demo — throws an unexpected error to exercise logging + reference id.</summary>
    public void ThrowDemoUnhandled() =>
        throw new InvalidOperationException("Demo: simulated unhandled fault for P43.");

    private void OnDispatcherUnhandled(object sender, DispatcherUnhandledExceptionEventArgs args)
    {
        Handle(args.Exception, "DispatcherUnhandledException");
        args.Handled = true;
    }

    private void OnDomainUnhandled(object sender, UnhandledExceptionEventArgs args)
    {
        if (args.ExceptionObject is not Exception ex)
            return;

        _logger.LogError(ex, $"AppDomain.UnhandledException IsTerminating={args.IsTerminating}");

        if (_app?.Dispatcher.CheckAccess() == true)
            ShowUserMessage(ex, null);
        else
            _app?.Dispatcher.BeginInvoke(() => ShowUserMessage(ex, null));
    }

    private void OnUnobservedTask(object? sender, UnobservedTaskExceptionEventArgs args)
    {
        Handle(args.Exception, "TaskScheduler.UnobservedTaskException", showDialog: false);
        args.SetObserved();
    }

    private static void ShowUserMessage(Exception exception, string? referenceId)
    {
        referenceId ??= "LOGGED";

        string body;
        MessageBoxImage icon;

        if (exception is ValidationException)
        {
            body = exception.Message;
            icon = MessageBoxImage.Warning;
        }
        else if (exception is SqlException)
        {
            body =
                "A database error occurred. Please try again or contact IT support.\n\n" +
                $"Reference: {referenceId}";
            icon = MessageBoxImage.Error;
        }
        else
        {
            body =
                "An unexpected error occurred. Contact IT support.\n\n" +
                $"Reference: {referenceId}";
            icon = MessageBoxImage.Error;
        }

        MessageBox.Show(
            body,
            "Practice FA",
            MessageBoxButton.OK,
            icon);
    }
}
