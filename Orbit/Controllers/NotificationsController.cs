using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Orbit.Hubs;

namespace Orbit.Controllers;

// Ensures only authenticated users can access this controller
[Authorize]
public class NotificationsController : Controller
{
    private readonly IHubContext<NotificationHub> _hubContext;  // Hub context for sending notifications

    // Constructor to inject the SignalR hub context
    public NotificationsController(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    // POST: /Notifications/Index - Sends a notification to a specific user
    [HttpPost]
    public async Task<IActionResult> Index(string user, string message)
    {
        // Sends the notification to the specified user via SignalR
        await _hubContext.Clients.User(user).SendAsync("ReceiveNotification", user, message);

        // Returns a NoContent response (HTTP 204) indicating the operation was successful but no content is returned
        return NoContent();
    }
}
