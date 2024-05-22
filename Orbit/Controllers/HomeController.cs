using Microsoft.AspNetCore.Mvc;

namespace Orbit.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
