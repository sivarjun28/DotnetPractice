namespace ECommerceAPI.Models.Requests
{
    public class AddCartItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}