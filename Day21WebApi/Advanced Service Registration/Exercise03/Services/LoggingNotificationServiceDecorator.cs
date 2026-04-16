using Exercise03.Enums;
using Exercise03.Models;

namespace Exercise03.Services
{
    public class LoggingNotificationServiceDecorator : INotificationService
    {
        private readonly INotificationService _inner;
        private readonly ILogger<LoggingNotificationServiceDecorator> _logger;

        public LoggingNotificationServiceDecorator(
            INotificationService inner,
            ILogger<LoggingNotificationServiceDecorator> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public bool CanHandle(NotificationType type) => _inner.CanHandle(type);

        public async Task SendAsync(Notification notification)
        {
            try
            {
                _logger.LogInformation("Sending {Type} notification to {Recipient}", notification.Type, notification.Recipient);

                await _inner.SendAsync(notification);

                _logger.LogInformation("Successfully sent {Type} notification to {Recipient}", notification.Type, notification.Recipient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send {Type} notification to {Recipient}", notification.Type, notification.Recipient);
                throw;
            }
        }
    }
}