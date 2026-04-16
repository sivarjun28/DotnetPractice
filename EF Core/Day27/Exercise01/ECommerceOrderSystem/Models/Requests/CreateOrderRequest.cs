namespace ECommerceOrderSystem.Models.Requests
{
    public class CreateOrderRequest
    {
        public int ShippingAddressId { get; set; }
        public int? BillingAddressId { get; set; }

        public List<OrderItemRequest> Items { get; set; } = new();
    }
}