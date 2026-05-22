using System.Windows;
using PracticeFA.App.Pages;
using PracticeFA.App.Services;

namespace PracticeFA.App;

public partial class MainWindow : Window
{
    private string _currentHub = "Master";

    public MainWindow()
    {
        InitializeComponent();
        NavigateToMaster();

        if (AppState.CurrentUser is { } user)
        {
            Title = $"Practice FA — {user.DisplayName} ({user.PlantCode})";
            ToolbarStatusText.Text = $"Signed in: {user.UserId} · {user.DisplayName} · Plant {user.PlantCode}";
        }
        else
        {
            ToolbarStatusText.Text = "No session user (unexpected).";
        }
    }

    private void Master_Click(object sender, RoutedEventArgs e) => NavigateToMaster();

    private void Reports_Click(object sender, RoutedEventArgs e) => NavigateToReports();

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        AppState.Clear();
        Close();
    }

    private void About_Click(object sender, RoutedEventArgs e)
    {
        var userLine = AppState.CurrentUser is { } u
            ? $"{u.DisplayName} ({u.UserId}, {u.PlantCode})"
            : "Not signed in";

        MessageBox.Show(
            $"Practice FA learning shell\nP06: SQL login via dbo.spLogin\nP34: UserControl + Menu\n\nSession: {userLine}",
            "About Practice FA",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void ToolbarRefresh_Click(object sender, RoutedEventArgs e)
    {
        if (_currentHub == "Reports")
            NavigateToReports();
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
}
