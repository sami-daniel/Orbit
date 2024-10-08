using Microsoft.AspNetCore.SignalR;

namespace Orbit.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendChatMessage(string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveChatMessage", user, message);
        }
    }
}
