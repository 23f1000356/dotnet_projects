using Microsoft.Extensions.DependencyInjection;
using PracticeFA.App.Services;
using PracticeFA.App.Services.Abstractions;
using PracticeFA.App.ViewModels;
using PracticeFA.App.Views;

namespace PracticeFA.App.Composition;

/// <summary>P39 — single place to register services and view models.</summary>
public static class ServiceRegistration
{
    public static bool UseMockDataAccess =>
        string.Equals(
            Environment.GetEnvironmentVariable("PRACTICE_FA_MOCK_DB"),
            "1",
            StringComparison.OrdinalIgnoreCase);

    public static void AddPracticeFaServices(this IServiceCollection services)
    {
        if (UseMockDataAccess)
            services.AddSingleton<IDataAccess, MockDataAccess>();
        else
            services.AddSingleton<IDataAccess, SqlDataAccess>();

        services.AddSingleton<IAppLogger, FileAppLogger>();
        services.AddSingleton<GlobalExceptionHandler>();

        services.AddSingleton<ISettingsService, JsonSettingsService>();
        services.AddSingleton<IAttendanceService, AttendanceServiceImpl>();
        services.AddSingleton<IOrderService, OrderServiceImpl>();
        services.AddSingleton<IAsyncDemoService, AsyncDemoService>();
        services.AddSingleton<IErpService, MockErpService>();
        services.AddSingleton<IOutputChartService, MockOutputChartService>();

        services.AddTransient<AttendanceViewModel>();
        services.AddTransient<OutputChartViewModel>();
        services.AddTransient<AsyncDemoViewModel>();
        services.AddTransient<ErpStockViewModel>();
        services.AddTransient<OrdersViewModel>();
        services.AddTransient<OrderWizardViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<AttendanceView>();
        services.AddTransient<SettingsView>();
        services.AddTransient<OrdersView>();
        services.AddTransient<OrdersLegacyView>();
        services.AddTransient<AsyncDemoView>();
        services.AddTransient<ErpStockView>();
        services.AddTransient<OutputChartView>();
        services.AddTransient<OrderWizardView>();
    }
}
