using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PracticeFA.App.Composition;
using PracticeFA.App.Services;
using PracticeFA.App.Services.Abstractions;
using PracticeFA.App.Views;

namespace PracticeFA.App;

public partial class App : Application
{
    private IHost? _host;

    /// <summary>P39 — composition root service provider.</summary>
    public static IServiceProvider Services { get; private set; } = null!;

    public static T GetRequiredService<T>() where T : notnull =>
        Services.GetRequiredService<T>();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddPracticeFaServices();
        _host = builder.Build();
        Services = _host.Services;

        Services.GetRequiredService<GlobalExceptionHandler>().Attach(this);

        var settings = Services.GetRequiredService<ISettingsService>();
        settings.Load();
        settings.ApplyTheme(Current, settings.Current.Theme);

        var splash = new SplashWindow();
        if (splash.ShowDialog() != true)
        {
            Shutdown();
            return;
        }

        var signIn = new SignInWindow();
        if (signIn.ShowDialog() != true)
        {
            Shutdown();
            return;
        }

        try
        {
            var main = new MainWindow();
            main.Closed += (_, _) =>
            {
                AppState.Clear();
                Shutdown();
            };
            MainWindow = main;
            main.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Main window failed to open:\n\n" + ex.Message,
                "Practice FA",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown();
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host?.Dispose();
        base.OnExit(e);
    }
}
