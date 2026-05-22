using PracticeFA.App.Models;

namespace PracticeFA.App.Services;

/// <summary>Application session after P06 login (FA: global user / m_UserInfo).</summary>
public static class AppState
{
    public static UserInfo? CurrentUser { get; set; }

    /// <summary>P07 — allowed module ids from spGetUserModules (cached per login).</summary>
    public static HashSet<int> AllowedModuleIds { get; private set; } = new();

    /// <summary>Debug list for UI: "1001 Style Creation, 2001 Bagging Entry, ..."</summary>
    public static string ModuleListDisplay { get; private set; } = "";

    public static void SetAllowedModules(IEnumerable<int> moduleIds, string displayList)
    {
        AllowedModuleIds = moduleIds.ToHashSet();
        ModuleListDisplay = displayList;
    }

    public static void Clear()
    {
        CurrentUser = null;
        AllowedModuleIds = new HashSet<int>();
        ModuleListDisplay = "";
    }

    public static bool IsSignedIn => CurrentUser is not null;
}
