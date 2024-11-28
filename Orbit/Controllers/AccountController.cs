using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Orbit.DTOs.Requests;
using Orbit.DTOs.Responses;
using Orbit.Extensions;
using Orbit.Filters;
using Orbit.Models;
using Orbit.Services.Exceptions;
using Orbit.Services.Interfaces;

namespace Orbit.Controllers;

[EnsureUserNotCreated]  // Custom attribute ensuring the user is not already created
public class AccountController : Controller
{
    private readonly IUserService _userService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;

    // Constructor that injects services for user management, environment, and mapping
    public AccountController(IMapper mapper, IUserService userService, IWebHostEnvironment webHostEnvironment)
    {
        _mapper = mapper;
        _userService = userService;
        _webHostEnvironment = webHostEnvironment;
    }

    // Action that displays the login or registration form
    [HttpGet]
    [Route("[controller]")]
    public IActionResult Index(bool? modalActive,
                               string? generalError,
                               string? errorMessage,
                               int formID,
                               string? userEmailError,
                               string? userNameError,
                               string? userProfileError,
                               string? userPasswordError)
    {
        // If formID is 1, it means the login form is being displayed
        if (formID == 1)
        {
            ViewBag.LoginModalActive = modalActive;
            ViewBag.LoginError = errorMessage;

            return View();
        }

        // If formID is 2, it means the registration form is being displayed
        ViewBag.RegisModalActive = modalActive;
        ViewBag.UserEmailError = userEmailError;
        ViewBag.UserNameError = userNameError;
        ViewBag.UserProfileError = userProfileError;
        ViewBag.UserPasswordError = userPasswordError;
        ViewBag.PartNumber = formID;

        return View(new UserAddRequest());  // Empty UserAddRequest for the registration form
    }

    // Action to create a new user
    [HttpPost("[controller]/create-user")]
    public async Task<IActionResult> CreateUser([FromForm] UserAddRequest userAddRequest)
    {
        // If the model state is invalid (data not valid)
        if (!ModelState.IsValid && !_webHostEnvironment.IsDevelopment())
        {
            // In non-development environments, redirect with validation errors
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
            // In development, return BadRequest with the model state errors
            return BadRequest(ModelState);
        }

        var user = _mapper.Map<User>(userAddRequest);  // Map the UserAddRequest to a User model

        try
        {
            await _userService.AddUserAsync(user);  // Add the new user to the database
        }
        catch (UserAlredyExistsException ex)
        when (ex.Message.Contains(user.UserEmail, StringComparison.CurrentCultureIgnoreCase))
        {
            // If the email already exists, redirect with the error message
            ViewBag.UserEmailError = ex.Message;
            return RedirectToAction("index", new
            {
                modalActive = true,
                userEmailError = ex.Message,
                formID = 2
            });
        }
        catch (UserAlredyExistsException ex)
        when (ex.Message.Contains(user.UserName, StringComparison.CurrentCultureIgnoreCase))
        {
            // If the username already exists, redirect with the error message
            ViewBag.UserError = ex.Message;
            return RedirectToAction("index", new
            {
                modalActive = true,
                userNameError = ex.Message,
                formID = 2
            });
        }
        catch (Exception ex)
        {
            // If there's any other error, redirect with a general error message
            return RedirectToAction("index", new
            {
                modalActive = true,
                generalError = ex.Message
            });
        }

        // Create claims for the user after successful registration
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
        
        // Sign in the user using the cookie authentication
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

        // Store the user object in the session and mark as a first-time login
        HttpContext.Session.SetObject("User", _mapper.Map<User, UserResponse>(user));
        HttpContext.Session.SetString("is-first-time", bool.TrueString);

        return RedirectToActionPermanent("", "panel");  // Redirect to the panel after successful registration
    }

    // Action for user login
    [HttpPost("[controller]/login")]
    public async Task<IActionResult> Login([FromForm] string? inputLogin, [FromForm] string? password)
    {
        // Check if login or password is null and redirect with error message
        if (inputLogin == null || password == null)
        {
            return RedirectToAction("index", new { modalActive = true, errorMessage = "Usuário ou senha inválidos!", formID = 1 });
        }

        var user = await _userService.GetUserByIdentifierAsync(inputLogin);  // Fetch user by email or username

        // If user doesn't exist or password is incorrect, redirect with error message
        if (user == null || password != user.UserPassword)
        {
            return RedirectToAction("index", new { modalActive = true, errorMessage = "Usuário ou senha inválidos!", formID = 1 });
        }

        // Create claims for the logged-in user
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

        // Sign in the user using the cookie authentication
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

        // Store the user object in the session and mark as a returning user
        HttpContext.Session.SetObject("User", _mapper.Map<User, UserResponse>(user));
        HttpContext.Session.SetString("is-first-time", bool.FalseString);

        return RedirectToActionPermanent("", "panel");  // Redirect to the panel after successful login
    }

    // Action for user logout
    [HttpGet("[controller]/log-out")]
    public async Task<IActionResult> LogOut()
    {
        // Sign out the user
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("index");  // Redirect to the index (login) page
    }

    // Action to validate if a username or email is already taken
    [HttpPost("[controller]/validate-user")]
    public async Task<IActionResult> ValidateUser(string uid)
    {
        var user = await _userService.GetUserByIdentifierAsync(uid);  // Check if the user already exists
        if (user == null)
        {
            return Json(true);  // If the user does not exist, return true (valid)
        }

        return Json(false);  // If the user exists, return false (invalid)
    }
}
