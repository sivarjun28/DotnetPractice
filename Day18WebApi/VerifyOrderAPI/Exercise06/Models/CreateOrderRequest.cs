namespace Exercise06.Models
{
    public class CreateOrderRequest
    {
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsExpress { get; set; }
        public string Address { get; set; } = " ";
        public List<OrderItem> Items { get; set; } = new();
    }
}