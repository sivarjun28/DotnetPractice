namespace ECommerceAPI.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }
        public List<string> Tags { get; set; } = new();
        public List<ProductImage> Images { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}