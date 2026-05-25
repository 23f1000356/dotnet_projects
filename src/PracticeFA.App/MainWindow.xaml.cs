using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using PracticeFA.App.Composition;
using PracticeFA.App.Pages;
using PracticeFA.App.Services;
using PracticeFA.App.Services.Abstractions;
using PracticeFA.App.Views;

namespace PracticeFA.App;

public partial class MainWindow : Window
{
    private string _currentHub = "Master";

    public MainWindow()
    {
        InitializeComponent();
        NavigateToMaster();
        ApplyFooterStatus();

        if (AppState.CurrentUser is { } user)
        {
            var settings = App.GetRequiredService<ISettingsService>();
            var dataMode = ServiceRegistration.UseMockDataAccess ? "Mock DB" : "SQL";
            Title = $"Practice FA — {user.DisplayName} ({user.PlantCode})";
            ToolbarStatusText.Text =
                $"Signed in: {user.UserId} · {user.DisplayName} · Login plant {user.PlantCode} · " +
                $"Prefs plant {settings.Current.PlantCode} · Theme {settings.Current.Theme} · P39 {dataMode} · {DbSettings.SummaryLine}";
        }
        else
        {
            ToolbarStatusText.Text = $"No session user (unexpected). · {DbSettings.SummaryLine}";
        }
    }

    private void Master_Click(object sender, RoutedEventArgs e) => NavigateToMaster();

    private void Reports_Click(object sender, RoutedEventArgs e) => NavigateToReports();

    private void OutputChart_Click(object sender, RoutedEventArgs e) => NavigateToOutputChart();

    private void Attendance_Click(object sender, RoutedEventArgs e) => NavigateToAttendance();

    private void Settings_Click(object sender, RoutedEventArgs e) => NavigateToSettings();

    private void Orders_Click(object sender, RoutedEventArgs e) => NavigateToOrders();

    private void OrdersLegacy_Click(object sender, RoutedEventArgs e) => NavigateToOrdersLegacy();

    private void OrderWizard_Click(object sender, RoutedEventArgs e) => NavigateToOrderWizard();

    private void AsyncDemo_Click(object sender, RoutedEventArgs e) => NavigateToAsyncDemo();

    private void ErpStock_Click(object sender, RoutedEventArgs e) => NavigateToErpStock();

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        AppState.Clear();
        Close();
    }

    private void ConnectionInfo_Click(object sender, RoutedEventArgs e)
    {
        DbSettings.Reload();
        var (success, testMessage) = DbSettings.TryTestConnection();
        MessageBox.Show(
            $"{DbSettings.SummaryLine}\n\nActive config: {DbSettings.ActiveConnectionName}\n\n" +
            $"Test: {testMessage}\n\n" +
            "How to switch without rebuild:\n" +
            "  • Set user env PRACTICE_FA_ENV=QA\n" +
            "  • Or PRACTICE_FA_CONNECTION=PracticeFA_QA\n" +
            "  • Or edit src/PracticeFA.App/App.config\n\n" +
            "See database/P24-README.md",
            "P24 — Database connection",
            MessageBoxButton.OK,
            success ? MessageBoxImage.Information : MessageBoxImage.Warning);
    }

    private void About_Click(object sender, RoutedEventArgs e)
    {
        var userLine = AppState.CurrentUser is { } u
            ? $"{u.DisplayName} ({u.UserId}, {u.PlantCode})"
            : "Not signed in";

        var logger = App.GetRequiredService<IAppLogger>();

        MessageBox.Show(
            $"Practice FA learning shell\n" +
            $"P06: SQL login via dbo.spLogin\n" +
            $"P24: {DbSettings.SummaryLine}\n" +
            $"P34: UserControl + Menu\n" +
            $"P43: Global errors → log + friendly message (no stack trace to operators)\n\n" +
            $"Session: {userLine}\n" +
            $"Error log: {logger.TodayLogPath}\n\n" +
            "Switch DB: PRACTICE_FA_ENV=QA or edit App.config connectionStrings.",
            "About Practice FA",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void SimulateValidationError_Click(object sender, RoutedEventArgs e) =>
        App.GetRequiredService<GlobalExceptionHandler>().ThrowDemoValidation();

    private void SimulateUnhandledError_Click(object sender, RoutedEventArgs e) =>
        App.GetRequiredService<GlobalExceptionHandler>().ThrowDemoUnhandled();

    private void OpenErrorLog_Click(object sender, RoutedEventArgs e)
    {
        var logger = App.GetRequiredService<IAppLogger>();
        Directory.CreateDirectory(logger.LogDirectory);
        Process.Start(new ProcessStartInfo
        {
            FileName = logger.LogDirectory,
            UseShellExecute = true,
        });
    }

    private void ToolbarRefresh_Click(object sender, RoutedEventArgs e)
    {
        if (_currentHub == "ErpStock")
            NavigateToErpStock();
        else if (_currentHub == "AsyncDemo")
            NavigateToAsyncDemo();
        else if (_currentHub == "OrderWizard")
            NavigateToOrderWizard();
        else if (_currentHub == "OrdersLegacy")
            NavigateToOrdersLegacy();
        else if (_currentHub == "Orders")
            NavigateToOrders();
        else if (_currentHub == "Settings")
            NavigateToSettings();
        else if (_currentHub == "Attendance")
            NavigateToAttendance();
        else if (_currentHub == "Reports")
            NavigateToReports();
        else if (_currentHub == "OutputChart")
            NavigateToOutputChart();
        else
            NavigateToMaster();

        ToolbarStatusText.Text = $"Refreshed {_currentHub} at {DateTime.Now:HH:mm:ss}";
    }

    private void ToolbarSave_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(
            "Toolbar Save is a placeholder.\nP08 will call stored procedures from the active screen.",
            "Save",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
        ToolbarStatusText.Text = "Save clicked (no database yet).";
    }

    private void NavigateToMaster()
    {
        _currentHub = "Master";
        MainFrame.Navigate(new MasterPage());
        NavHintText.Text = "Current: Master (product data hub)";
    }

    private void NavigateToReports()
    {
        _currentHub = "Reports";
        MainFrame.Navigate(new ReportsPage());
        NavHintText.Text = "Current: Reports (MIS hub)";
    }

    private void NavigateToOutputChart()
    {
        _currentHub = "OutputChart";
        MainFrame.Navigate(AppNavigation.CreatePage<OutputChartView>());
        NavHintText.Text = "Current: Hourly output chart (P21 LiveCharts)";
    }

    private void NavigateToAttendance()
    {
        _currentHub = "Attendance";
        MainFrame.Navigate(AppNavigation.CreatePage<AttendanceView>());
        NavHintText.Text = "Current: Attendance (P05 MVVM · P39 DI)";
    }

    private void NavigateToSettings()
    {
        _currentHub = "Settings";
        MainFrame.Navigate(AppNavigation.CreatePage<SettingsView>());
        NavHintText.Text = "Current: Settings (P05b JSON · P39 DI)";
    }

    private void NavigateToOrders()
    {
        _currentHub = "Orders";
        MainFrame.Navigate(AppNavigation.CreatePage<OrdersView>());
        NavHintText.Text = "Current: Orders (P40 master-detail)";
    }

    private void NavigateToOrdersLegacy()
    {
        _currentHub = "OrdersLegacy";
        MainFrame.Navigate(AppNavigation.CreatePage<OrdersLegacyView>());
        NavHintText.Text = "Current: Orders legacy (P44 code-behind · DataTable)";
    }

    private void NavigateToOrderWizard()
    {
        _currentHub = "OrderWizard";
        MainFrame.Navigate(AppNavigation.CreatePage<OrderWizardView>());
        NavHintText.Text = "Current: Order wizard (P42 tabs)";
    }

    private void NavigateToAsyncDemo()
    {
        _currentHub = "AsyncDemo";
        MainFrame.Navigate(AppNavigation.CreatePage<AsyncDemoView>());
        NavHintText.Text = "Current: Async demo (P20 busy overlay)";
    }

    private void NavigateToErpStock()
    {
        _currentHub = "ErpStock";
        MainFrame.Navigate(AppNavigation.CreatePage<ErpStockView>());
        NavHintText.Text = "Current: SAP stock check (P45 IErpService · cancel)";
    }

    private void ApplyFooterStatus()
    {
        var ok = StartupState.DatabaseOk;
        var okBrush = new SolidColorBrush(Color.FromRgb(0x2E, 0x7D, 0x32));
        var failBrush = new SolidColorBrush(Color.FromRgb(0xB7, 0x1C, 0x1C));

        SqlIndicator.Fill = ok ? okBrush : failBrush;
        SqlStatusText.Text = ok ? "SQL: OK" : "SQL: Offline";
        SqlStatusText.Foreground = ok ? okBrush : failBrush;

        FooterDbText.Text = StartupState.DatabaseMessage;
        FooterUserText.Text = AppState.CurrentUser is { } user
            ? $"{user.UserId} · {user.DisplayName} · Plant {user.PlantCode} · P10"
            : "Not signed in";
    }
}
