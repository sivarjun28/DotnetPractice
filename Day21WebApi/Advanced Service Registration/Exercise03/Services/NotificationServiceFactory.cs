using Exercise03.Enums;

namespace Exercise03.Services
{
    public class NotificationServiceFactory : INotificationServiceFactory
    {

        private readonly IEnumerable<INotificationService> _services;

        public NotificationServiceFactory(IEnumerable<INotificationService> services)
        {
            _services = services;
        }
        public INotificationService Create(NotificationType type)
        {
            var service = _services.FirstOrDefault(s => s.CanHandle(type));
            if(service == null)
                throw new InvalidOperationException($"No Servie Found for type {type}");
            return service;
        }
    }
}