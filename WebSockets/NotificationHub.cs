using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Payroll.ViewModels;

namespace Payroll.WebSockets
{
    public class NotificationHub : Hub
    {
        public async Task SentNotificationToAll(Notification notification)
        {
            await Clients.All.SendAsync("ReceiveMessage", notification);
        }

        public async Task SentNotification(string user, Notification notification)
        {
            await Clients.User(user).SendAsync("ReceiveMessage", notification);
        }
    }
}
