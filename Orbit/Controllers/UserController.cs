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

namespace Orbit.Controllers
{
    [EnsureUserNotCreated]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IMapper mapper ,IUserService userService)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            UserResponse? userResponse = HttpContext.Session.GetObject<UserResponse>("User");

            if (userResponse == null)
            {
                Claim usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
                var user = await _userService.GetUserByIdentifierAsync(usr.Value);
                userResponse = _mapper.Map<User, UserResponse>(user!);
                HttpContext.Session.SetObject("User", userResponse);
            }

            ViewBag.EditSectionUserName = userResponse!.UserName;
            ViewBag.EditSectionProfileName = userResponse!.UserProfileName;
            ViewBag.EditSectionDesc = userResponse!.UserDescription;

            return View(userResponse);
        }

        //[HttpGet]
        //[Route("[controller]/watch/{username}")]
        //public async Task<IActionResult> ViewExternal(string username)
        //{
        //    var users = _context.Users.Include(u => u.Users).Include(u => u.Followers).Where(u => u.UserName == username);
        //    var user = await users.FirstOrDefaultAsync();

        //    Claim usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
        //    var usersAL = _context.Users.Include(u => u.Users).Include(u => u.Followers).Where(u => u.UserEmail == usr.Value);
        //    var userAl = await usersAL.FirstOrDefaultAsync();

        //    ViewBag.ViewExternalUsernameFollower = userAl!.UserName;

        //    if (user is null)
        //        return NotFound();

        //    return View(user);
        //}

        //[HttpGet]
        //public async Task<IActionResult> Search([FromQuery] string username)
        //{
        //    if (string.IsNullOrEmpty(username))
        //    {
        //        return BadRequest("Query não pode ser vazia!");
        //    }

        //    string normalizeQuery = username.ToLower().Trim();

        //    var profiles = await _userService.GetAllUserAsync();
        //    profiles = profiles.Where(u => u.UserName.Contains(username));
        //    var matchProfiles = profiles.Select(p => new { p.UserName, ProfileName = p.UserProfileName, p.UserProfileImageByteType });

        //    return Ok(matchProfiles);
        //}

        //[HttpPost]
        //public async Task<IActionResult> UploadProfileImage(IFormFile profileImage)
        //{
        //    if (profileImage != null && profileImage.Length > 0)
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await profileImage.CopyToAsync(memoryStream);
        //            var us = HttpContext.Session.GetObject<User>("User")!.UserName;
        //            var profile = await _context.Users.FirstOrDefaultAsync(u => u.UserName == us);

        //            profile.UserProfileImageByteType = memoryStream.ToArray();

        //            _context.Update(profile);
        //            await _context.SaveChangesAsync();
        //        }

        //        return NoContent();
        //    }

        //    return BadRequest(profileImage);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetProfileImage([FromQuery] uint userID)
        //{
        //    var imageEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userID);
        //    if (imageEntity == null || imageEntity.UserProfileImageByteType == null)
        //    {
        //        return NotFound();
        //    }

        //    return File(imageEntity.UserProfileImageByteType, "image/png");
        //}

        //[HttpPost]
        //public async Task<IActionResult> UploadBannerImage(IFormFile backgroundImg)
        //{
        //    if (backgroundImg != null && backgroundImg.Length > 0)
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await backgroundImg.CopyToAsync(memoryStream);
        //            var us = HttpContext.Session.GetObject<User>("User")!.UserName; // This is unstable because
        //                                                                            // the image only can be
        //                                                                            // updated if the user is recently
        //                                                                            // logged, because the profile controller
        //                                                                            // don't set the User in the section, 
        //                                                                            // but only verify if is alredy on session.
        //                                                                            // Otherwhise, just proceed the flow
        //            var profile = await _context.Users.FirstOrDefaultAsync(u => u.UserName == us);

        //            profile.UserProfileBannerImageByteType = memoryStream.ToArray();

        //            _context.Update(profile);
        //            await _context.SaveChangesAsync();
        //        }

        //        return NoContent();
        //    }

        //    return BadRequest(backgroundImg);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Follow(int id, string followerUserName)
        //{
        //    if (followerUserName == null)
        //    {
        //        return BadRequest("Follower user name não pode ser vazio!");
        //    }

        //    var usrToFollow = await _context.Users.Include(u => u.Users).Include(u => u.Followers).Where(u => u.UserId == id).FirstAsync();
        //    var follower = await _context.Users.Where(u => u.UserName == followerUserName).FirstOrDefaultAsync();

        //    usrToFollow.Users.Add(follower!);

        //    await _context.SaveChangesAsync();

        //    return Ok();
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetBannerImage([FromQuery] uint userID)
        //{
        //    var imageEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userID);
        //    if (imageEntity == null || imageEntity.UserProfileBannerImageByteType == null)
        //    {
        //        return NotFound();
        //    }

        //    return File(imageEntity.UserProfileBannerImageByteType, "image/png");
        //}

        //[HttpPut]
        //public async Task<IActionResult> UpdateProfile(User user, int id)
        //{
        //    var usr = await _context.Users.FindAsync(uint.Parse(id.ToString()));

        //    if (usr == null)
        //    {
        //        return NotFound();
        //    }

        //    usr.UserProfileName = user.UserProfileName;
        //    usr.UserName = user.UserName;
        //    usr.UserDescription = user.UserDescription;
        //    HttpContext.Session.Clear();
        //    HttpContext.Session.SetObject("User", usr);
        //    await _context.SaveChangesAsync();

        //    return Ok();
        //}

        //[HttpPost]
        //public async Task<IActionResult> UpdateProfileDescription(string desc)
        //{
        //    var us = HttpContext.Session.GetObject<User>("User")!.UserName;

        //    var profileToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.UserName == us);

        //    profileToUpdate!.UserDescription = desc;

        //    _context.Users.Update(profileToUpdate);

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
