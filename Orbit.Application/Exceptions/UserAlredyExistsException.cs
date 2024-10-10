namespace Orbit.Application.Exceptions;

/// <summary>
/// Exceção lançada quando um usuário com o mesmo identificador já existe.
/// </summary>
public class UserAlredyExistsException : Exception
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="UserAlredyExistsException"/>.
    /// </summary>
    public UserAlredyExistsException()
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="UserAlredyExistsException"/> com uma mensagem de erro especificada.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    public UserAlredyExistsException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="UserAlredyExistsException"/> com uma mensagem de erro especificada e uma referência à exceção interna que é a causa da exceção.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    /// <param name="innerException">A exceção que é a causa da exceção atual.</param>
    public UserAlredyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
