using Microsoft.AspNetCore.Mvc;

namespace Orbit.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
