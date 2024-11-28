using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Orbit.Data.Contexts;
using Orbit.DTOs.Responses;
using Orbit.Extensions;
using Orbit.Hubs;
using Orbit.Models;
using Orbit.Services.Interfaces;

namespace Orbit.Controllers;

// Controller for user-related actions that requires authentication
[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService; // Service for user-related actions
    private readonly IMapper _mapper; // Mapper for converting models to DTOs
    private readonly IHubContext<NotificationHub> _hubContext; // SignalR hub context for notifications

    // Constructor to initialize the controller with dependencies
    public UserController(IMapper mapper, IUserService userService, IHubContext<NotificationHub> hubContext)
    {
        _userService = userService;
        _mapper = mapper;
        _hubContext = hubContext;
    }

    [HttpGet]
    [Route("[controller]/search")]
    public async Task<IActionResult> Search([FromQuery] string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return BadRequest("Query não pode ser vazia!");
        }

        string normalizeQuery = username.ToLower().Trim();

        IEnumerable<User> profiles = await _userService.GetAllUsersAsync();
        profiles = profiles.Where(u => u.UserName.Contains(username, StringComparison.InvariantCultureIgnoreCase));
        var matchProfiles = profiles.Select(p => new { p.UserName, ProfileName = p.UserProfileName, p.UserProfileImageByteType });

        return Ok(matchProfiles);
    }

    // GET: [controller] - Displays the user profile page
    [HttpGet("[controller]")]
    public async Task<IActionResult> Index()
    {
        // Retrieve the current user from the session
        UserResponse? userResponse = HttpContext.Session.GetObject<UserResponse>("User");
        
        // Get the user's email from the claims
        Claim usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
        
        // Get the user from the database by email
        var user = await _userService.GetAllUsersAsync(u => u.UserEmail == usr.Value!, includeProperties: "Followers,Users");
        
        // If user is not found, sign out and redirect to login page
        if (user.FirstOrDefault() == null)
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction("index", "account");
        }

        // Map the user model to a response DTO
        userResponse = _mapper.Map<User, UserResponse>(user.First());
        
        // Save the user response in session and set login state
        HttpContext.Session.SetObject("User", userResponse);
        ViewBag.StateLogin = HttpContext.Session.GetString("is-first-time");
        HttpContext.Session.SetString("is-first-time", false.ToString());

        return View(userResponse);
    }

    // GET: [controller]/{username} - Displays another user's profile
    [HttpGet]
    [Route("[controller]/{username}")]
    public async Task<IActionResult> ViewExternal([FromRoute] string username, [FromQuery] string returnTo)
    {
        // If the requested username is the current user's, redirect to their own profile
        if (username == HttpContext.Session.GetObject<UserResponse>("User")!.UserName)
        {
            return RedirectToAction("Index", "User", new { returnTo});
        }

        // Retrieve the requested user from the database
        var users = await _userService.GetAllUsersAsync(u => u.UserName == username, includeProperties: "Followers,Users");
        var user = users.FirstOrDefault();

        // If the user is not found, return a 404 error
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Get the current logged-in user's information
        Claim usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
        var usersAL = await _userService.GetAllUsersAsync(u => u.UserEmail == usr.Value!, includeProperties: "Followers,Users");
        var userAl = usersAL.First();

        ViewBag.ViewExternalUsernameFollower = userAl!.UserName;

        // Return the profile view with the user data
        return View(_mapper.Map<User, UserResponse>(user));
    }

    // POST: [controller]/follow - Allows a user to follow another user
    [HttpPost("[controller]/follow")]
    public async Task<IActionResult> Follow([FromForm] string id, [FromQuery] string returnTo, [FromForm] string followerUserName)
    {
        if (followerUserName == null)
        {
            return BadRequest("Follower user name cannot be empty!");
        }

        // Retrieve the user to be followed and the follower user
        var userToBeFollowed = await _userService.GetUserByIdentifierAsync(id);
        var follower = await _userService.GetUserByIdentifierAsync(followerUserName);

        // Validate that the users exist
        if (follower == null)
        {
            return BadRequest($"The follower user with the username '{followerUserName}' does not exist.");
        }

        if (userToBeFollowed == null)
        {
            return BadRequest($"The user to be followed with the username '{userToBeFollowed}' does not exist.");
        }

        // Perform the follow action
        await _userService.FollowUserAsync(follower.UserName, userToBeFollowed.UserName);

        // Send a notification to the followed user via SignalR
        await _hubContext.Clients.User(userToBeFollowed.UserName).SendAsync("ReceiveNotification", userToBeFollowed.UserName, $"<a style=\"color:black\" href=\"../user/{follower.UserName}\">{follower.UserName} followed you!</a>");

        return RedirectPermanent(returnTo);
    }

    // POST: [controller]/unfollow - Allows a user to unfollow another user
    [HttpPost("[controller]/unfollow")]
    public async Task<IActionResult> Unfollow([FromForm] string id, [FromQuery] string returnTo, [FromForm] string followerUserName, [FromServices] ApplicationDbContext applicationDbContext)
    {
        if (followerUserName == null)
        {
            return BadRequest("Follower user name cannot be empty!");
        }

        // Retrieve the user being unfollowed and the follower user
        var userBeingUnfollowed = await _userService.GetAllUsersAsync(u => u.UserName == id, includeProperties: "Followers");
        var userFollower = await _userService.GetUserByIdentifierAsync(followerUserName);

        // Remove the follower from the user
        userBeingUnfollowed.First().Followers.Remove(userFollower!);
        await applicationDbContext.SaveChangesAsync();

        // Send a notification to the unfollowed user via SignalR
        await _hubContext.Clients.User(userBeingUnfollowed!.First().UserName).SendAsync("ReceiveNotification", userFollower!.UserName, $"<a style=\"color:black\" href=\"user/{userFollower.UserName}\">{userFollower.UserName} unfollowed you!</a>");

        return RedirectPermanent(returnTo);
    }

    // GET: [controller]/get-banner-image - Retrieves the user's profile banner image
    [HttpGet("[controller]/get-banner-image")]
    public async Task<IActionResult> GetBannerImage([FromQuery] string userName)
    {
        var imageEntity = await _userService.GetUserByIdentifierAsync(userName);
        if (imageEntity == null || imageEntity.UserProfileBannerImageByteType == null)
        {
            return NotFound();
        }

        return File(imageEntity.UserProfileBannerImageByteType, "image/png");
    }

    // GET: [controller]/get-profile-image - Retrieves the user's profile image
    [HttpGet("[controller]/get-profile-image")]
    public async Task<IActionResult> GetProfileImage([FromQuery] string userName)
    {
        var imageEntity = await _userService.GetUserByIdentifierAsync(userName);
        if (imageEntity == null || imageEntity.UserProfileImageByteType == null)
        {
            return NotFound();
        }

        return File(imageEntity.UserProfileImageByteType, "image/png");
    }

    // POST: [controller]/update-profile/{name} - Updates the user's profile information
    [HttpPost("[controller]/update-profile/{name}")]
    public async Task<IActionResult> UpdateProfile([FromForm] IFormFile curriculum, [FromForm] User user, [FromRoute] string name, [FromQuery] string returnTo, [FromServices] ApplicationDbContext context)
    {
        var usr = await _userService.GetUserByIdentifierAsync(name);

        if (usr == null)
        {
            return RedirectPermanent(returnTo);
        }

        // Update user profile fields
        usr.UserProfileName = user.UserProfileName;
        usr.UserName = user.UserName;
        usr.UserDescription = user.UserDescription;
        await context.SaveChangesAsync();

        // Update the user's claims for authentication
        var claims = User.Claims;
        var newClaims = new List<Claim>(claims);
        newClaims.Remove(claims.First(c => c.Type == ClaimTypes.NameIdentifier));
        newClaims.Add(new Claim(ClaimTypes.NameIdentifier, usr.UserName));
        await HttpContext.SignOutAsync();
        var identity = new ClaimsIdentity(newClaims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        // Clear session and update the user session
        HttpContext.Session.Clear();
        HttpContext.Session.SetObject("User", _mapper.Map<User, UserResponse>(usr));

        return RedirectPermanent(returnTo);
    }

    // POST: [controller]/upload-banner-image - Uploads the user's profile banner image
    [HttpPost("[controller]/upload-banner-image")]
    public async Task<IActionResult> UploadBannerImage([FromForm] IFormFile backgroundImg, [FromServices] ApplicationDbContext context)
    {
        if (backgroundImg.ContentType.Contains("image") == false)
        {
            return BadRequest("The file is not an image!");
        }

        if (backgroundImg != null && backgroundImg.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await backgroundImg.CopyToAsync(memoryStream);
                var us = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
                var profile = await _userService.GetUserByIdentifierAsync(us);

                profile!.UserProfileBannerImageByteType = memoryStream.ToArray();
                await context.SaveChangesAsync();
            }

            return NoContent();
        }

        return BadRequest(backgroundImg);
    }

    // POST: [controller]/upload-profile-image - Uploads the user's profile image
    [HttpPost("[controller]/upload-profile-image")]
    public async Task<IActionResult> UploadProfileImage([FromForm] IFormFile profileImage, [FromServices] ApplicationDbContext context)
    {
        if (profileImage.ContentType.Contains("image") == false)
        {
            return BadRequest("The file is not an image!");
        }

        if (profileImage != null && profileImage.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await profileImage.CopyToAsync(memoryStream);
                var username = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
                var profile = await _userService.GetUserByIdentifierAsync(username);

                profile!.UserProfileImageByteType = memoryStream.ToArray();
                await context.SaveChangesAsync();
            }

            return NoContent();
        }

        return BadRequest(profileImage);
    }
}
