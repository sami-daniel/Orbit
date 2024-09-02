using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Orbit.Hubs;
using Orbit.Infrastructure.Data.Contexts;

namespace Orbit.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var name = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var users = await _context.Users.Where(u => u.UserName == name).Include(u => u.Followers).ToListAsync();
            var user = users.FirstOrDefault();

            return View(user);
        }

        public async Task<IActionResult> Send(string user, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
            return NoContent();
        }
    }
}
