using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Exercise05
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string Category { get; set; } = "";
    }

    public static class ExpressionBuilder
    {
        // Build predicate: product => product.Price > minPrice
        public static Expression<Func<Product, bool>> BuildPriceFilter(decimal minPrice, ParameterExpression param)
        {
            // Property access: product.Price
            MemberExpression property = Expression.Property(param, "Price");

            // Constant expression for minPrice
            ConstantExpression constant = Expression.Constant(minPrice);

            // Comparison: product.Price > minPrice
            BinaryExpression comparison = Expression.GreaterThan(property, constant);

            return Expression.Lambda<Func<Product, bool>>(comparison, param);
        }

        // Build predicate: product => product.Category == category
        public static Expression<Func<Product, bool>> BuildCategoryFilter(string category, ParameterExpression param)
        {
            // Property access: product.Category
            MemberExpression property = Expression.Property(param, "Category");

            // Constant expression for category
            ConstantExpression constant = Expression.Constant(category);

            // Comparison: product.Category == category
            BinaryExpression comparison = Expression.Equal(property, constant);

            return Expression.Lambda<Func<Product, bool>>(comparison, param);
        }

        // Combine two expressions with AND
        public static Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            // Ensure both expressions use the same parameter (expr1.Parameters[0])
            var param = expr1.Parameters[0];

            // Combine the body of both expressions
            var body = Expression.AndAlso(expr1.Body, expr2.Body);

            // Return the combined expression with the same parameter
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new()
            {
                new Product { Id = 1, Name = "Laptop", Price = 999, Category = "Electronics" },
                new Product { Id = 2, Name = "Mouse", Price = 25, Category = "Electronics" },
                new Product { Id = 3, Name = "Desk", Price = 300, Category = "Furniture" },
                new Product { Id = 4, Name = "Chair", Price = 150, Category = "Furniture" },
            };

            // Define a common parameter for both filters
            ParameterExpression param = Expression.Parameter(typeof(Product), "product");

            // Build price filter
            var priceFilter = ExpressionBuilder.BuildPriceFilter(100, param);
            Func<Product, bool> compiledPriceFilter = priceFilter.Compile();
            var expensiveProducts = products.Where(compiledPriceFilter).ToList();

            // Output expensive products
            Console.WriteLine("Products over $100:");
            foreach (var product in expensiveProducts)
            {
                Console.WriteLine($"{product.Name} : $ {product.Price}");
            }

            // Build category filter with correct spelling ("Electronics")
            var categoryFilter = ExpressionBuilder.BuildCategoryFilter("Electronics", param);

            // Combine filters (price > 100 AND category == "Electronics")
            var combinedFilter = ExpressionBuilder.And(priceFilter, categoryFilter);
            Func<Product, bool> compiledCombinedFilter = combinedFilter.Compile();
            var filteredProducts = products.Where(compiledCombinedFilter).ToList();

            // Output filtered products (over $100 and in Electronics category)
            Console.WriteLine($"\nProducts over $100 in Electronics category:");
            foreach (var product in filteredProducts)
            {
                Console.WriteLine($"{product.Name}: $ {product.Price}");
            }

            // Display the expressions
            Console.WriteLine($"\nPrice Filter Expression: {priceFilter}");
            Console.WriteLine($"\nCategory Filter Expression: {categoryFilter}");
            Console.WriteLine($"\nCombined Filter Expression: {combinedFilter}");
        }
    }
}