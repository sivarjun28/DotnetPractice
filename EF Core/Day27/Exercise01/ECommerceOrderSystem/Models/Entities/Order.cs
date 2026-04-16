namespace ECommerceOrderSystem.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }

        public int ShippingAddressId { get; set; }
        public int? BillingAddressId { get; set; }

        public Customer Customer { get; set; }
        public Address ShippingAddress { get; set; }
        public Address? BillingAddress { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public Payment? Payment { get; set; }
    }
}