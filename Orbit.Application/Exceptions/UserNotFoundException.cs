using System.Runtime.Serialization;

namespace Orbit.Application.Exceptions;

/// <summary>
/// Exceção lançada quando um usuário não é encontrado.
/// </summary>
public class UserNotFoundException : Exception
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="UserNotFoundException"/>.
    /// </summary>
    public UserNotFoundException()
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="UserNotFoundException"/> com uma mensagem de erro especificada.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    public UserNotFoundException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="UserNotFoundException"/> com uma mensagem de erro especificada e uma referência à exceção interna que é a causa da exceção.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    /// <param name="innerException">A exceção que é a causa da exceção atual.</param>
    public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
