namespace Exercise05.Models
{
    public interface INotificationService
    {
        Task SendOrderConfirmationAsync(Order order);
    }
}