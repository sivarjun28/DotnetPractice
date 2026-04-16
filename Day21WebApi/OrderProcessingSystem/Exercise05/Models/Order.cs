namespace Exercise05.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public List<OrderItem> Items { get; set; }
        public ShippingAddress Address { get; set; }
        public PaymentRequest Payment { get; set; }
    }
}