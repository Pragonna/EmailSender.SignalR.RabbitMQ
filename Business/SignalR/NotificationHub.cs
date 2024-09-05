
using Microsoft.AspNetCore.SignalR;

namespace Business.SignalR
{
    public class NotificationHub : Hub<INotificationClient>
    {
        public async Task SendMessageAsync(string message)
        {
            await Clients.Caller.ReceiveMessage(message);
        }
    }
}
