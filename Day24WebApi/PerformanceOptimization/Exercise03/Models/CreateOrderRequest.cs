namespace Exercise03.Models
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public List<int> ProductIds { get; set; } = new();
    }
}