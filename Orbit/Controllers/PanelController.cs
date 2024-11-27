using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.DTOs.Responses;
using Orbit.Extensions;
using Orbit.Models;
using Orbit.Services.Interfaces;

namespace Orbit.Controllers;

[Authorize]
public class PanelController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public PanelController(IUserService userService, IMapper mapper)
    {
        _userService = userService; 
        _mapper = mapper;   
    }

    public async Task<IActionResult> Index()
    {
        UserResponse? userResponse = HttpContext.Session.GetObject<UserResponse>("User");
        Claim usr = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
        var user = await _userService.GetAllUserAsync(u => u.UserEmail == usr.Value!, includeProperties: "Followers,Users");
        if (user.FirstOrDefault() == null)
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction("index", "account");
        }

        userResponse = _mapper.Map<User, UserResponse>(user.First());
        HttpContext.Session.SetObject("User", userResponse);
        ViewBag.StateLogin = HttpContext.Session.GetString("is-first-time");
        HttpContext.Session.SetString("is-first-time", false.ToString());

        return View(userResponse);
    }
}
