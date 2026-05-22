namespace PracticeFA.P31.Exceptions;

/// <summary>
/// Business validation error — show Message to user; log full detail if needed.
/// </summary>
public sealed class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }

    public ValidationException(string message, Exception inner) : base(message, inner) { }
}
