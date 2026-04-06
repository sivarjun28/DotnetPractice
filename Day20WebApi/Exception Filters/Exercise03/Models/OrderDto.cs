namespace Exercise03.Models
{

    public class OrderDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public List<OrderItemDto> Items { get; set; } = new();
        public decimal TotalAmount
        {
            get; set;
        }

        
    }   
    public class OrderItemDto
        {
            public string ProductId { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
}