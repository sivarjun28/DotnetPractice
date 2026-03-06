using System;
namespace Exercise06
{
    /*
Abstract class: Notification
- Properties: Message, CreatedAt, Priority
- Abstract: Send()
- Virtual: Validate()

Interfaces:
1. ISchedulable: ScheduleFor(dateTime), CancelSchedule()
2. IRetryable: RetryCount, MaxRetries, Retry()
3. ITrackable: TrackingId, Status, GetDeliveryStatus()

Notification types:
1. EmailNotification: Notification, ISchedulable, IRetryable, ITrackable
2. SMSNotification: Notification, IRetryable, ITrackable
3. PushNotification: Notification, ITrackable
4. SlackNotification: Notification, ISchedulable

NotificationService:
- Register notification handlers
- Send notification
- Retry failed notifications
- Track delivery status
*/

    public abstract class Notification
    {
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Priority { get; set; }

        public Notification(string message, DateTime createdAt, int priority)
        {
            Message = message;
            CreatedAt = createdAt;
            Priority = priority;
        }

        public abstract void Send();

        public virtual bool Validate()
        {
            return !string.IsNullOrEmpty(Message);
        }
    }

    public interface ISchedulable
    {
        void ScheduleFor(DateTime dateTime);
        void CancelSchedule();
    }
    public interface IRetryable
    {
        int RetryCount { get; set; }
        int MaxRetries { get; set; }
        void Retry();
    }
    public interface ITrackable
    {
        string TrackingId { get; set; }
        string Status { get; set; }
        void GetDeliveryStatus();
    }

    public class EmailNotification : Notification, ISchedulable, IRetryable, ITrackable
    {
        public int RetryCount { get; set; }
        public int MaxRetries { get; set; }
        public string TrackingId { get; set; }
        public string Status { get; set; }

        public EmailNotification(string message, DateTime createdAt, int priority)
                : base(message, createdAt, priority)
        {
            MaxRetries = 3;
        }

        public override void Send()
        {
            if (Validate())
            {
                System.Console.WriteLine($"Sending mail : {Message}");
                Status = "sent";
            }
            else
            {
                Status = "Failed to send";
                Retry();
            }
        }

        public void Retry()
        {
            if (RetryCount < MaxRetries)
            {
                RetryCount++;
                System.Console.WriteLine($"Retrying email....Attempt {RetryCount}");
                Send();
            }
            else
            {
                System.Console.WriteLine("Maximum retry attempts Reached ");
            }
        }
        public void GetDeliveryStatus()
        {
            System.Console.WriteLine($"Tracking email - status: {Status}, Tracking Id : {TrackingId}");
        }

        public void ScheduleFor(DateTime dateTime)
        {
            System.Console.WriteLine($"Slack message schedule for: {dateTime}");
        }
        public void CancelSchedule()
        {
            System.Console.WriteLine($"Email schedule cancelled");
        }
    }

    public class SMSNotification : Notification, IRetryable, ITrackable
    {
        public int RetryCount { get; set; }
        public int MaxRetries { get; set; }
        public string TrackingId { get; set; }
        public string Status { get; set; }

        public SMSNotification(string message, DateTime createdAt, int priority)
            : base(message, createdAt, priority)
        {
            MaxRetries = 3;
        }
        public override void Send()
        {
            if (Validate())
            {
                System.Console.WriteLine($"Sending mail : {Message}");
                Status = "sent";
            }
            else
            {
                Status = "Failed to send";
                Retry();
            }
        }

        public void Retry()
        {
            if (RetryCount < MaxRetries)
            {
                RetryCount++;
                System.Console.WriteLine($"Retrying email....Attempt {RetryCount}");
                Send();
            }
            else
            {
                System.Console.WriteLine("Maximum retry attempts Reached ");
            }
        }

        public void GetDeliveryStatus()
        {
            Console.WriteLine($"Tracking Push Notification - Status: {Status}, Tracking ID: {TrackingId}");
        }
    }


    public class PushNotification : Notification, ITrackable
    {
        public string TrackingId { get; set; }
        public string Status { get; set; }

        public PushNotification(string message, DateTime createdAt, int priority)
            : base(message, createdAt, priority)
        { }

        public override void Send()
        {
            if (Validate())
            {
                Console.WriteLine($"Sending Push Notification: {Message}");
                Status = "Sent";
            }
            else
            {
                Status = "Failed to Send";
            }
        }

        public void GetDeliveryStatus()
        {
            Console.WriteLine($"Tracking Push Notification - Status: {Status}, Tracking ID: {TrackingId}");
        }
    
    }

    public class SlackNotification :Notification, ISchedulable
    {
        public SlackNotification(string message, DateTime createdAt, int priority)
            : base(message, createdAt, priority)
        { }

        public override void Send()
        {
            if (Validate())
            {
                Console.WriteLine($"Sending Slack message: {Message}");
            }
        }

        public void ScheduleFor(DateTime dateTime)
        {
            Console.WriteLine($"Slack message scheduled for: {dateTime}");
        }

        public void CancelSchedule()
        {
            Console.WriteLine("Slack message schedule canceled.");
        }
    }

    public class NotificationService
    {
        private readonly List<Notification> _notifications = new();
        public void RegisterNotificaton(Notification notification)
        {
            _notifications.Add(notification);
        }

        public void SendAllNotifications()
        {
            foreach (var notification in _notifications)
            {
                notification.Send();
            }
        }
        public void RetryFailedNotification()
        {
            foreach (var notification in _notifications.OfType<IRetryable>().Where(n => ((IRetryable)n).RetryCount < ((IRetryable)n).MaxRetries))
            {
                notification.Retry();
            }
        }

        public void TrackDeliveryStatus()
        {
            foreach (var notification in _notifications.OfType<ITrackable>())
            {
                notification.GetDeliveryStatus();
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            NotificationService notificationService = new NotificationService();

            var email = new EmailNotification("Welcome Email", DateTime.Now, 1);
            var sms = new SMSNotification("SMS Verification", DateTime.Now, 2);
            var push = new PushNotification("Push Notification", DateTime.Now, 3);
            var slack = new SlackNotification("Slack Alert", DateTime.Now, 1);

            notificationService.RegisterNotificaton(email);
            notificationService.RegisterNotificaton(sms);
            notificationService.RegisterNotificaton(push);
            notificationService.RegisterNotificaton(slack);

            notificationService.SendAllNotifications();
            notificationService.RetryFailedNotification();
            notificationService.TrackDeliveryStatus();
        }
    }

    
}

