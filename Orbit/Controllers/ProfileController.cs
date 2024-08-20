using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Application.Dtos.Responses;
using Orbit.Application.Interfaces;
using Orbit.Domain.Entities;
using Orbit.Extensions;
using Orbit.Infrastructure.Data.Contexts;
using System.Security.Claims;

namespace Orbit.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public readonly IUserService _userService;
        public readonly ApplicationDbContext _context;

        public ProfileController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            User? user = HttpContext.Session.GetObject<User>("User");

            if (user == null)
            {
                Claim usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
                var users = _context.Users.Include(u => u.Users).Include(u => u.Followers).Where(u => u.UserName == usr.Value);
                user = await users.FirstOrDefaultAsync();
            }

            return View(user);
        }

        [HttpGet]
        [Route("[controller]/watch/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> ViewExternal(string username)
        {
            var users = _context.Users.Include(u => u.Users).Include(u => u.Followers).Where(u => u.UserName == username);
            var user = await users.FirstOrDefaultAsync();

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
