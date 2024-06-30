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
                var usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
                var awaiter = await _userService.GetAllUsersAsync("Followers", "Users");
                
                user = awaiter.Where(u => u.UserName == usr.Value).First();
            }

            return View(user);
        }

        public async Task<IActionResult> GetUserImageAsync(string username)
        {
            IEnumerable<UserResponse> users = await _userService.FindUsersAsync(u => u.UserName == username);

            UserResponse? user = users.FirstOrDefault();
            // Se o UserService possuisse um método Find, em vez de ter que pegar todos os usuarios
            // e então usar o Linq para pegar o usúario desejado, teriamos melhor performance

            return user == null || user.UserImageByteType == null ? NotFound() : File(user.UserImageByteType, "application/image");
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Query não pode ser vazia!");
            }

            string normalizeQuery = username.ToLower().Trim();

            IEnumerable<UserResponse> profiles = await _userService.FindUsersAsync(p => p.UserName.ToLower().Contains(normalizeQuery));


            var matchProfiles = profiles.Select(p => new { p.UserName, ProfileName = p.UserProfileName, p.UserImageByteType });

            return Ok(matchProfiles);
        }
    }
}
