namespace Exercise02.Models
{
    public class UpdateOrderRequest
    {
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public ShippingAddress? NewShippingAddress { get; set; }
    }
}
