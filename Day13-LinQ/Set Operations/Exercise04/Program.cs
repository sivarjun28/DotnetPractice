using System;
namespace Exercise04
{
    internal class Program
    {

        /*
Implement:
1. Skills known by at least one team (Union)
2. Skills common to all teams (Intersect)
3. Skills unique to team1 (Except)
4. Skills that any two teams share
5. Products available in both stores
6. Products only available online
7. Combine all products from both stores (no duplicates)
8. Products with different prices in different stores
*/
        static void Main(string[] args)
        {
            List<string> team1Skills = new() { "C#", "SQL", "Azure", "Git", "Docker" };
            List<string> team2Skills = new() { "Python", "SQL", "AWS", "Git", "Kubernetes" };
            List<string> team3Skills = new() { "Java", "SQL", "Azure", "Git", "Jenkins" };

            List<Product> onlineStore = Product.GetOnlineStoreProducts();
            List<Product> physicalStore = Product.GetPhysicalStoreProducts();

            //1. Skills known by at least one team (Union)
            var skillKnown = team1Skills.Union(team2Skills).Union(team3Skills);
            Console.WriteLine("All skills known by at least one team:");
            foreach (var item in skillKnown)
            {
                System.Console.WriteLine($"{item}");
            }

            //2. Skills common to all teams (Intersect)

            var commonSkills = team1Skills.Intersect(team2Skills).Intersect(team3Skills);
            System.Console.WriteLine("Skills common to all teams (Intersect)");
            foreach (var skill in commonSkills)
            {
                System.Console.WriteLine(skill);
            }
            // 3. Skills unique to team1 (Except)

            var exceptSkills = team1Skills.Except(team2Skills).Except(team3Skills);
            System.Console.WriteLine("Skills unique to team1 (Except)");
            foreach (var skill in exceptSkills)
            {
                System.Console.WriteLine(skill);
            }

            // 4. Skills that any two teams share
            var team1And2 = team1Skills.Intersect(team2Skills);
            var team1And3 = team1Skills.Intersect(team3Skills);
            var team2And3 = team2Skills.Intersect(team3Skills);
            var sharedByAnyTwo = team1And2.Union(team1And3).Union(team2And3).Except(commonSkills);
            Console.WriteLine("\nSkills shared by any two teams:");
            foreach (var skill in sharedByAnyTwo) Console.WriteLine(skill);
            // 5. Products available in both stores
            var productsInBothStores = onlineStore.Intersect(physicalStore, new ProductComparer());
            System.Console.WriteLine("Products available in both stores");
            foreach (var item in productsInBothStores)
            {
                System.Console.WriteLine($"{item.Name} : {item.Category} : {item.Price}");
            }
            // 6. Products only available online
            var availableInOnline = onlineStore.Except(physicalStore, new ProductComparer());
            System.Console.WriteLine("Products only available online");
            foreach (var item in availableInOnline)
            {
                System.Console.WriteLine($"{item.Name} : {item.Category} : {item.Price}");
            }
            // 7. Combine all products from both stores (no duplicates)

            var allProducts = onlineStore.Union(physicalStore, new ProductComparer());
            Console.WriteLine("\nAll products from both stores (no duplicates):");
            foreach (var p in allProducts) Console.WriteLine(p);
            // 8. Products with different prices in different stores

            var diffPriceProducts = onlineStore
                                    .Join(physicalStore,
                                        o => new { o.Name, o.Category },
                                        p => new { p.Name, p.Category },
                                        (o, p) => new { Online = o, Physical = p })
                                        .Where(x => x.Online.Price != x.Physical.Price)
                                        .Select(x => x.Online.Name);
            Console.WriteLine("\nProducts with different prices in stores:");
            foreach (var name in diffPriceProducts) Console.WriteLine(name);

        }


    }

    public class Product
    {
        public string Name { get; set; } = " ";
        public string Category { get; set; } = " ";
        public decimal Price { get; set; }

        public Product() { }
        public Product(string name, string category, decimal price)
        {
            Name = name;
            Category = category;
            Price = price;
        }

        public static List<Product> GetOnlineStoreProducts()
        {
            return new List<Product>
            {
                new Product("Laptop", "Electronics", 1200m),
                new Product("Headphones", "Electronics", 150m),
                new Product("Coffee Maker", "Home Appliances", 80m)
            };
        }

        public static List<Product> GetPhysicalStoreProducts()
        {
            return new List<Product>
            {
                new Product("Smartphone", "Electronics", 800m),
                new Product("Headphones", "Electronics", 10m),
                new Product("Blender", "Home Appliances", 60m),
                new Product("Desk Chair", "Furniture", 200m)
            };
        }

        public override string ToString() => $"{Name} ({Category}) - ${Price}";
    }

    public class ProductComparer : IEqualityComparer<Product>
    {
        public bool Equals(Product? x, Product? y)
        {
            if (x == null || y == null) return false;
            return x.Name == y.Name && x.Category == y.Category;
        }

        public int GetHashCode(Product obj) => HashCode.Combine(obj.Name, obj.Category);
    }
}

