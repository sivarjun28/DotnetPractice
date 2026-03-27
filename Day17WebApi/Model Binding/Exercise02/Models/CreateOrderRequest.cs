namespace Exercise02.Models
{
    public class CreateOrderRequest
{
    public string CustomerId { get; set; } = string.Empty;
    public List<OrderItemRequest> Items { get; set; } = new();
    public ShippingAddress ShippingAddress { get; set; } = new();
    public PaymentInfo PaymentInfo { get; set; } = new();
}
}