using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.DTOs.Responses;
using Orbit.Extensions;
using Orbit.Models;
using Orbit.Services.Interfaces;

namespace Orbit.Controllers;

// Ensures only authenticated users can access this controller
[Authorize]
public class PanelController : Controller
{
    private readonly IUserService _userService;  // Service for user-related operations
    private readonly IMapper _mapper;  // AutoMapper to map between DTOs and models

    // Constructor to initialize the user service and mapper
    public PanelController(IUserService userService, IMapper mapper)
    {
        _userService = userService; 
        _mapper = mapper;   
    }

    // GET: /Panel - Index action for the panel view
    public async Task<IActionResult> Index()
    {
        // Retrieve the "User" object stored in the session
        UserResponse? userResponse = HttpContext.Session.GetObject<UserResponse>("User");

        // Get the email claim from the current user
        Claim usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);

        // Fetch the user from the database based on their email
        var user = await _userService.GetAllUsersAsync(u => u.UserEmail == usr.Value!, includeProperties: "Followers,Users");

        // If the user doesn't exist, clear the session, sign out, and redirect to the login page
        if (user.FirstOrDefault() == null)
        {
            HttpContext.Session.Clear();  // Clear session
            await HttpContext.SignOutAsync();  // Sign out the current user
            return RedirectToAction("index", "account");  // Redirect to the account login page
        }

        // Map the User entity to a UserResponse DTO
        userResponse = _mapper.Map<User, UserResponse>(user.First());

        // Store the UserResponse object back in the session
        HttpContext.Session.SetObject("User", userResponse);

        // Set the "is-first-time" flag to false (this can be used to display first-time login messages or settings)
        ViewBag.StateLogin = HttpContext.Session.GetString("is-first-time");
        HttpContext.Session.SetString("is-first-time", false.ToString());

        // Return the view with the UserResponse object
        return View(userResponse);
    }
}
