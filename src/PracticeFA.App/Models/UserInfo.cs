namespace PracticeFA.App.Models;

/// <summary>
/// Session user — column names match dbo.spLogin result (FA: m_UserInfo).
/// </summary>
public sealed class UserInfo
{
    public string UserId { get; init; } = "";
    public string DisplayName { get; init; } = "";
    public string PlantCode { get; init; } = "";
}
