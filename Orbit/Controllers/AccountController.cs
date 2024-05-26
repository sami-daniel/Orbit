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

            return RedirectToAction("", "Account");
        }
    }
}
