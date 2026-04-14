using ECommerceAPI.Data;
using ECommerceAPI.Models.Entities;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<PaymentService> _logger;
        private readonly ApplicationDbContext _context;

        public PaymentService(
            IOrderRepository orderRepository,
            ILogger<PaymentService> logger,
            ApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _context = context;
        }

        public async Task<Payment> ProcessPaymentAsync(Order order, decimal amount, PaymentMethod method)
        {
            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = amount,
                Method = method,
                Status = PaymentStatus.Pending,
                ProcessedAt = DateTime.UtcNow,
                TransactionId = GenerateTransactionId()
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            payment.Status = PaymentStatus.Completed;
            await UpdatePaymentStatusAsync(payment.Id, PaymentStatus.Completed);

            return payment;
        }

        public async Task<bool> UpdatePaymentStatusAsync(int paymentId, PaymentStatus status)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
            {
                return false;
            }

            payment.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Payment?> GetPaymentByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        private string GenerateTransactionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}