using Exercise06.Helpers;
using Exercise06.Models;

namespace Exercise06.Services
{
    

    public class ProductService
    {
        private readonly string path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "products.json");

        // Get all products
        public List<Product> GetAll() => JsonHelper.Read<Product>(path);

        // Get a single product
        public Product? Get(int id) => GetAll().FirstOrDefault(p => p.Id == id);

        // Create a new product
        public Product Create(Product product)
        {
            var products = GetAll();
            product.Id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
            products.Add(product);
            JsonHelper.Write(path, products);
            return product;
        }

        // Update product
        public Product Update(Product updated)
        {
            var products = GetAll();
            var product = products.FirstOrDefault(p => p.Id == updated.Id);
            if (product == null) throw new KeyNotFoundException($"Product with Id {updated.Id} not found");

            product.Name = updated.Name;
            product.Price = updated.Price;
            product.Stock = updated.Stock;

            JsonHelper.Write(path, products);
            return product;
        }

        // Delete product
        public bool Delete(int id)
        {
            var products = GetAll();
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;

            products.Remove(product);
            JsonHelper.Write(path, products);
            return true;
        }
    }
}