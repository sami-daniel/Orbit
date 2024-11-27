using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.DTOs.Responses;
using Orbit.Extensions;

namespace Orbit.Controllers;

[Authorize]
public class PanelController : Controller
{
    public IActionResult Index()
    {
        var user = HttpContext.Session.GetObject<UserResponse>("User");
        return View(user);
    }
}
