using Exercise03.Enums;
using Exercise03.Models;

namespace Exercise03.Services
{
    public class RetryNotificationServiceDecorator : INotificationService
    {
        private readonly INotificationService _inner;
        private readonly int _maxRetries;
        public RetryNotificationServiceDecorator(INotificationService inner, int maxRetries = 3)
        {
            _inner = inner;
            _maxRetries = maxRetries;
        }
        public bool CanHandle(NotificationType type) => _inner.CanHandle(type);


        public async Task SendAsync(Notification notification)
        {
            var attempt = 0;
            while (true)
            {
                try
                {
                    await _inner.SendAsync(notification);
                    return;
                }
                catch
                {
                    attempt++;
                    if (attempt > _maxRetries)
                        throw;

                    var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt));
                    await Task.Delay(delay);
                }
            }
        }
    }
}