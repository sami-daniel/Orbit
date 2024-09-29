using Microsoft.AspNetCore.Mvc;
using Orbit.Application.Interfaces;
using Orbit.DTOs.Responses;
using Orbit.Extensions;

namespace Orbit.Controllers;

[Route("[controller]")]
public class InteractionController : Controller
{
    private readonly IUserService _userService;

    public InteractionController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("followers")]
    public async Task<IActionResult> GetFollowers()
    {
        var username = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
        var users = await _userService.GetAllUserAsync(u => u.UserName == username, includeProperties: "Followers");
        var user = users.First();

        var followers = user!.Followers;

        return View(followers);
    }

    [HttpGet("followed")]
    public async Task<IActionResult> GetFollowed()
    {
        var username = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
        var users = await _userService.GetAllUserAsync(u => u.UserName == username, includeProperties: "Users");
        var user = users.First();

        var followeds = user!.Users;

        return View(followeds);
    }
}
