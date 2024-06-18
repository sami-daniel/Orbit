using Microsoft.AspNetCore.Mvc;

namespace Orbit.Controllers
{
    public class HomeController : Controller
    {
        //Route: {url}/Home/Index OR {url}/Home/
        public IActionResult Index()
        {
            return RedirectToActionPermanent("", "Account"); //~/Views/Home/Index.cshtml
        }
    }
}
