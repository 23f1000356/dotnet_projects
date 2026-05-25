namespace PracticeFA.App.Models;

public sealed class OrderSaveResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = "";
    public int? OrderId { get; init; }

    public static OrderSaveResult Ok(string message = "", int? orderId = null) =>
        new() { Success = true, Message = message, OrderId = orderId };

    public static OrderSaveResult Fail(string message) =>
        new() { Success = false, Message = message };
}
