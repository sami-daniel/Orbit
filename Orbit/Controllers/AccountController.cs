using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Orbit.Application.Dtos.Requests;
using Orbit.Application.Dtos.Responses;
using Orbit.Application.Interfaces;
using Orbit.Extensions;
using Orbit.Filters;
using System.Security.Claims;

namespace Orbit.Controllers
{
    [EnsureProfileNotCreated]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([FromForm] UserAddRequest userAddRequest)
        {
            if (!ModelState.IsValid && !_webHostEnvironment.IsDevelopment())
            {
                IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                ViewBag.SummaryErrors = errors;
                ViewBag.ModalRegisActive = true;
                HttpContext.Response.StatusCode = 400;

                return RedirectToAction("Index");
            }
            else if (!ModelState.IsValid && _webHostEnvironment.IsDevelopment())
            {
                return BadRequest(ModelState);
            }

            UserResponse userReponse;

            try
            {
                userReponse = await _userService.AddUserAsync(userAddRequest);
            }
            catch (ArgumentException ex)
            {
                ViewBag.SummaryErrors = ex.Message;
                ViewBag.RegisModalActive = true;
                HttpContext.Response.StatusCode = 400;

                return View("Index");
            }

            List<Claim> claims =
            [
                new Claim(ClaimTypes.Role, "CommonUser"),
                new Claim(ClaimTypes.Email, userReponse.UserEmail),
                new Claim(ClaimTypes.NameIdentifier, userReponse.UserName)
            ];
            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new(identity);
            AuthenticationProperties authProperties = new()
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddYears(1)
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            HttpContext.Session.SetObject("User", userReponse);

            return RedirectToAction("", "Profile");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] string? input_login, [FromForm] string? password)
        {
            // ERRATA: O atributo email pode assumir dois valores - email ou username
            // porem, para o model binder realizar a vinculação de dados, o nome do
            // parametro não pode ser diferente do atributo name do campo do formulario
            // na página, então o nome do parametro permanece inalterado, assumindo sua
            // dupla função

            if (input_login == null || password == null)
            {
                HttpContext.Response.StatusCode = 400;
                ViewBag.LoginModalActive = true;

                return View("Index");
            }

            input_login = input_login.Trim();
            password = password.Trim();

            IEnumerable<UserResponse> users = await _userService.GetAllUsersAsync("Followers", "Users");

            UserResponse? user = input_login.Contains('@')
                ? users.FirstOrDefault(user => user.UserEmail == input_login)
                : users.FirstOrDefault(user => user.UserName == input_login);
            if (user == null || password != user.UserPassword)
            {
                ViewBag.LoginModalActive = true;
                ViewBag.LoginError = "Usuário ou senha inválidos!";
                ViewBag.InputLogin = input_login;
                ViewBag.InputPassword = password;

                HttpContext.Response.StatusCode = 401;

                return View("Index");
            }

            List<Claim> claims =
            [
                new Claim(ClaimTypes.Role, "CommonUser"),
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            ];
            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new(identity);
            AuthenticationProperties authProperties = new()
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddYears(1)
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            HttpContext.Session.SetObject("User", user);

            return RedirectToActionPermanent("", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<bool> CheckEmail([FromForm] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }

            IEnumerable<UserResponse> users = await _userService.FindUsersAsync(u => u.UserEmail == email);

            return !users.Any();
        }

        [HttpPost]
        public async Task<bool> CheckUsername([FromForm] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return true;
            }

            IEnumerable<UserResponse> users = await _userService.FindUsersAsync(u => u.UserName == username);

            return !users.Any();
        }
    }
}
