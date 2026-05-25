namespace PracticeFA.App.Exceptions;

/// <summary>
/// P31/P43 — business validation error; show <see cref="Exception.Message"/> to the operator only.
/// </summary>
public sealed class ValidationException : Exception
{
    public ValidationException(string message)
        : base(message)
    {
    }

    public ValidationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
