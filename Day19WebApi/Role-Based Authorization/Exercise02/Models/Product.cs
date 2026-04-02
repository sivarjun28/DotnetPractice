namespace Exercise02.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = " ";
        public decimal Price { get; set; }
        public int Stock { get; set; }


        public static List<Product> GenerateSampleProducts()
        {
            return new List<Product>
    {
        new Product { Id = 1, Name = "Product 1", Price = 10, Stock = 100 },
        new Product { Id = 2, Name = "Product 2", Price = 20, Stock = 200 }
    };
        }
    }
}
