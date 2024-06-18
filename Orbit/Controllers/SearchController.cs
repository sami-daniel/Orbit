using Microsoft.AspNetCore.Mvc;

namespace Orbit.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
