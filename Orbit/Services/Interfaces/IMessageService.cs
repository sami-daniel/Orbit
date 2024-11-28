using Orbit.Models;

namespace Orbit.Services.Interfaces;

public interface IMessageService
{
    /// <summary>
    /// Retrieves all messages between two users.
    /// </summary>
    /// <param name="user">The username of the first participant in the conversation.</param>
    /// <param name="with">The username of the other participant in the conversation.</param>
    /// <returns>A task representing the asynchronous operation. The result contains a list of messages exchanged between the two users.</returns>
    Task<IEnumerable<Message>> GetAllMessagesAsync(string user, string with);

    /// <summary>
    /// Adds a new message to the system.
    /// </summary>
    /// <param name="message">The message to be added.</param>
    /// <param name="from">The username of the message sender.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddMessageAsync(Message message, string from);
}
