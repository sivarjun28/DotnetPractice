using Exercise05.Models;

namespace Exercise05.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            var success = request.Amount <= 1000;
            return Task.FromResult(new PaymentResult
            {
                Success = success,
                TransactionId = success ? Guid.NewGuid().ToString() : null,
                ErrorMessage = success ? null : "paymentFailed"
            });
        }

        public Task RefundAsync(string transactionId)
        {
           System.Console.WriteLine($"Refund processed for Transactin {transactionId}");
           return Task.CompletedTask;
        }
    }
}   