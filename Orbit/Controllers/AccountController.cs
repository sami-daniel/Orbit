using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Application.Interfaces;
using Orbit.Domain.Entities;
using Orbit.DTOs.Requests;
using Orbit.Extensions;

namespace Orbit.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public AccountController(IMapper mapper, IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("create-user")]
        public async Task<IActionResult> CreateUser([FromForm] UserAddRequest userAddRequest)
        {
            if (!ModelState.IsValid && !_webHostEnvironment.IsDevelopment())
            {
                IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return RedirectToAction("Index", new
                {
                    modalActive = true,
                    userEmailError = errors.FirstOrDefault(e => e.Contains("email")),
                    userNameError = errors.FirstOrDefault(e => e.Contains("nome do usuário")),
                    userProfileError = errors.FirstOrDefault(e => e.Contains("nome de perfil")),
                    userPasswordError = errors.FirstOrDefault(e => e.Contains("senha")),
                    formID = 2
                });
            }
            else if (!ModelState.IsValid && _webHostEnvironment.IsDevelopment())
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(userAddRequest);

            await _userService.AddUserAsync(user);

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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] string? inputLogin, [FromForm] string? password)
        {
            // ERRATA: O atributo email pode assumir dois valores - email ou username
            // porem, para o model binder realizar a vinculação de dados, o nome do
            // parametro não pode ser diferente do atributo name do campo do formulario
            // na página, então o nome do parametro permanece inalterado, assumindo sua
            // dupla função

            if (inputLogin == null || password == null)
            {
                return RedirectToAction("Index", new { modalActive = true, errorMessage = "Usuário ou senha inválidos!", formID = 1 });
            }

            var user = await _userService.GetUserByIdentifierAsync(inputLogin);

            if (user == null || password != user.UserPassword)
            {
                return RedirectToAction("Index", new { modalActive = true, errorMessage = "Usuário ou senha inválidos!", formID = 1 });
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
        [Route("log-out")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
