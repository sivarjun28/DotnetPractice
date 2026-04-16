using Exercise03.Enums;
using Exercise03.Models;

namespace Exercise03.Services
{
    public class PushNotificationService : INotificationService
    {
        public bool CanHandle(NotificationType type) => type == NotificationType.Push;


        public async Task SendAsync(Notification notification)
        {
            using var httpClient = new HttpClient();
            var payload = new
            {
                to = notification.Recipient,
                title = notification.Subject,
                body = notification.Message,
                data = notification.Metadata
            };
            var response = await httpClient.PostAsJsonAsync("https://api.pushprovider.com/send", payload);
            response.EnsureSuccessStatusCode();
        }
    }
}