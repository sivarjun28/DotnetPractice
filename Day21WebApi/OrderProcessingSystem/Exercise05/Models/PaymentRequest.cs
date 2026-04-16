namespace Exercise05.Models
{
    public class PaymentRequest
    {
        public string CreditCardNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}