using ECommerceOrderSystem.Data;
using ECommerceOrderSystem.Models.Entities;
using ECommerceOrderSystem.Models.Requests;
using ECommerceOrderSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderSystem.Services.Implementations
{
    public class OrderService : IOrderService
{
    private readonly ECommerceDbContext _context;

    public OrderService(ECommerceDbContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateOrderAsync(int customerId, CreateOrderRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
            throw new Exception("Customer not found");

        var shippingAddress = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == request.ShippingAddressId && a.CustomerId == customerId);

        if (shippingAddress == null)
            throw new Exception("Invalid shipping address");

        Address? billingAddress = null;
        if (request.BillingAddressId.HasValue)
        {
            billingAddress = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == request.BillingAddressId && a.CustomerId == customerId);

            if (billingAddress == null)
                throw new Exception("Invalid billing address");
        }

        var productIds = request.Items.Select(i => i.ProductId).ToList();
        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

        if (products.Count != productIds.Count)
            throw new Exception("One or more products not found");

        var order = new Order
        {
            CustomerId = customerId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            ShippingAddressId = request.ShippingAddressId,
            BillingAddressId = request.BillingAddressId
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(); 

        decimal total = 0;

        foreach (var item in request.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);

            if (product.Stock < item.Quantity)
                throw new Exception($"Insufficient stock for product {product.Name}");

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            };

            total += item.Quantity * product.Price;

            product.Stock -= item.Quantity;

            _context.OrderItems.Add(orderItem);
        }

        order.Total = total;

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return order;
    }

    public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.ShippingAddress)
            .Include(o => o.BillingAddress)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
    {
        var order = await _context.Orders
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
            throw new Exception("Order not found");

        if (newStatus == OrderStatus.Cancelled && order.Status == OrderStatus.Shipped)
            throw new Exception("Cannot cancel a shipped order");

        if (newStatus == OrderStatus.Shipped && order.Payment == null)
            throw new Exception("Cannot ship unpaid order");

        order.Status = newStatus;

        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<Payment> ProcessPaymentAsync(int orderId, ProcessPaymentRequest request)
    {
        var order = await _context.Orders
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
            throw new Exception("Order not found");

        if (order.Payment != null)
            throw new Exception("Payment already exists");

        if (order.Total != request.Amount)
            throw new Exception("Payment amount mismatch");

        var payment = new Payment
        {
            OrderId = orderId,
            Amount = request.Amount,
            Method = request.Method,
            TransactionId = request.TransactionId,
            PaymentDate = DateTime.UtcNow
        };

        _context.Payments.Add(payment);

        order.Status = OrderStatus.Processing;

        await _context.SaveChangesAsync();

        return payment;
    }

    public async Task<List<Order>> GetCustomerOrdersAsync(int customerId)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<decimal> CalculateOrderTotalAsync(int orderId)
    {
        var orderItems = await _context.OrderItems
            .Where(i => i.OrderId == orderId)
            .ToListAsync();

        return orderItems.Sum(i => i.Quantity * i.UnitPrice);
    }
}
}