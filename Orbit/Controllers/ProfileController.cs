using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.Application.Dtos.Responses;
using Orbit.Application.Interfaces;
using Orbit.Extensions;
using System.Security.Claims;

namespace Orbit.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            UserResponse? user = HttpContext.Session.GetObject<UserResponse>("User");

            if (user == null)
            {
                Claim usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
                IEnumerable<UserResponse> awaiter = await _userService.FindUsersAsync(new { UserName = usr.Value },"Followers", "Users");

                user = awaiter.Where(u => u.UserName == usr.Value).First();
            }

            return View(user);
        }

        [HttpGet]
        [Route("[controller]/watch/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> ViewExternal(string username)
        {
            var users = await _userService.FindUsersAsync(new { UserName = username });
            var user = users.FirstOrDefault();

            if (user is null)
                return NotFound();

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Query não pode ser vazia!");
            }

            string normalizeQuery = username.ToLower().Trim();

            IEnumerable<UserResponse> profiles = await _userService.GetAllUsersAsync();
            profiles = profiles.Where(u => u.UserName.Contains(username));
            var matchProfiles = profiles.Select(p => new { p.UserName, ProfileName = p.UserProfileName, p.UserImageByteType });

            return Ok(matchProfiles);
        }
    }
}
