using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Orbit.Application.Dtos.Requests;
using Orbit.Application.Dtos.Responses;
using Orbit.Application.Interfaces;
using Orbit.Domain.Entities;
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
        public async Task<IActionResult> CreateUser(UserAddRequest userAddRequest)
        {
            userAddRequest.UserDateOfBirth = new DateOnly(userAddRequest.Year, userAddRequest.Month, userAddRequest.Day);
            if (!ModelState.IsValid && !_webHostEnvironment.IsDevelopment())
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                ViewBag.SummaryErrors = errors;
                ViewBag.ModalRegisActive = true;
                HttpContext.Response.StatusCode = 400;

                return View("Index");
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

                return View("Index");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "CommonUser"),
                new Claim(ClaimTypes.Email, userReponse.UserEmail),
                new Claim(ClaimTypes.NameIdentifier, userReponse.UserName)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddYears(1)
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            HttpContext.Session.SetObject("User", userReponse);

            return RedirectToAction("", "Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            // ERRATA: O atributo email pode assumir dois valores - email ou username
            // porem, para o model binder realizar a vinculação de dados, o nome do
            // parametro não pode ser diferente do atributo name do campo do formulario
            // na página, então o nome do parametro permanece inalterado, assumindo sua
            // dupla função

            IEnumerable<UserResponse> users = await _userService.GetAllUsersAsync("Followers", "Users");

            UserResponse? user = email.Contains('@')
                ? users.FirstOrDefault(user => user.UserEmail == email)
                : users.FirstOrDefault(user => user.UserName == email);
            if (user == null || password != user.UserPassword)
            {
                ViewBag.LoginError = "Usuário ou senha incorretos";
                HttpContext.Response.StatusCode = 401;
                ViewBag.ModalLoginActive = true;

                return View("Index");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "CommonUser"),
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddYears(1)
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            HttpContext.Session.SetObject("User", user);

            return RedirectToActionPermanent("", "Profile");
        }

        [HttpPost]
        public async Task<bool> CheckEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }

            IEnumerable<UserResponse> users = await _userService.FindUsersAsync(u => u.UserEmail == email);

            return users.Any();
        }

        [HttpPost]
        public async Task<bool> CheckUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return true;
            }

            IEnumerable<UserResponse> users = await _userService.FindUsersAsync(u => u.UserName == username);

            return users.Any();
        }
    }
}
