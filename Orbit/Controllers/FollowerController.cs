using Microsoft.AspNetCore.Mvc;
using Orbit.Application.Interfaces;
using Orbit.DTOs.Responses;
using Orbit.Extensions;

namespace Orbit.Controllers;
public class FollowerController : Controller
{
    private readonly IUserService _userService;

    public FollowerController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var username = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
        var users = await _userService.GetAllUserAsync(u => u.UserName == username, includeProperties: "Followers");
        var user = users.First();

        var followers = user!.Followers;

        return View(followers);
    }
}
