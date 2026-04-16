using Exercise03.Enums;
using Exercise03.Models;

namespace Exercise03.Services
{
    public class SmsNotificationService : INotificationService
    {
        public bool CanHandle(NotificationType type) => type == NotificationType.Sms;

        public async Task SendAsync(Notification notification)
        {
            using var httpClient = new HttpClient();
            var payload = new
            {
                to = notification.Recipient,
                message = notification.Message
            };

            var response = await httpClient.PostAsJsonAsync("https://api.smsprovider.com/send", payload);
            response.EnsureSuccessStatusCode();
        }
    }
}