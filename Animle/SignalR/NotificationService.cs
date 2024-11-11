using Animle.Models;
using Animle.SignalR;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;

public class NotificationService
{
    private readonly IHubContext<SignarlRHub> _hubContext;

    public NotificationService(IHubContext<SignarlRHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendGlobalNotification(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
    }


    public async Task SendMessageToTheUser(string clientId, string action, object message)
    {
        await _hubContext.Clients.Client(clientId).SendAsync(action, message);
    }

}