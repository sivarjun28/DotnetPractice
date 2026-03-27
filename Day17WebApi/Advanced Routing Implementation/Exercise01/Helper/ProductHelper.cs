using System.Text.Json;
using Exercise01.Models;

namespace Exercise01.Helper
{
    public static class ProductHelper
    {
        public static List<Product> LoadProducts()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "products.json");
            if (!File.Exists(filePath))
                return new List<Product>();
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }
    }
}