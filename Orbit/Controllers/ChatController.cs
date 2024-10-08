using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Orbit.Application.Interfaces;
using Orbit.Hubs;
using Orbit.Models;

namespace Orbit.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IUserService userService, IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
            _userService = userService;
        }

        [Route("[controller]/{participantname?}")]
        public async Task<IActionResult> Index(string? participantname)
        {

            var hostname = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value!;

            var hosts = await _userService.GetAllUserAsync(u => u.UserName == hostname, includeProperties: "Followers,Users");
            var host = hosts.First();

            if (participantname == null)
            {
                return View(new ChatContext
                {
                    Host = host
                });
            }

            var participants = await _userService.GetAllUserAsync(u => u.UserName == participantname &&
                                                                      u.Followers.Contains(host) ||
                                                                      u.Users.Contains(host));

            if (!participants.Any())
            {
                return NotFound();
            }

            return View(new ChatContext
            {
                Host = host,
                Guest = participants.First()
            });
        }

    }
}
