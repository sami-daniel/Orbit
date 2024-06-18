using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Application.Dtos.Responses;
using Orbit.Application.Interfaces;
using Orbit.Extensions;
using Orbit.Infrastructure.Data.Contexts;

namespace Orbit.Controllers
{
    public class DashboardController : Controller
    {
        public readonly IUserService _userService;

        public DashboardController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var user = HttpContext.Session.GetObject<UserReponse>("User");

            if (user == null)
            {
                return RedirectToAction("", "Account");
            }

            return View(user);
        }

        public async Task<IActionResult> GetUserImageAsync (string username)
        {
            var users = await _userService.GetAllUsersAsync();

            var user = users.FirstOrDefault(u => u.UserName == username);

            if (user == null || user.UserImageByteType == null) 
            {
                return NotFound();
            }

            return File(user.UserImageByteType, "application/image");
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Query não pode ser vazia!");
            }

            var normalizeQuery = username.ToLower().Trim();

            var profiles = await _userService.GetAllUsersAsync();

            var matchProfiles =
                profiles
                    .Where(p => p.UserName.ToLower().Contains(normalizeQuery))
                    .Select(p => new { UserName = p.UserName, ProfileName = p.UserProfileName, UserImageByteType = p.UserImageByteType });

            return Ok(matchProfiles);
        } 
    }
}
