using Exercise03.Enums;
using Exercise03.Services;

public interface INotificationServiceFactory
{
    INotificationService Create(NotificationType type);
}