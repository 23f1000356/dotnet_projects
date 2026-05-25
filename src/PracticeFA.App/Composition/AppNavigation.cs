using Microsoft.Extensions.DependencyInjection;

namespace PracticeFA.App.Composition;

/// <summary>P39 — resolve views from DI (no new ViewModel() in code-behind).</summary>
public static class AppNavigation
{
    public static TPage CreatePage<TPage>() where TPage : class =>
        App.Services.GetRequiredService<TPage>();
}
