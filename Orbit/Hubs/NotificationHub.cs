using Microsoft.AspNetCore.SignalR;

namespace Orbit.Hubs;

/// <summary>
/// A SignalR Hub for sending notifications to specific users.
/// </summary>
public class NotificationHub : Hub
{
    /// <summary>
    /// Sends a notification to a specific user.
    /// </summary>
    /// <param name="user">The username of the recipient.</param>
    /// <param name="message">The message to be sent to the user.</param>
    /// <returns>A task representing the asynchronous operation of sending the notification.</returns>
    public async Task SendNotification(string user, string message)
    {
        // Sends the "ReceiveNotification" message to the specified user with the user and message as parameters.
        await Clients.User(user).SendAsync("ReceiveNotification", user, message);
    }
}
