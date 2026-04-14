using ECommerceAPI.Models.Entities;

namespace ECommerceAPI.Models.Requests
{
    public class CreateOrderRequest
    {
        public List<CreateOrderItemRequest> Items { get; set; } = new List<CreateOrderItemRequest>();
        public Address ShippingAddress { get; set; } = null!;
        public Address BillingAddress { get; set; } = null!;
    }
}