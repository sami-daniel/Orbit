using Microsoft.AspNetCore.Mvc;

namespace Orbit.Controllers
{
    public class PanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
