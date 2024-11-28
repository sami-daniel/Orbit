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

    // Constructor to inject dependencies for user service and message service
    public ChatController(IUserService userService, IMessageService messageService)
    {
        _userService = userService;
        _messageService = messageService;
    }

    // Action to render the chat page. Optionally accepts a 'guestname' to initialize a conversation.
    [HttpGet("[controller]/{guestname?}")]
    public async Task<IActionResult> Index([FromRoute] string? guestname)
    {
        // Get the current authenticated user's name (the "host").
        var hostname = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value!;

        // Retrieve the host user details including their followers and followed users.
        var hosts = await _userService.GetAllUserAsync(u => u.UserName == hostname, includeProperties: "Followers,Users");
        var host = hosts.First();

        // If no guestname is provided, return a chat view with just the host.
        if (guestname == null)
        {
            return View(new ChatContext
            {
                Host = host
            });
        }

        // Retrieve the guest user, making sure they are either a follower or followed by the host.
        var participants = await _userService.GetAllUserAsync(u => u.UserName == guestname && (u.Followers.Contains(host) || u.Users.Contains(host)));

        // If no valid participants (i.e., no guests meeting the conditions), return a NotFound result.
        if (!participants.Any())
        {
            return NotFound();
        }

        // Return a chat view with both host and guest (participant) details.
        return View(new ChatContext
        {
            Host = host,
            Guest = participants.First()
        });
    }

    // Action to save a new message. Expects a 'message' object and the sender's username.
    [HttpPost("[controller]/save-message")]
    public async Task<IActionResult> SaveMessage([FromForm] Message message, [FromForm] string from)
    {
        // If the message is null, return a BadRequest response.
        if (message == null)
        {
            return BadRequest();
        }

        // Add the new message to the database through the message service.
        await _messageService.AddMessageAsync(message, from);
        return Ok();
    }

    // Action to load messages between two users. Expects 'username' and 'with' (the conversation partner).
    [HttpGet("[controller]/load-messages")]
    public async Task<IActionResult> LoadMessages([FromQuery] string username, [FromQuery] string with)
    {
        // Retrieve all messages between the given users.
        var messages = await _messageService.GetAllMessagesAsync(username, with);

        // If no messages are found, return an empty list as a JSON response.
        if (!messages.Any())
        {
            return Json(new List<Message>());
        }

        // Return the retrieved messages as a successful response.
        return Ok(messages);
    }
}
