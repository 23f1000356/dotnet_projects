using PracticeFA.App.Models;

namespace PracticeFA.App.Services;

/// <summary>Application session after P06 login (FA: global user / m_UserInfo).</summary>
public static class AppState
{
    public static UserInfo? CurrentUser { get; set; }

    public static void Clear() => CurrentUser = null;

    public static bool IsSignedIn => CurrentUser is not null;
}
