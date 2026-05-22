using System.Windows;
using PracticeFA.App.Services;

namespace PracticeFA.App.Views;

public partial class SignInWindow : Window
{
    public SignInWindow() => InitializeComponent();

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
