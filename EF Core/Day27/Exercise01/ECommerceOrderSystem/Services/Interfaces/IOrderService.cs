using ECommerceOrderSystem.Models.Entities;
using ECommerceOrderSystem.Models.Requests;

namespace ECommerceOrderSystem.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int customerId, CreateOrderRequest request);
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
        Task<Payment> ProcessPaymentAsync(int orderId, ProcessPaymentRequest request);
        Task<List<Order>> GetCustomerOrdersAsync(int customerId);
        Task<decimal> CalculateOrderTotalAsync(int orderId);
    }
}