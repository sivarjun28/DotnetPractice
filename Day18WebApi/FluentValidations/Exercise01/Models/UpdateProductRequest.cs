namespace Exercise01.Models
{
    public class UpdateProductRequest
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int? Stock { get; set; }
        public List<string>? Tags { get; set; }
    }
}