using Microsoft.AspNetCore.Mvc;
using Orbit.Application.Dtos.Requests;
using Orbit.Application.Dtos.Responses;
using Orbit.Application.Interfaces;
using Orbit.Extensions;

namespace Orbit.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateUser(UserAddRequest userAddRequest)
        {
            userAddRequest.UserDateOfBirth = new DateOnly(userAddRequest.Year, userAddRequest.Month, userAddRequest.Day);
            if (!ModelState.IsValid && !_webHostEnvironment.IsDevelopment())
            {
                var errors = string.Join(',', ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errors);
            }
            else if (!ModelState.IsValid && _webHostEnvironment.IsDevelopment())
            {
                return BadRequest(ModelState);
            }

            UserReponse userReponse;

            try
            {
                userReponse = await _userService.AddUserAsync(userAddRequest);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            HttpContext.Session.SetObject("User", userReponse);

            return RedirectToAction("", "Dashboard");
        }

        public async Task<IActionResult> Login(string email, string password)
        {
            var users = await _userService.GetAllUsersAsync();

            var user = users.FirstOrDefault(user => user.UserEmail == email);

            if (user == null)
            {
                return NotFound("Usuario com esse nome nâo foi encontrado!");
            }

            if (password != user.UserPassword)
            {
                return BadRequest("Senha incorreta!");
            }

            HttpContext.Session.SetObject("User", user);

            return RedirectToActionPermanent("", "Dashboard");
        }

        [HttpPost]
        public async Task<bool> CheckEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }
            var users = await _userService.GetAllUsersAsync();

            var userWithEmail = users.Where(u => u.UserEmail == email);

            if (userWithEmail.Count() > 0)
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        public async Task<bool> CheckUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return true;
            }

            var users = await _userService.GetAllUsersAsync();

            var usersWithID = users.Where(u => u.UserName == username);

            if (usersWithID.Count() > 0)
            {
                return false;
            }

            return true;
        }
    }
}
