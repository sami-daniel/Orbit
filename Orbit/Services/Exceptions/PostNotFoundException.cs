namespace Orbit.Services.Exceptions;

<<<<<<< HEAD
/// <summary>
/// Exception thrown when a post is not found.
/// </summary>
public class PostNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
=======
public class PostNotFoundException : Exception
{
>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
    public PostNotFoundException(string? message) : base(message)
    {
    }
}