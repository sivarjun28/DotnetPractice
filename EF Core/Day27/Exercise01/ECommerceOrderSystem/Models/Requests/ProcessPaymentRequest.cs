using ECommerceOrderSystem.Models.Entities;

namespace ECommerceOrderSystem.Models.Requests
{
    public class ProcessPaymentRequest
    {
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; } = string.Empty;
    }
}