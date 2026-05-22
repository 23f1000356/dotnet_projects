using System.Windows;
using PracticeFA.App.Services;
using PracticeFA.App.Views;

namespace PracticeFA.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var signIn = new SignInWindow();
        if (signIn.ShowDialog() != true)
        {
            Shutdown();
            return;
        }

        var main = new MainWindow();
        main.Closed += (_, _) =>
        {
            AppState.Clear();
            Shutdown();
        };
        main.Show();
    }
}
