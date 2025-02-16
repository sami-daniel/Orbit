﻿using Microsoft.AspNetCore.SignalR;

namespace Orbit.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotification(string user, string message)
    {
        await Clients.User(user).SendAsync("ReceiveNotification", user, message);
    }

}
