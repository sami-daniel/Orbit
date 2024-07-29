using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Orbit.Application.Dtos.Responses;
using Orbit.Application.Interfaces;
using Orbit.Extensions;
using Orbit.Infrastructure.Data.Contexts;
using Pomelo.EntityFrameworkCore.MySql.Metadata.Internal;
using System.Security.Claims;

namespace Orbit.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public UserResponse UserInSession { get; set; } = null!;
        public ProfileController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
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

            UserInSession = user;
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

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile imageFile)
        {
            if(imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream()) 
                {
                    await imageFile.CopyToAsync(memoryStream);

                    var profile = _context.Users.First(u => u.UserName == UserInSession.UserName);

                    profile.UserImageByteType = memoryStream.ToArray();

                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }

                return NoContent();
            }

            return BadRequest(imageFile);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfileImage(object userID)
        {
            var imageEntity = await _context.Users.FindAsync(userID);
            if (imageEntity == null || imageEntity.UserImageByteType == null)
            {
                return NotFound();
            }

            return File(imageEntity.UserImageByteType, "image/png");
        }
    }
}
