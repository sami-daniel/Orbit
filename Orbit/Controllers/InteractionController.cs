using Microsoft.AspNetCore.Mvc;
using Orbit.DTOs.Responses;
using Orbit.Extensions;
using Orbit.Services.Interfaces;

namespace Orbit.Controllers;

[Route("[controller]")] // Define the base route for this controller (interaction routes)
public class InteractionController : Controller
{
    // Declare a dependency on the IUserService to handle user-related operations.
    private readonly IUserService _userService;

    // Constructor to inject the IUserService dependency.
    public InteractionController(IUserService userService)
    {
        _userService = userService;
    }

    // HTTP GET action to get the list of followers of a user.
    [HttpGet("followers")]
    public async Task<IActionResult> GetFollowers([FromQuery] string? userName)
    {
        // Retrieve the currently authenticated user's username from session.
        string username = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;

        // If a username is provided as a query parameter, override the session username with it.
        if (userName != null)
        {
            username = userName;
        }

        // Fetch user data (including followers) from the database.
        var users = await _userService.GetAllUserAsync(u => u.UserName == username, includeProperties: "Followers");

        // Get the first user (in case of only one match).
        var user = users.First();

        // Get the list of followers of the user.
        var followers = user!.Followers;

        // Return the view displaying the followers list.
        return View(followers);
    }

    // HTTP GET action to get the list of users followed by a user.
    [HttpGet("followed")]
    public async Task<IActionResult> GetFollowed([FromQuery] string? userName)
    {
        // Retrieve the currently authenticated user's username from session.
        string username = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;

        // If a username is provided as a query parameter, override the session username with it.
        if (userName != null)
        {
            username = userName;
        }

        // Fetch user data (including followed accounts) from the database.
        var users = await _userService.GetAllUserAsync(u => u.UserName == username, includeProperties: "Users");

        // Get the first user (in case of only one match).
        var user = users.First();

        // Get the list of users followed by the user.
        var followedAccounts = user!.Users;

        // Return the view displaying the list of followed accounts.
        return View(followedAccounts);
    }
}
