using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Orbit.Hubs;

/// <summary>
/// A SignalR Hub for managing chat messages with users. Requires authorization to access.
/// </summary>
[Authorize] // Ensures that the user must be authenticated to connect to the hub.
public class ChatHub : Hub
{
    /// <summary>
    /// Sends a chat message to a specific user.
    /// </summary>
    /// <param name="user">The username of the recipient.</param>
    /// <param name="message">The chat message to be sent to the user.</param>
    /// <returns>A task representing the asynchronous operation of sending the chat message.</returns>
    public async Task SendChatMessage(string user, string message)
    {
        // Sends the "ReceiveChatMessage" method to the specified user with the message and sender's username.
        await Clients.User(user).SendAsync("ReceiveChatMessage", user, message);
    }
}
