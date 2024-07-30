using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
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
                var u = await _context.Users.FirstOrDefaultAsync(u => u.UserName == usr.Value);
                HttpContext.Session.SetObject("User", u.ToUserResponse());
                user = u.ToUserResponse();
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
        public async Task<IActionResult> UploadProfileImage(IFormFile profileImage)
        {
            if(profileImage != null && profileImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream()) 
                {
                    await profileImage.CopyToAsync(memoryStream);
                    var us = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
                    var profile = await _context.Users.FirstOrDefaultAsync(u => u.UserName == us);

                    profile.UserImageByteType = memoryStream.ToArray();

                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }

                return NoContent();
            }

            return BadRequest(profileImage);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfileImage([FromQuery] uint userID)
        {
            var imageEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userID);
            if (imageEntity == null || imageEntity.UserImageByteType == null)
            {
                return NotFound();
            }

            return File(imageEntity.UserImageByteType, "image/png");
        }

        [HttpPost]
        public async Task<IActionResult> UploadBannerImage(IFormFile backgroundImg)
        {
            if (backgroundImg != null && backgroundImg.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await backgroundImg.CopyToAsync(memoryStream);
                    var us = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
                    var profile = await _context.Users.FirstOrDefaultAsync(u => u.UserName == us);

                    profile.UserBannerByteType = memoryStream.ToArray();

                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }

                return NoContent();
            }

            return BadRequest(backgroundImg);
        }


        [HttpGet]
        public async Task<IActionResult> GetBannerImage([FromQuery] uint userID)
        {
            var imageEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userID);
            if (imageEntity == null || imageEntity.UserBannerByteType == null)
            {
                return NotFound();
            }

            return File(imageEntity.UserBannerByteType, "image/png");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfileDescription(string desc)
        {
            var us = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;

            var profileToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.UserName == us);

            profileToUpdate!.UserDescription = desc;

            _context.Users.Update(profileToUpdate);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
