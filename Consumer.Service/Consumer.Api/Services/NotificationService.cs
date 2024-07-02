using Consumer.Api.Controllers;

namespace Consumer.Api.Services
{
    public class NotificationService
    {
        public void NotifyClients(string message)
        {
            NotificationsController.SendNotification(message);
        }
    }
}
