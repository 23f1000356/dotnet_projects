namespace PracticeFA.P01.Models;

public sealed class UserSession
{
    public required string UserId { get; init; }
    public required string DisplayName { get; init; }
    public string PlantCode { get; init; } = "1001";
}
