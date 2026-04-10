namespace Exercise03.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }

        public string Category { get; set; } = string.Empty;

        public List<Review> Reviews { get; set; } = new();
        public List<Image> Images { get; set; } = new();
    }
}