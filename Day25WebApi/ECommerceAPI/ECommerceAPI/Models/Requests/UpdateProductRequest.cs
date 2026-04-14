namespace ECommerceAPI.Models.Requests
{
    public class UpdateProductRequest
    {
        public string? Name { get; set; }  // Nullable to allow partial updates
        public string? Description { get; set; }
        public string? Sku { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public int? Stock { get; set; }
        public bool? IsActive { get; set; }
        public List<string>? Tags { get; set; }
    }
}