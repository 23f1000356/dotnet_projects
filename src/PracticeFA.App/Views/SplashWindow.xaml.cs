using System.Windows;
using System.Windows.Media;
using PracticeFA.App.Services;

namespace PracticeFA.App.Views;

/// <summary>P10 — FA-style splash: short delay + database ping before sign-in.</summary>
public partial class SplashWindow : Window
{
    private static readonly SolidColorBrush OkBrush = new(Color.FromRgb(0x2E, 0x7D, 0x32));
    private static readonly SolidColorBrush FailBrush = new(Color.FromRgb(0xB7, 0x1C, 0x1C));

    public SplashWindow()
    {
        InitializeComponent();
        DbDetailText.Text = DbSettings.SummaryLine;
        Loaded += SplashWindow_Loaded;
    }

    private async void SplashWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            var minimumDisplay = Task.Delay(1500);
            var databaseCheck = Task.Run(CheckDatabaseAsync);
            await Task.WhenAll(minimumDisplay, databaseCheck).ConfigureAwait(true);

            var (success, message) = await databaseCheck.ConfigureAwait(true);
            StartupState.SetDatabaseStatus(success, message);
            ApplyStatus(success, message);

            await Task.Delay(success ? 400 : 1200).ConfigureAwait(true);
            DialogResult = true;
            Close();
        }
        catch
        {
            DialogResult = false;
            Close();
        }
    }

    private static (bool Success, string Message) CheckDatabaseAsync()
    {
        var (success, message) = DbSettings.TryTestConnection();
        if (!success)
            return (false, message);

        try
        {
            using var connection = new Microsoft.Data.SqlClient.SqlConnection(DbSettings.ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "dbo.spPing";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            using var reader = command.ExecuteReader();
            if (reader.Read())
                return (true, $"DB OK — spPing returned Ok={reader["Ok"]}.");

            return (true, "DB OK — connected (spPing returned no row).");
        }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 2812)
        {
            return (true, "DB OK — connected. Run 011_P10_Ping.sql for dbo.spPing.");
        }
        catch (Exception ex)
        {
            return (true, $"DB OK — connected. spPing skipped: {ex.Message}");
        }
    }

    private void ApplyStatus(bool success, string message)
    {
        StartupProgress.IsIndeterminate = false;
        StartupProgress.Value = 100;

        DbIndicator.Fill = success ? OkBrush : FailBrush;
        DbStatusText.Text = success ? "SQL: OK" : "SQL: Failed";
        DbStatusText.Foreground = success ? OkBrush : FailBrush;
        DbDetailText.Text = message;
    }
}
