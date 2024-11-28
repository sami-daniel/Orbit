using Microsoft.AspNetCore.Mvc;

namespace Orbit.Controllers;

public class HomeController : Controller
{
    // Define the route for the home page ("/" will map to this action).
    [Route("/")]
    public IActionResult Index()
    {
        // Redirect the user permanently to the "index" action in the "account" controller.
        return RedirectToActionPermanent("index", "account");
    }
}
