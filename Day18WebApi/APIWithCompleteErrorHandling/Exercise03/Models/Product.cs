namespace Exercise03.Models
{
    namespace Exercise03.Repository
    {
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Stock { get; set; }  // Available stock for this product
            public decimal Price { get; set; }  // Price of the product
            public string Description { get; set; } = string.Empty;  // Optional description of the product
        }
    }
}