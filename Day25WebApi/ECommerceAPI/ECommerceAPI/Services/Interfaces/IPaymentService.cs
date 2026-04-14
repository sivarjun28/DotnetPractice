using ECommerceAPI.Models.Entities;

namespace ECommerceAPI.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> ProcessPaymentAsync(Order order, decimal amount, PaymentMethod method);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, PaymentStatus status);
        Task<Payment?> GetPaymentByOrderIdAsync(int orderId);
    }
}