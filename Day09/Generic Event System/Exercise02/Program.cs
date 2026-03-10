using System;
namespace Exercise02
{
    public abstract class EventArgs
    {
        public DateTime TimeStamp { get; } = DateTime.Now;
        public string EventId { get; } = Guid.NewGuid().ToString();
    }

    public class UserLoggedInEventsArgs : EventArgs
    {
        public required string UserName { get; init; }

        public string IpAddress { get; init; } = string.Empty;
    }

    public class OrderPlacedEventArgs : EventArgs
    {
        public required int OrderId { get; init; }
        public required decimal Total { get; init; }
        public required string CustomerId { get; init; }
    }

    public class EventBus
    {
        private readonly Dictionary<Type, List<Delegate>> subscribers = new();
        public void Subscribe<TEventArgs>(Action<TEventArgs> handler)
            where TEventArgs : EventArgs
        {
            Type eventType = typeof(TEventArgs);

            if (!subscribers.ContainsKey(eventType))
            {
                subscribers[eventType] = new List<Delegate>();
            }
            subscribers[eventType].Add(handler);
            System.Console.WriteLine($"Subscribed to {eventType.Name}");
        }

        public void UnSubscribe<TEventArgs>(Action<TEventArgs> handler)
                where TEventArgs : EventArgs
        {
            Type eventType = typeof(EventArgs);
            if (subscribers.ContainsKey(eventType))
            {
                subscribers[eventType].Remove(handler);
            }
        }
        public void Publish<TEventArgs>(TEventArgs eventArgs)
        where TEventArgs : EventArgs
        {
            Type eventType = typeof(TEventArgs);

            if (!subscribers.ContainsKey(eventType))
            {
                Console.WriteLine($"No subscribers for {eventType.Name}");
                return;
            }

            Console.WriteLine($"\nPublishing {eventType.Name}...");

            foreach (Delegate handler in subscribers[eventType])
            {
                handler.DynamicInvoke(eventArgs);
            }
        }
    }


    public class Logger
    {
        public void OnUserLoggedIn(UserLoggedInEventsArgs e)
        {
            System.Console.WriteLine($"[Log] user {e.UserName} logged in at {e.TimeStamp}");
        }
        public void OrderPlaced(OrderPlacedEventArgs e)
        {
            System.Console.WriteLine($"[Log] Order {e.OrderId} Placed {e.Total:C}");
        }
    }
    public class EmailService
    {
        public void OnUserLoggedIn(UserLoggedInEventsArgs e)
        {
            System.Console.WriteLine($"[Email] sending welcome email to {e.UserName}");
        }

        public void OrderPlaced(OrderPlacedEventArgs e)
        {
            System.Console.WriteLine($"[Email] sending order information for #{e.OrderId}");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            EventBus bus = new();
            Logger logger = new();
            EmailService emailService = new();

            bus.Subscribe<UserLoggedInEventsArgs>(logger.OnUserLoggedIn);
            bus.Subscribe<UserLoggedInEventsArgs>(emailService.OnUserLoggedIn);
            bus.Subscribe<OrderPlacedEventArgs>(logger.OrderPlaced);
            bus.Subscribe<OrderPlacedEventArgs>(logger.OrderPlaced);

            bus.Publish(new UserLoggedInEventsArgs { UserName = "alice", IpAddress = "192.168.1.1" });
            bus.Publish(new OrderPlacedEventArgs { OrderId = 123, Total = 99.99m, CustomerId = "C001" });
        }
    }

}