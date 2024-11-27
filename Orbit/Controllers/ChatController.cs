using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.Models;
using Orbit.Services.Interfaces;

namespace Orbit.Controllers;

[Authorize]
public class ChatController : Controller
{
    private readonly IUserService _userService;
    private readonly IMessageService _messageService;

    public ChatController(IUserService userService, IMessageService messageService)
    {
        _userService = userService;
        _messageService = messageService;
    }

    [HttpGet("[controller]/{guestname?}")]
    public async Task<IActionResult> Index([FromRoute] string? guestname)
    {

        var hostname = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value!;

        var hosts = await _userService.GetAllUserAsync(u => u.UserName == hostname, includeProperties: "Followers,Users");
        var host = hosts.First();

        if (guestname == null)
        {
            return View(new ChatContext
            {
                Host = host
            });
        }

        var participants = await _userService.GetAllUserAsync(u => u.UserName == guestname && (u.Followers.Contains(host) || u.Users.Contains(host)));

        if (!participants.Any())
        {
            return NotFound();
        }

        return View(new ChatContext
        {
            Host = host,
            Guest = participants.First()
        });
    }

    [HttpPost("[controller]/save-message")]
    public async Task<IActionResult> SaveMessage([FromForm] Message message, [FromForm] string from)
    {
        if (message == null)
        {
            return BadRequest();
        }

        await _messageService.AddMessageAsync(message, from);
        return Ok();
    }

    [HttpGet("[controller]/load-messages")]
    public async Task<IActionResult> LoadMessages([FromQuery] string username, [FromQuery] string with)
    {
        var messages = await _messageService.GetAllMessagesAsync(username, with);

        if (!messages.Any())
        {
            return Json(new List<Message>());
        }

        return Ok(messages);
    }
}
