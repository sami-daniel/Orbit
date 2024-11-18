using Microsoft.AspNetCore.Mvc;
using Orbit.DTOs.Responses;
using Orbit.Extensions;
using Orbit.Services.Interfaces;

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
    public async Task<IActionResult> GetFollowers([FromQuery] string? userName)
    {
        string username = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
        if (userName != null)
        {
            username = userName;
        }
        var users = await _userService.GetAllUserAsync(u => u.UserName == username, includeProperties: "Followers");
        var user = users.First();

        var followers = user!.Followers;

        return View(followers);
    }

    [HttpGet("followed")]
    public async Task<IActionResult> GetFollowed([FromQuery] string? userName)
    {
        string username = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
        if (userName != null)
        {
            username = userName;
        }
        var users = await _userService.GetAllUserAsync(u => u.UserName == username, includeProperties: "Users");
        var user = users.First();

        var followedAccounts = user!.Users;

        return View(followedAccounts);
    }
}
