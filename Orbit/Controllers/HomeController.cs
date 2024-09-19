using Microsoft.AspNetCore.Mvc;

namespace Orbit.Controllers;
public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return RedirectToActionPermanent("index", "account");
    }
}
