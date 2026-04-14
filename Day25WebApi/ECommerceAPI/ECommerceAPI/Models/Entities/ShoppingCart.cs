namespace ECommerceAPI.Models.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public List<CartItem> Items { get; set; } = new();
        public DateTime UpdatedAt { get; set; }
    }
}