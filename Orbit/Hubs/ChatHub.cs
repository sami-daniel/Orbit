using Microsoft.AspNetCore.SignalR;

namespace Orbit.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string userChecker, string message)
        {
            await Clients.User(user).SendAsync("ReceiveMessage", user, userChecker, message);
        }
    }
}
