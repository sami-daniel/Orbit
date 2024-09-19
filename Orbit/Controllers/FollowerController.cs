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
        var user = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
        var users = await _userService.GetUserByIdentifierAsync(user);

        var followers = users!.Followers;

        return View(followers);
    }
}
