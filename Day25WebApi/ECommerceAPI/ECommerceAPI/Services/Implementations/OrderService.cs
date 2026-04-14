using ECommerceAPI.Models.Entities;
using ECommerceAPI.Models.Requests;
using ECommerceAPI.Models.Responses;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;

namespace ECommerceAPI.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IPaymentService paymentService,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request, int userId)
        {
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {item.ProductId} not found.");
                }

                if (product.Stock < item.Quantity)
                {
                    throw new InvalidOperationException($"Not enough stock for product {item.ProductId}. Available stock: {product.Stock}.");
                }
            }

            decimal subtotal = 0;
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    subtotal += product.Price * item.Quantity;
                }
            }

            decimal tax = subtotal * 0.1m;
            decimal shipping = 10.00m;
            decimal total = subtotal + tax + shipping;

            var order = new Order
            {
                UserId = userId,
                Items = request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList(),
                Subtotal = subtotal,
                Tax = tax,
                Shipping = shipping,
                Total = total,
                Status = OrderStatus.Pending,
                ShippingAddress = request.ShippingAddress,
                BillingAddress = request.BillingAddress,
                OrderDate = DateTime.UtcNow
            };

            order = await _orderRepository.CreateOrderAsync(order);

            var payment = await _paymentService.ProcessPaymentAsync(order, total, PaymentMethod.CreditCard);

            return new OrderResponse
            {
                Id = order.Id,
                Subtotal = order.Subtotal,
                Tax = order.Tax,
                Shipping = order.Shipping,
                Total = order.Total,
                Status = order.Status,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                BillingAddress = order.BillingAddress,
                Items = order.Items.Select(i => new OrderItemResponse
                {
                    ProductId = i.ProductId,
                    ProductName = "Product Name Placeholder",
                    Quantity = i.Quantity,
                    Price = 100
                }).ToList()
            };
        }

        public async Task<OrderResponse?> GetOrderAsync(int orderId, int userId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null || order.UserId != userId)
            {
                return null;
            }

            return new OrderResponse
            {
                Id = order.Id,
                Subtotal = order.Subtotal,
                Tax = order.Tax,
                Shipping = order.Shipping,
                Total = order.Total,
                Status = order.Status,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                BillingAddress = order.BillingAddress,
                Items = order.Items.Select(i => new OrderItemResponse
                {
                    ProductId = i.ProductId,
                    ProductName = "Product Name Placeholder",
                    Quantity = i.Quantity,
                    Price = 100
                }).ToList()
            };
        }

        public async Task<IEnumerable<OrderResponse>> GetUserOrdersAsync(int userId)
        {
            var orders = await _orderRepository.GetUserOrdersAsync(userId);
            return orders.Select(order => new OrderResponse
            {
                Id = order.Id,
                Subtotal = order.Subtotal,
                Tax = order.Tax,
                Shipping = order.Shipping,
                Total = order.Total,
                Status = order.Status,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                BillingAddress = order.BillingAddress,
                Items = order.Items.Select(i => new OrderItemResponse
                {
                    ProductId = i.ProductId,
                    ProductName = "Product Name Placeholder",
                    Quantity = i.Quantity,
                    Price = 100
                }).ToList()
            });
        }

        public async Task<OrderResponse?> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _orderRepository.UpdateOrderStatusAsync(orderId, status);
            if (order == null)
            {
                return null;
            }

            return new OrderResponse
            {
                Id = order.Id,
                Subtotal = order.Subtotal,
                Tax = order.Tax,
                Shipping = order.Shipping,
                Total = order.Total,
                Status = order.Status,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                BillingAddress = order.BillingAddress,
                Items = order.Items.Select(i => new OrderItemResponse
                {
                    ProductId = i.ProductId,
                    ProductName = "Product Name Placeholder",
                    Quantity = i.Quantity,
                    Price = 100
                }).ToList()
            };
        }

        public async Task<bool> CancelOrderAsync(int orderId, int userId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null || order.UserId != userId)
            {
                return false;
            }

            return await _orderRepository.CancelOrderAsync(orderId);
        }
    }
}