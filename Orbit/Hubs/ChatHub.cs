using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Orbit.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task SendChatMessage(string user, string message)
    {
        await Clients.User(user).SendAsync("ReceiveChatMessage", user, message);
    }
}
