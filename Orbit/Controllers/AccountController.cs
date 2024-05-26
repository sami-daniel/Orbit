using AutoMenu.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Orbit.Application.Dtos.Requests;
using Orbit.Application.Dtos.Responses;
using Orbit.Application.Interfaces;
using System.Security.Claims;

namespace Orbit.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISession _session;

        public AccountController(IUserService userService, IWebHostEnvironment webHostEnvironment, ISession session)
        {
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _session = session;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateUser(UserAddRequest userAddRequest)
        {
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

            _session.SetObject("User", userReponse);
            
            return RedirectToAction("", "Account");
        }
    }
}
