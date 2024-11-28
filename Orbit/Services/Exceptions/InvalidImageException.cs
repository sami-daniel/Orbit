namespace Orbit.Services.Exceptions;

/// <summary>
/// Exception thrown when an invalid image is encountered.
/// </summary>
public class InvalidImageException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidImageException"/> class.
    /// </summary>
    public InvalidImageException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidImageException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidImageException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidImageException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public InvalidImageException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
