using Exercise05.Models;

namespace Exercise05.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
        Task RefundAsync(string transactionId);
    }
}