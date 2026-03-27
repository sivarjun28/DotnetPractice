namespace Exercise02.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerId { get; set; } = "";
        public List<OrderItemRequest> Items { get; set; } = new();
        public ShippingAddress ShippingAddress { get; set; } = new();
        public PaymentInfo PaymentInfo { get; set; } = new();
        public decimal Total { get; set; }
        public string Status { get; set; } = "";
        public DateTime Date { get; set; }
    }
}