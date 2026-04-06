using System.Text.Json;

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
            string filePath = "products.json";

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("products.json file not found.");
            }

            string json = File.ReadAllText(filePath)!;
            return JsonSerializer.Deserialize<List<Product>>(json);
        }
    }
}
