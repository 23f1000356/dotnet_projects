using System.Windows;
using PracticeFA.App.Services;

namespace PracticeFA.App.Views;

public partial class SignInWindow : Window
{
    public SignInWindow()
    {
        InitializeComponent();
        RefreshDbTargetDisplay();
    }

    private void RefreshDbTargetDisplay() =>
        DbTargetText.Text = DbSettings.SummaryLine +
            "\n\nSwitch: set PRACTICE_FA_ENV=QA or edit App.config (see database/P24-README.md).";

    private void TestConnection_Click(object sender, RoutedEventArgs e)
    {
        DbSettings.Reload();
        RefreshDbTargetDisplay();

        var (success, message) = DbSettings.TryTestConnection();
        MessageBox.Show(
            message,
            success ? "Database connection" : "Database connection failed",
            MessageBoxButton.OK,
            success ? MessageBoxImage.Information : MessageBoxImage.Warning);
    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        ErrorText.Text = "";

        var result = LoginService.TryLogin(UserIdBox.Text, PasswordBox.Password);
        if (!result.Success)
        {
            ErrorText.Text = result.Message;
            return;
        }

        AppState.CurrentUser = result.User;
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
