namespace Exercise03.Models
{
    public class CreateOrderRequest
    {
        public string CustomerId { get; set; } = string.Empty;
        public List<OrderItemRequest> Items { get; set; } = new();
        public ShippingAddress ShippingAddress { get; set; } = new();
        public string PaymentMethod { get; set; } = string.Empty;
        public string? CouponCode { get; set; }
    }
}