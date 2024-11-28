namespace Orbit.Services.Exceptions;

/// <summary>
/// Exception thrown when a post is not found.
/// </summary>
public class PostNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public PostNotFoundException(string? message) : base(message)
    {
    }
}