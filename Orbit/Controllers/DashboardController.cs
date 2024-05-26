using Microsoft.AspNetCore.Mvc;
using Orbit.Application.Dtos.Responses;
using Orbit.Extensions;

namespace Orbit.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var user = HttpContext.Session.GetObject<UserReponse>("User");

            if (user == null) 
            {
                return RedirectToAction("", "Account");
            }

            return View(user);
        }
    }
}
