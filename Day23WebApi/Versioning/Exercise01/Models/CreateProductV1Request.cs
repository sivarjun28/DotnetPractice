namespace Exercise01.Models
{
    public class CreateProductV1Request
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}