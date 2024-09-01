using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Orbit.Hubs;

namespace Orbit.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Index(string user, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", user, message);
            return NoContent();
        }
    }
}
