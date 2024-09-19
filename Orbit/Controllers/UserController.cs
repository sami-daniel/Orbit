using System.Net.NetworkInformation;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Application.Interfaces;
using Orbit.Domain.Entities;
using Orbit.DTOs.Responses;
using Orbit.Extensions;
using Orbit.Filters;
using Orbit.Infrastructure.Data.Contexts;

namespace Orbit.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IMapper mapper, IUserService userService)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            UserResponse? userResponse = HttpContext.Session.GetObject<UserResponse>("User");
            Claim usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
            var user = await _userService.GetAllUserAsync(u => u.UserEmail == usr.Value!, includeProperties: "Followers,Users");
            userResponse = _mapper.Map<User, UserResponse>(user.First());
            HttpContext.Session.SetObject("User", userResponse);

            ViewBag.EditSectionUserName = userResponse!.UserName;
            ViewBag.EditSectionProfileName = userResponse!.UserProfileName;
            ViewBag.EditSectionDesc = userResponse!.UserDescription;

            return View(userResponse);
        }

        [HttpGet]
        [Route("{username}")]
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

        [HttpGet("[action]")]
        public async Task<IActionResult> Search([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Query não pode ser vazia!");
            }

            string normalizeQuery = username.ToLower().Trim();

            var profiles = await _userService.GetAllUserAsync();
            profiles = profiles.Where(u => u.UserName.Contains(username));
            var matchProfiles = profiles.Select(p => new { p.UserName, ProfileName = p.UserProfileName, p.UserProfileImageByteType });

            return Ok(matchProfiles);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadProfileImage(IFormFile profileImage, [FromServices] ApplicationDbContext context)
        {
            if (profileImage != null && profileImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profileImage.CopyToAsync(memoryStream);
                    var us = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
                    var profile = await _userService.GetUserByIdentifierAsync(us);

                    profile!.UserProfileImageByteType = memoryStream.ToArray();
                    await context.SaveChangesAsync();
                }

                return NoContent();
            }

            return BadRequest(profileImage);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProfileImage([FromQuery] string userID)
        {
            var imageEntity = await _userService.GetUserByIdentifierAsync(userID);
            if (imageEntity == null || imageEntity.UserProfileImageByteType == null)
            {
                return NotFound();
            }

            return File(imageEntity.UserProfileImageByteType, "image/png");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadBannerImage(IFormFile backgroundImg, [FromServices] ApplicationDbContext context)
        {
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Follow(string id, string followerUserName, [FromServices] ApplicationDbContext applicationDbContext)
        {
            if (followerUserName == null)
            {
                return BadRequest("Follower user name não pode ser vazio!");
            }

            var userToBeFollowed = await _userService.GetUserByIdentifierAsync(id);
            var follower = await _userService.GetUserByIdentifierAsync(followerUserName);

            userToBeFollowed!.Followers.Add(follower!);
            await applicationDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetBannerImage([FromQuery] string userID)
        {
            var imageEntity = await _userService.GetUserByIdentifierAsync(userID);
            if (imageEntity == null || imageEntity.UserProfileBannerImageByteType == null)
            {
                return NotFound();
            }

            return File(imageEntity.UserProfileBannerImageByteType, "image/png");
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateProfile(User user, string id, [FromServices] ApplicationDbContext context)
        {
            var usr = await _userService.GetUserByIdentifierAsync(id);

            if (usr == null)
            {
                return NotFound();
            }

            usr.UserProfileName = user.UserProfileName;
            usr.UserName = user.UserName;
            usr.UserDescription = user.UserDescription;
            await context.SaveChangesAsync();
            HttpContext.Session.Clear();
            HttpContext.Session.SetObject("User", _mapper.Map<User, UserResponse>(usr));

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfileDescription(string desc, [FromServices] ApplicationDbContext context)
        {
            var us = HttpContext.Session.GetObject<User>("User")!.UserName;

            var profileToUpdate = await _userService.GetUserByIdentifierAsync(us);

            profileToUpdate!.UserDescription = desc;

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
