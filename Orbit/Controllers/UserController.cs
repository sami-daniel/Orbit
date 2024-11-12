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

[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IHubContext<NotificationHub> _hubContext;

    public UserController(IMapper mapper, IUserService userService, IHubContext<NotificationHub> hubContext)
    {
        _userService = userService;
        _mapper = mapper;
        _hubContext = hubContext;
    }

    [HttpGet("[controller]")]
    public async Task<IActionResult> Index()
    {
        UserResponse? userResponse = HttpContext.Session.GetObject<UserResponse>("User");
        Claim usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
        var user = await _userService.GetAllUserAsync(u => u.UserEmail == usr.Value!, includeProperties: "Followers,Users");
        if (user.FirstOrDefault() == null)
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction("index", "account");
        }

        userResponse = _mapper.Map<User, UserResponse>(user.First());
        HttpContext.Session.SetObject("User", userResponse);

        return View(userResponse);
    }

    [HttpGet]
    [Route("[controller]/{username}")]
    public async Task<IActionResult> ViewExternal(string username, string? returnTo)
    {
        var users = await _userService.GetAllUserAsync(u => u.UserName == username, includeProperties: "Followers,Users");
        var user = users.FirstOrDefault();

        if (user == null)
        {
            return RedirectToRoute(returnTo);
        }

        Claim usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
        var usersAL = await _userService.GetAllUserAsync(u => u.UserEmail == usr.Value!, includeProperties: "Followers,Users");
        var userAl = usersAL.First();

        ViewBag.ViewExternalUsernameFollower = userAl!.UserName;

        if (user is null)
            return NotFound();

        return View(_mapper.Map<User, UserResponse>(user));
    }

    [HttpPost("[controller]/follow")]
    public async Task<IActionResult> Follow([FromForm] string id, [FromQuery] string returnTo, [FromForm] string followerUserName, [FromServices] ApplicationDbContext applicationDbContext)
    {
        if (followerUserName == null)
        {
            return BadRequest("Follower user name não pode ser vazio!");
        }

        var userToBeFollowed = await _userService.GetUserByIdentifierAsync(id);
        var follower = await _userService.GetUserByIdentifierAsync(followerUserName);

        userToBeFollowed!.Followers.Add(follower!);
        await applicationDbContext.SaveChangesAsync();

        return RedirectPermanent(returnTo);
    }

    [HttpPost("[controller]/unfollow")]
    public async Task<IActionResult> Unfollow([FromForm] string id, [FromQuery] string returnTo, [FromForm] string followerUserName, [FromServices] ApplicationDbContext applicationDbContext)
    {
        if (followerUserName == null)
        {
            return BadRequest("Follower user name não pode ser vazio!");
        }

        var userToBeFollowed = await _userService.GetAllUserAsync(u => u.UserName == id, includeProperties: "Followers");
        var follower = await _userService.GetUserByIdentifierAsync(followerUserName);

        userToBeFollowed.First().Followers.Remove(follower!);
        await applicationDbContext.SaveChangesAsync();

        return RedirectPermanent(returnTo);
    }

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

    [HttpPost("[controller]/update-profile/{name}")]
    public async Task<IActionResult> UpdateProfile([FromForm] User user, [FromRoute] string name, [FromQuery] string returnTo, [FromServices] ApplicationDbContext context)
    {
        var usr = await _userService.GetUserByIdentifierAsync(name);

        if (usr == null)
        {
            return RedirectPermanent(returnTo);
        }

        usr.UserProfileName = user.UserProfileName;
        usr.UserName = user.UserName;
        usr.UserDescription = user.UserDescription;
        await context.SaveChangesAsync();
        var claims = User.Claims;
        var newClaims = new List<Claim>(claims);
        newClaims.Remove(claims.First(c => c.Type == ClaimTypes.NameIdentifier));
        newClaims.Add(new Claim(ClaimTypes.NameIdentifier, usr.UserName));
        await HttpContext.SignOutAsync();
        var identity = new ClaimsIdentity(newClaims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        HttpContext.Session.Clear();
        HttpContext.Session.SetObject("User", _mapper.Map<User, UserResponse>(usr));

        return RedirectPermanent(returnTo);
    }

    [HttpPost("[controller]/upload-banner-image")]
    public async Task<IActionResult> UploadBannerImage([FromForm] IFormFile backgroundImg, [FromServices] ApplicationDbContext context)
    {
        if (backgroundImg.ContentType.Contains("image") == false)
        {
            return BadRequest("O arquivo não é uma imagem!");
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

    [HttpPost("[controller]/upload-profile-image")]
    public async Task<IActionResult> UploadProfileImage([FromForm] IFormFile profileImage, [FromServices] ApplicationDbContext context)
    {
        if (profileImage.ContentType.Contains("image") == false)
        {
            return BadRequest("O arquivo não é uma imagem!");
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
