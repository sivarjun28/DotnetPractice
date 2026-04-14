namespace ECommerceAPI.Models.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
        public string? AltText { get; set; }

        public bool IsPrimary { get; set; } = false;
        public int SortOrder { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}