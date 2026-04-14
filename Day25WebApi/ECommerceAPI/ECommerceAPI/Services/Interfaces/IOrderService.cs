using ECommerceAPI.Models.Entities;
using ECommerceAPI.Models.Requests;
using ECommerceAPI.Models.Responses;

namespace ECommerceAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request, int userId);
        Task<OrderResponse?> GetOrderAsync(int orderId, int userId);
        Task<IEnumerable<OrderResponse>> GetUserOrdersAsync(int userId);
        Task<OrderResponse?> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> CancelOrderAsync(int orderId, int userId);
    }
}