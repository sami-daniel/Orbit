namespace Orbit.Application.Exceptions;

/// <summary>
/// Exceção lançada quando uma imagem inválida é encontrada.
/// </summary>
public class InvalidImageException : Exception
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InvalidImageException"/>.
    /// </summary>
    public InvalidImageException()
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InvalidImageException"/> com uma mensagem de erro especificada.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    public InvalidImageException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InvalidImageException"/> com uma mensagem de erro especificada e uma referência à exceção interna que é a causa da exceção.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    /// <param name="innerException">A exceção que é a causa da exceção atual.</param>
    public InvalidImageException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
