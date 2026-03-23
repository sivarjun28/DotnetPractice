using System;
using System.Linq.Expressions;
namespace Exercise05
{
    public class QueryBuilder<T>
    {
        private IQueryable<T> query;

        public QueryBuilder(IEnumerable<T> data)
        {
            query = data.AsQueryable();
        }

        public QueryBuilder<T> Where(Expression<Func<T, bool>> predicate)
        {
            query = query.Where(predicate);
            return this;
        }

        public QueryBuilder<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            query = query.OrderBy(keySelector);
            return this;
        }

        public QueryBuilder<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            query = query.OrderByDescending(keySelector);
            return this;
        }

        public QueryBuilder<T> Skip(int count)
        {
            query = query.Skip(count);
            return this;
        }

        public QueryBuilder<T> Take(int count)
        {
            query = query.Take(count);
            return this;
        }

        public List<T> ToList()
        {
            return query.ToList();
        }

        public PagedResult<T> ToPagedList(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var totalCount = query.Count();

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public List<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            return query.Select(selector).ToList();
        }
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Stock { get; set; }

        public static List<Product> GetProducts()
        {
            return new List<Product>()
        {
            new Product { Id = 1, Name = "Laptop", Price = 1200, Category = "Electronics", Stock = 10 },
            new Product { Id = 2, Name = "Smartphone", Price = 800, Category = "Electronics", Stock = 25 },
            new Product { Id = 3, Name = "Headphones", Price = 150, Category = "Electronics", Stock = 50 },
            new Product { Id = 4, Name = "Keyboard", Price = 70, Category = "Electronics", Stock = 40 },
            new Product { Id = 5, Name = "Mouse", Price = 40, Category = "Electronics", Stock = 60 },

            new Product { Id = 6, Name = "Office Chair", Price = 200, Category = "Furniture", Stock = 15 },
            new Product { Id = 7, Name = "Desk", Price = 350, Category = "Furniture", Stock = 8 },
            new Product { Id = 8, Name = "Bookshelf", Price = 120, Category = "Furniture", Stock = 20 },

            new Product { Id = 9, Name = "T-Shirt", Price = 25, Category = "Clothing", Stock = 100 },
            new Product { Id = 10, Name = "Jeans", Price = 60, Category = "Clothing", Stock = 70 },
            new Product { Id = 11, Name = "Jacket", Price = 120, Category = "Clothing", Stock = 30 },

            new Product { Id = 12, Name = "Coffee Maker", Price = 90, Category = "Appliances", Stock = 18 },
            new Product { Id = 13, Name = "Blender", Price = 65, Category = "Appliances", Stock = 22 },
            new Product { Id = 14, Name = "Microwave", Price = 150, Category = "Appliances", Stock = 12 },

            new Product { Id = 15, Name = "Notebook", Price = 5, Category = "Stationery", Stock = 200 },
            new Product { Id = 16, Name = "Pen Pack", Price = 10, Category = "Stationery", Stock = 300 },

            new Product { Id = 17, Name = "Gaming Console", Price = 500, Category = "Electronics", Stock = 5 },
            new Product { Id = 18, Name = "Monitor", Price = 300, Category = "Electronics", Stock = 14 },
            new Product { Id = 19, Name = "Tablet", Price = 450, Category = "Electronics", Stock = 9 },
            new Product { Id = 20, Name = "Smartwatch", Price = 220, Category = "Electronics", Stock = 35 }
        };
        }
    }

    class Program
    {
        static void Main()
        {
            var products = Product.GetProducts();

            var queryBuilder = new QueryBuilder<Product>(products);

            // Complex query
            var result = queryBuilder
                .Where(p => p.Category == "Electronics")
                .Where(p => p.Price > 100)
                .OrderByDescending(p => p.Price)
                .Skip(2)
                .Take(5)
                .ToList();

            Console.WriteLine("Filtered Products:");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Name} - {item.Price}");
            }

            // Paginated query
            var paged = new QueryBuilder<Product>(products)
                .Where(p => p.Stock > 0)
                .OrderBy(p => p.Name)
                .ToPagedList(page: 2, pageSize: 5);

            Console.WriteLine("\nPaged Products:");
            foreach (var item in paged.Items)
            {
                Console.WriteLine($"{item.Name}");
            }

            Console.WriteLine($"Page {paged.Page} of {paged.TotalPages}");

            // Projection
            var names = new QueryBuilder<Product>(products)
                .Where(p => p.Price < 50)
                .OrderBy(p => p.Name)
                .Select(p => p.Name); // ❗ no ToList() here

            Console.WriteLine("\nCheap Product Names:");
            foreach (var name in names)
            {
                Console.WriteLine(name);
            }
        }
    }
}

