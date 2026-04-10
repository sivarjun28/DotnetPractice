using Exercise03.Models;

namespace Exercise03.Services
{
    public interface IOrderService
    {
        Task<OrderSummary> GetOrderSummaryAsync(int orderId);
        Task<BulkOperationResult> ProcessBulkOrdersAsync(List<CreateOrderRequest> requests);
    }
}