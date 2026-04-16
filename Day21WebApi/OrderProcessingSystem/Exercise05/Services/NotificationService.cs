using Exercise05.Models;

namespace Exercise05.Services
{
    public class NotificationService : INotificationService
    {
        public Task SendOrderConfirmationAsync(Order order)
        {
            Console.WriteLine($"Order {order.OrderId} confirmed and notification sent to customer.");
            return Task.CompletedTask;
        }
    }
}