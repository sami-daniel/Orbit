using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Application.Interfaces;
using Orbit.Domain.Entities;
using Orbit.Extensions;
using Orbit.Infrastructure.Data.Contexts;

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
                Claim usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
                var users = _context.Users.Include(u => u.Users).Include(u => u.Followers).Where(u => u.UserEmail == usr.Value);
                user = await users.FirstOrDefaultAsync();
            }

            ViewBag.EditSectionUserName = user!.UserName;
            ViewBag.EditSectionProfileName = user!.UserProfileName;
            ViewBag.EditSectionDesc = user!.UserDescription;

            return View(user);
        }

        [HttpGet]
        [Route("[controller]/watch/{username}")]
        public async Task<IActionResult> ViewExternal(string username)
        {
            var users = _context.Users.Include(u => u.Users).Include(u => u.Followers).Where(u => u.UserName == username);
            var user = await users.FirstOrDefaultAsync();

            Claim usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
            var usersAL = _context.Users.Include(u => u.Users).Include(u => u.Followers).Where(u => u.UserEmail == usr.Value);
            var userAl = await usersAL.FirstOrDefaultAsync();

            ViewBag.ViewExternalUsernameFollower = userAl!.UserName;

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
            if (profileImage != null && profileImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profileImage.CopyToAsync(memoryStream);
                    var us = HttpContext.Session.GetObject<User>("User")!.UserName;
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
                    var us = HttpContext.Session.GetObject<User>("User")!.UserName; // This is unstable because
                                                                                    // the image only can be
                                                                                    // updated if the user is recently
                                                                                    // logged, because the profile controller
                                                                                    // don't set the User in the section, 
                                                                                    // but only verify if is alredy on session.
                                                                                    // Otherwhise, just proceed the flow
                    var profile = await _context.Users.FirstOrDefaultAsync(u => u.UserName == us);

                    profile.UserBannerByteType = memoryStream.ToArray();

                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }

                return NoContent();
            }

            return BadRequest(backgroundImg);
        }

        [HttpPost]
        public async Task<IActionResult> Follow(int id, string followerUserName)
        {
            if (followerUserName == null)
            {
                return BadRequest("Follower user name não pode ser vazio!");
            }

            var usrToFollow = await _context.Users.Include(u => u.Users).Include(u => u.Followers).Where(u => u.UserId == id).FirstAsync();
            var follower = await _context.Users.Where(u => u.UserName == followerUserName).FirstOrDefaultAsync();

            usrToFollow.Users.Add(follower!);

            await _context.SaveChangesAsync();

            return Ok();
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

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(User user, int id)
        {
            var usr = await _context.Users.FindAsync(uint.Parse(id.ToString()));

            if (usr == null)
            {
                return NotFound();
            }

            usr.UserProfileName = user.UserProfileName;
            usr.UserName = user.UserName;
            usr.UserDescription = user.UserDescription;
            HttpContext.Session.Clear();
            HttpContext.Session.SetObject("User", usr);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfileDescription(string desc)
        {
            var us = HttpContext.Session.GetObject<User>("User")!.UserName;

            var profileToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.UserName == us);

            profileToUpdate!.UserDescription = desc;

            _context.Users.Update(profileToUpdate);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
