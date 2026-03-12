// See https://aka.ms/new-console-template for more information
/*
Refactor to follow DIP:
1. Create INotificationService interface
2. Implement EmailNotificationService
3. Implement SmsNotificationService
4. Implement PushNotificationService
5. UserService depends on INotificationService
6. Use dependency injection
7. Create composite notification service (send to multiple channels)
*/

INotificationService emailService = new EmailNotificationService();
UserService service1 = new UserService(emailService);
service1.NotifyUser("arjun", "Hello Good morning via Email");

INotificationService smsService = new SmsNotificationService();
UserService service2 = new UserService(smsService);
service2.NotifyUser("siva", "Hello good afternoon via sms");

INotificationService composite = new CompositeNotificationService(new List<INotificationService>
{
    emailService,
    smsService
});
UserService service3 = new UserService(composite);
service3.NotifyUser("user123", "Hello via all channels");
public interface INotificationService
{
    void Send(string recipient, string message);
}

public class EmailNotificationService : INotificationService
{
    public void Send(string recipient, string message)
    {
        System.Console.WriteLine($"Sending email to {recipient}: {message}");
    }
}
public class SmsNotificationService : INotificationService
{
    public void Send(string recipient, string message)
    {
        System.Console.WriteLine($"Sending sms to {recipient}: {message}");
    }
}

public class PushNotificationService : INotificationService
{
    public void Send(string recipient, string message)
    {
        System.Console.WriteLine($"Sending push to {recipient}: {message}");
    }
}

public class UserService
{
    public INotificationService _notification;

    public UserService(INotificationService notification)
    {
        _notification = notification;
    }

    public void NotifyUser(string userId, string message)
    {
        _notification.Send($"{userId}@Gmail.com", message);
    }
}

public class CompositeNotificationService : INotificationService
{
    public readonly IEnumerable<INotificationService> _notificationServices;
    public CompositeNotificationService(IEnumerable<INotificationService> notificationServices)
    {
        _notificationServices = notificationServices;
    }

    public void Send(string recipient, string message)
    {
        foreach (var service in _notificationServices)
        {
            service.Send(recipient, message);
        }
    }
}
