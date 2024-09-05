
namespace Business.SignalR
{
    public interface INotificationClient
    {
        Task ReceiveMessage(string message);    
    }
}
