using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Exercise07
{
    // Represents the filtering criteria from user input
    public class FilterCriteria
    {
        public string FieldName { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty; // "eq", "ne", "gt", "lt", "contains"
        public object Value { get; set; } = null!;
    }

    // Dynamic LINQ query builder using Expression Trees
    public class DynamicQueryBuilder<T>
    {
        public IQueryable<T> BuildQuery(
            IQueryable<T> source,
            List<FilterCriteria> filters,
            string? sortField,
            bool sortDescending)
        {
            if (filters != null && filters.Any())
            {
                // Build a combined expression: x => (condition1 && condition2 && ...)
                ParameterExpression param = Expression.Parameter(typeof(T), "x");
                Expression? combined = null;

                foreach (var filter in filters)
                {
                    Expression? exp = BuildExpression(param, filter);
                    if (exp != null)
                    {
                        combined = combined == null ? exp : Expression.AndAlso(combined, exp);
                    }
                }

                if (combined != null)
                {
                    var lambda = Expression.Lambda<Func<T, bool>>(combined, param);
                    source = source.Where(lambda);
                }
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortField))
            {
                source = ApplyOrdering(source, sortField, sortDescending);
            }

            return source;
        }

        private Expression? BuildExpression(ParameterExpression param, FilterCriteria filter)
        {
            try
            {
                // Get the property info
                var property = typeof(T).GetProperty(filter.FieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                    throw new ArgumentException($"Field '{filter.FieldName}' does not exist on type {typeof(T).Name}");

                Expression left = Expression.Property(param, property);
                Expression right = Expression.Constant(Convert.ChangeType(filter.Value, property.PropertyType), property.PropertyType);

                return filter.Operator.ToLower() switch
                {
                    "eq" => Expression.Equal(left, right),
                    "ne" => Expression.NotEqual(left, right),
                    "gt" => Expression.GreaterThan(left, right),
                    "lt" => Expression.LessThan(left, right),
                    "contains" when property.PropertyType == typeof(string) => Expression.Call(left, nameof(string.Contains), null, right),
                    _ => throw new NotSupportedException($"Operator '{filter.Operator}' is not supported or invalid for type {property.PropertyType.Name}")
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error building expression for {filter.FieldName}: {ex.Message}");
                return null;
            }
        }

        private IQueryable<T> ApplyOrdering(IQueryable<T> source, string sortField, bool descending)
        {
            var property = typeof(T).GetProperty(sortField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                throw new ArgumentException($"Sort field '{sortField}' does not exist on type {typeof(T).Name}");

            var param = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(param, property);
            var orderByExp = Expression.Lambda(propertyAccess, param);

            string methodName = descending ? "OrderByDescending" : "OrderBy";
            MethodCallExpression resultExp = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.PropertyType },
                source.Expression,
                Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<T>(resultExp);
        }
    }

    // Example entity class
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    class Program
    {
        static void Main()
        {
            // Sample data
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Apple", Price = 1.2m, Stock = 100 },
                new Product { Id = 2, Name = "Banana", Price = 0.8m, Stock = 50 },
                new Product { Id = 3, Name = "Cherry", Price = 2.5m, Stock = 0 },
                new Product { Id = 4, Name = "Date", Price = 3.0m, Stock = 30 },
                new Product { Id = 5, Name = "Eggplant", Price = 1.5m, Stock = 20 }
            }.AsQueryable();

            // Define filters (simulate user input)
            var filters = new List<FilterCriteria>
            {
                new FilterCriteria { FieldName = "Price", Operator = "gt", Value = 1.0m },
                new FilterCriteria { FieldName = "Name", Operator = "contains", Value = "a" }
            };

            var builder = new DynamicQueryBuilder<Product>();

            // Measure performance
            var sw = Stopwatch.StartNew();
            var query = builder.BuildQuery(products, filters, "Stock", sortDescending: true);
            var result = query.ToList();
            sw.Stop();

            // Display results
            Console.WriteLine("Filtered and sorted products:");
            foreach (var p in result)
            {
                Console.WriteLine($"{p.Name} - Price: {p.Price} - Stock: {p.Stock}");
            }

            Console.WriteLine($"\nQuery executed in {sw.ElapsedMilliseconds} ms");
        }
    }
}
