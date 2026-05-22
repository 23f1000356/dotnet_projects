using PracticeFA.App.Models;

namespace PracticeFA.App.Services;

/// <summary>P07 — module access from database (FA: CheckAuth).</summary>
public static class ModuleAuth
{
    public static bool CanAccess(int moduleId) =>
        AppState.AllowedModuleIds.Contains(moduleId);

    public static void RequireAccess(int moduleId)
    {
        if (!CanAccess(moduleId))
            throw new UnauthorizedAccessException($"Module {moduleId} is not allowed for this user.");
    }
}
