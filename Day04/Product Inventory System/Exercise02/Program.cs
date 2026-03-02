using System;
namespace Exercise02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Inventory inventory = new Inventory();
            inventory.AddProduct(new Product("P001", "Mobile", 6789.55m, 10));
            inventory.AddProduct(new Product("P002", "Laptop", 69789.55m, 20));
            inventory.AddProduct(new Product("P003", "Monitor", 78909.55m, 12));
            inventory.PrintInventoryReport();
        }
    }

    public class Product
    {
        private decimal price;
        private int stock;

        public string Id { set; get; }
        public string Name { set; get; }

        public decimal Price
        {
            get => price;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Price cannot be negative");

                }
                price = value;
            }
        }

        public int Stock => stock;

        public Product(string id, string name, decimal price, int initialStock)
        {
            Id = id;
            Name = name;
            Price = price;
            stock = initialStock;
        }

        //Add Stock
        public void AddStock(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("quantity must be positive");
            stock += quantity;
        }

        public bool RemoveStock(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("quantity must be positive");
            if (quantity > stock)
                return false;
            stock -= quantity;
            return true;
        }

        public decimal GetTotalValue()
        {
            return Price * stock;
        }

        public override string ToString()
        {
            return $"{Id} | {Name,-20} | Price: {Price} | Stock: {Stock} | value: {GetTotalValue():c}";
        }

    }

    public class Inventory
    {
        private Dictionary<string, Product> products = new Dictionary<string, Product>();

        public bool AddProduct(Product product)
        {
            if (products.ContainsKey(product.Id))
                return false;
            products[product.Id] = product;
            return true;
        }

        //Implement RemoveProduct
        public bool RemoveProduct(Product product)
        {
            if (product == null)
                return false;

            products.Remove(product.Id);
            return true;

        }
        //Implement FindProduct by ID
        public Product FindProductById(string id)
        {
            if (id == null)
                return null;
            products.TryGetValue(id, out Product product);
            return product;
        }

        public Product FindProductByName(string name)
        {
            if (name == null)
                return null;
            products.TryGetValue(name, out Product product);
            return product;
        }

        public List<Product> ListAllPrpoducts()
        {
            return products.Values.OrderBy(p => p.Name).ToList();
        }

        public decimal GetTotalInventoryValue()
        {
            return products.Values.Sum(p => p.GetTotalValue());
        }

        public void PrintInventoryReport()
        {
            System.Console.WriteLine("===Inventory Report===");
            System.Console.WriteLine($"Total Products: {products.Count}");
            System.Console.WriteLine($"Total Value: {GetTotalInventoryValue():C}\n");
            foreach(var product in ListAllPrpoducts())
            {
                System.Console.WriteLine(product);
            }
        }
    }
}