using Exercise03.Enums;
using Exercise03.Models;

namespace Exercise03.Services
{
    public interface INotificationService
    {
        Task SendAsync(Notification notification);
        bool CanHandle(NotificationType type);
    }
}