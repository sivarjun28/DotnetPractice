namespace ECommerceOrderSystem.Models.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; } = string.Empty;

        public Order Order { get; set; } = null!;
    }
}