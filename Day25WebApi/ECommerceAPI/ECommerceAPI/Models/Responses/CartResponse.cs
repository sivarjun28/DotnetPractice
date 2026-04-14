namespace ECommerceAPI.Models.Responses
{
    public class CartResponse
    {
        public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();
        public DateTime UpdatedAt { get; set; }
    }
}