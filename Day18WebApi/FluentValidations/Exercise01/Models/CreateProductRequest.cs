namespace Exercise01.Models
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Stock { get; set; }
        public List<string> Tags { get; set; } = new();
        public ProductSpecifications? Specifications { get; set; }
    }
}