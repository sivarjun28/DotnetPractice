namespace Exercise01.Models
{
    public class UpdateProductV2Request
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Stock { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}