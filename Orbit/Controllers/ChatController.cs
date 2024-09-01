using Microsoft.AspNetCore.Mvc;
using Orbit.Infrastructure.Data.Contexts;

namespace Orbit.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
