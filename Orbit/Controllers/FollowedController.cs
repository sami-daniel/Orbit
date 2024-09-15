using Microsoft.AspNetCore.Mvc;
using Orbit.Application.Interfaces;
using Orbit.Domain.Entities;
using Orbit.DTOs.Responses;
using Orbit.Extensions;

namespace Orbit.Controllers;
public class FollowedController : Controller
{
    private readonly IUserService _userService;

    public FollowedController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var user = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
        var users = await _userService.GetUserByIdentifierAsync(user);

        var followed = users!.Users;
        return View(followed);
    }
}
