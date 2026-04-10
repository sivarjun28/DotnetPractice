using System.Collections.Concurrent;
using Exercise03.Models;
using Exercise03.Repository;

namespace Exercise03.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderSummary> GetOrderSummaryAsync(int orderId)
        {
            var orderTask = _orderRepository.GetByIdAsync(orderId);
            var customerTask = GetCustomerAsync(orderId);
            var itemsTask = GetOrderItemsAsync(orderId);
            var invoiceTask = GetInvoiceAsync(orderId);

            await Task.WhenAll(orderTask, customerTask, itemsTask, invoiceTask);

            return new OrderSummary
            {
                Order = await orderTask,
                Customer = await customerTask,
                Items = await itemsTask,
                Invoice = await invoiceTask
            };
        }

        public async Task<BulkOperationResult> ProcessBulkOrdersAsync(List<CreateOrderRequest> requests)
        {
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            var results = new ConcurrentBag<OrderResult>();

            await Parallel.ForEachAsync(requests, options, async (request, ct) =>
            {
                try
                {
                    var order = await CreateOrderAsync(request);
                    results.Add(new OrderResult { Success = true, OrderId = order.Id });
                }
                catch (Exception ex)
                {
                    results.Add(new OrderResult { Success = false, Error = ex.Message });
                }
            });

            return new BulkOperationResult
            {
                Successful = results.Count(r => r.Success),
                Failed = results.Count(r => !r.Success),
                Results = results.ToList()
            };
        }

        private Task<object> GetCustomerAsync(int id) => Task.FromResult(new object());
        private Task<object> GetOrderItemsAsync(int id) => Task.FromResult(new object());
        private Task<object> GetInvoiceAsync(int id) => Task.FromResult(new object());
        private Task<Order> CreateOrderAsync(CreateOrderRequest r) => Task.FromResult(new Order { Id = 1 });
    }

}