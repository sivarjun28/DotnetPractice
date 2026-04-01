using Exercise03.Exceptions;
using Exercise03.Models;
using Exercise03.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise03.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct);
        Task<Order> GetByIdAsync(int id, CancellationToken ct);
        Task UpdateStatusAsync(int id, string status, string? notes, CancellationToken ct);
        Task CancelOrderAsync(int id, CancellationToken ct);
        Task ProcessPaymentAsync(int id, CancellationToken ct);
    }

    public class OrderService : IOrderService
    {
        private readonly IProductRepository _productRepository;
        private static List<Order> _orders = new List<Order>(); // In-memory order store

        public OrderService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct)
        {
            // 1. Validate customer exists (mock validation)
            if (string.IsNullOrEmpty(request.CustomerId))
            {
                throw new BusinessRuleException("Customer not found.");
            }

            // 2. Check that products exist and have sufficient stock
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId, ct);
                if (product == null)
                {
                    throw new BusinessRuleException($"Product with ID {item.ProductId} does not exist.");
                }

                if (product.Stock < item.Quantity)
                {
                    throw new InsufficientStockException($"Not enough stock for product {item.ProductId}. Available: {product.Stock}, Requested: {item.Quantity}");
                }
            }

            // 3. Create the order (mock)
            var order = new Order
            {
                Id = new Random().Next(1000, 9999), // Simulate generating a new order ID
                CustomerId = request.CustomerId,
                ShippingAddress = request.ShippingAddress,
                PaymentMethod = request.PaymentMethod,
                Status = "Pending", // Set initial status to "Pending"
            };

            // 4. Save the order in the in-memory list
            _orders.Add(order);

            return order;
        }

        public async Task<Order> GetByIdAsync(int id, CancellationToken ct)
        {
            // 1. Retrieve the order by ID from the in-memory list
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                throw new BusinessRuleException("Order not found.");
            }

            return order;
        }

        public async Task UpdateStatusAsync(int id, string status, string? notes, CancellationToken ct)
        {
            // 1. Fetch the order
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                throw new BusinessRuleException("Order not found.");
            }

            // 2. Business rule: Only allow certain status transitions
            if (status == "Cancelled" && (order.Status != "Pending" && order.Status != "Confirmed"))
            {
                throw new BusinessRuleException("Cannot cancel order in current state.");
            }

            // 3. Update the order status (mock logic)
            order.Status = status;
           

            // Save the updated order back to the in-memory list (no actual DB interaction)
        }

        public async Task CancelOrderAsync(int id, CancellationToken ct)
        {
            // 1. Fetch the order
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                throw new BusinessRuleException("Order not found.");
            }

            // 2. Business rule: Cannot cancel an order already shipped or delivered
            if (order.Status == "Shipped" || order.Status == "Delivered")
            {
                throw new BusinessRuleException("Order cannot be cancelled at this stage.");
            }

            // 3. Update order status to "Cancelled"
            order.Status = "Cancelled";

            // 4. Release reserved stock and refund payment (mock logic)
            foreach (var item in order.Items)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId, ct);
                if (product != null)
                {
                    product.Stock += item.Quantity; // Releasing reserved stock
                }
            }
        }

        public async Task ProcessPaymentAsync(int id, CancellationToken ct)
        {
            // 1. Fetch the order
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                throw new BusinessRuleException("Order not found.");
            }

            // 2. Simulate payment processing (mock payment)
            var paymentSuccess = true; // Set this to `false` for a failed payment simulation

            if (!paymentSuccess)
            {
                throw new PaymentException("Payment failed.");
            }

            // 3. Update order status to "Paid" after successful payment
            order.Status = "Paid";

            // In-memory update of the order status (no DB interaction)
        }
    }
}