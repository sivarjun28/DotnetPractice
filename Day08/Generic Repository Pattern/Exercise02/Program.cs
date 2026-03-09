using System;
namespace Exercise02
{
    /*
Components:
1. IEntity interface: Id property
2. IRepository<T> interface: CRUD methods
3. InMemoryRepository<T>: Implementation
4. Specification<T>: Filter pattern

Features:
- Add, Update, Delete, GetById, GetAll
- Find with specification (predicate)
- Pagination support
- Sorting support
*/

    public interface IEntity
    {
        int Id { get; set; }
    }

    public interface IRepository<T> where T : IEntity
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        T? GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, bool> predicate);
        IEnumerable<T> GetPage(int pageNumber, int pageSize);
    }

    public class InMemoryRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly List<T> entities = new();
        private int nextId = 1;
        public void Add(T entity)
        {
            entity.Id = nextId++;
            entities.Add(entity);
        }

        public void Update(T entity)
        {
            var index = entities.FindIndex(e => e.Id == entity.Id);
            if (index >= 0)
            {
                entities[index] = entity;
            }
            else
            {
                throw new InvalidOperationException("Entity not found");
            }
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entities.Remove(entity);
            }
            else
            {
                throw new InvalidOperationException("Entity not found");
            }
        }

        public T? GetById(int id)
        {
            return entities.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return entities;
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return entities.Where(predicate);
        }

        public IEnumerable<T> GetPage(int pageNumber, int pageSize)
        {
            return entities
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<T> GetSorted(Func<T, object> sortBy, bool ascending = true)
        {
            return ascending
                ? entities.OrderBy(sortBy)
                : entities.OrderByDescending(sortBy);
        }
    }

    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    public class Customer : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            IRepository<Product> productRepo = new InMemoryRepository<Product>();

            productRepo.Add(new Product { Name = "Laptop", Price = 999.99m, Category = "Electronics" });
            productRepo.Add(new Product { Name = "Mouse", Price = 25.50m, Category = "Electronics" });
            productRepo.Add(new Product { Name = "Desk", Price = 299.99m, Category = "Furniture" });

            // Find expensive products
            var expensive = productRepo.Find(p => p.Price > 100);

            Console.WriteLine("Expensive Products:");
            foreach (var p in expensive)
            {
                Console.WriteLine($"Name: {p.Name}, Price: {p.Price}, Category: {p.Category}");
            }

            // Get page
            var page = productRepo.GetPage(1, 2);

            Console.WriteLine("\nPaged Products:");
            foreach (var p in page)
            {
                Console.WriteLine($"Name: {p.Name}, Price: {p.Price}, Category: {p.Category}");
            }

            // Customer repository
            IRepository<Customer> customerRepo = new InMemoryRepository<Customer>();

            customerRepo.Add(new Customer { Name = "Arjun", Email = "alice@example.com" });

            // Assuming you have a method to get all customers
            var customers = customerRepo.Find(c => true);

            Console.WriteLine("\nCustomers:");
            foreach (var c in customers)
            {
                Console.WriteLine($"Name: {c.Name}, Email: {c.Email}");
            }
        }
    }
}
