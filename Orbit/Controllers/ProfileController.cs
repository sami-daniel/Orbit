using Microsoft.AspNetCore.Mvc;
using Orbit.Application.Dtos.Responses;
using Orbit.Application.Interfaces;
using Orbit.Extensions;

namespace Orbit.Controllers
{
    public class ProfileController : Controller
    {
        public readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var user = HttpContext.Session.GetObject<UserResponse>("User");

            if (user == null)
            {
                return RedirectToAction("", "Account");
            }
#warning FIXME: A autenticação e autorização via OAuth2.0 (Facebook, Google, Microsoft...) não está habilitado! Atualmente o redirecionamento para a página inicial verifica somente se o usuario está registrado na sessão ou não
            return View(user);
        }

        public async Task<IActionResult> GetUserImageAsync(string username)
        {
            var users = await _userService.GetAllUsersAsync();

            var user = users.FirstOrDefault(u => u.UserName == username); 
            // Se o UserService possuisse um método Find, em vez de ter que pegar todos os usuarios
            // e então usar o Linq para pegar o usúario desejado, teriamos melhor performance

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
