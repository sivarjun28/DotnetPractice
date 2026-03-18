using System;
namespace Exercise02
{


    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        public static List<Customer> GetCustomers()
        {
            return new List<Customer>
    {
        new Customer { Id = 1, Name = "Alice", Email = "alice@example.com", City = "Bangalore" },
        new Customer { Id = 2, Name = "Bob", Email = "bob@example.com", City = "Mumbai" },
        new Customer { Id = 3, Name = "Charlie", Email = "charlie@example.com", City = "Delhi" },
        new Customer { Id = 4, Name = "David", Email = "david@example.com", City = "Chennai" }
    };
        }
    }

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;

        public static List<Order> GetOrders()
        {
            return new List<Order>
    {
        new Order { Id = 1, CustomerId = 1, OrderDate = DateTime.Now.AddDays(-5), Total = 1500, Status = "Completed" },
        new Order { Id = 2, CustomerId = 1, OrderDate = DateTime.Now.AddDays(-2), Total = 2500, Status = "Pending" },
        new Order { Id = 3, CustomerId = 2, OrderDate = DateTime.Now.AddDays(-10), Total = 1000, Status = "Completed" },
        new Order { Id = 4, CustomerId = 3, OrderDate = DateTime.Now.AddDays(-1), Total = 3000, Status = "Shipped" },
        new Order { Id = 5, CustomerId = 4, OrderDate = DateTime.Now.AddDays(-7), Total = 500, Status = "Cancelled" }
    };
        }
    }
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public static List<OrderItem> GetOrderItems()
        {
            return new List<OrderItem>
    {
        new OrderItem { Id = 1, OrderId = 1, Product = "Laptop", Quantity = 1, Price = 1500 },
        new OrderItem { Id = 2, OrderId = 2, Product = "Phone", Quantity = 2, Price = 1250 },
        new OrderItem { Id = 3, OrderId = 3, Product = "Tablet", Quantity = 1, Price = 1000 },
        new OrderItem { Id = 4, OrderId = 4, Product = "Monitor", Quantity = 2, Price = 1500 },
        new OrderItem { Id = 5, OrderId = 5, Product = "Keyboard", Quantity = 1, Price = 500 }
    };
        }
    }
    /*
Implement joins:
1. Inner join: Customers with their orders
2. Left join: All customers (show those without orders)
3. Group join: Customers with order count and total spent
4. Three-way join: Customers -> Orders -> OrderItems
5. Self join: Orders placed on same date
6. Cross join: All customer-product combinations
7. Join with filtering: Customers with completed orders only
8. Join with grouping: Customer order summary by month
*/
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Customer> customers = Customer.GetCustomers();
            List<Order> orders = Order.GetOrders();
            List<OrderItem> orderItems = OrderItem.GetOrderItems();
            //1. Inner join: Customers with their orders

            var CustomerOrders1 = from c in customers
                                  join o in orders on c.Id equals o.CustomerId
                                  select new
                                  {
                                      c.Name,
                                      c.Email,
                                      o.OrderDate,
                                      o.Total
                                  };
            //method Syntax
            var CustomerOrders2 = customers.Join(orders,
                                        c => c.Id,
                                        o => o.CustomerId,
                                        (c, o) => new { c.Name, o.OrderDate, o.Total });
            foreach (var item in CustomerOrders2)
            {
                System.Console.WriteLine($"{item.Name}: {item.OrderDate} : {item.Total}");
            }

            //2. Left join: All customers (show those without orders)
            var CustomerWithoutOrders = from c in customers
                                        join o in orders on c.Id equals o.CustomerId into customerOrders
                                        from o in customerOrders.DefaultIfEmpty()
                                        select new
                                        {
                                            c.Name,
                                            OrderId = o?.Id ?? 0,
                                            Total = o?.Total ?? 0

                                        };
            foreach (var item in CustomerWithoutOrders)
            {
                System.Console.WriteLine($"{item.Name} | {item.OrderId} | {item.Total}");
            }

            //Group join: Customers with order count and total spent
            System.Console.WriteLine("--------------------------------------");
            var customerWithTotalSpent = from c in customers
                                         join o in orders on c.Id equals o.CustomerId into customerOrders
                                         select new
                                         {
                                             c.Name,
                                             OrderCount = customerOrders.Count(),
                                             TotalSpent = customerOrders.Sum(x => x.Total)
                                         };
            foreach (var item in customerWithTotalSpent)
            {
                System.Console.WriteLine($"{item.Name} | {item.OrderCount} orders | {item.TotalSpent}");
            }

            //Three-way join: Customers -> Orders -> OrderItems
            System.Console.WriteLine("-------------------------------------------");
            var threewayJoin = from c in customers
                               join o in orders on c.Id equals o.CustomerId
                               join oi in orderItems on o.Id equals oi.OrderId
                               select new
                               {
                                   CustomerName = c.Name,
                                   OrderId = c.Id,
                                   Product = oi.Product,
                                   Quantity = oi.Quantity,
                                   Price = oi.Price

                               };
            foreach (var item in threewayJoin)
            {
                Console.WriteLine($"{item.CustomerName} | Order: {item.OrderId} | Product: {item.Product} | Qty: {item.Quantity} | Price: {item.Price}");
            }

            //5. Self join: Orders placed on same date
            System.Console.WriteLine("---------------------------------------------");
            var selfJoin =
                        from o1 in orders
                        join o2 in orders
                            on o1.OrderDate.Date equals o2.OrderDate.Date
                        where o1.Id < o2.Id // avoid duplicates & self-pairing
                        select new
                        {
                            Date = o1.OrderDate.Date,
                            Order1 = o1.Id,
                            Order2 = o2.Id
                        };

            foreach (var item in selfJoin)
            {
                Console.WriteLine($"Date: {item.Date.ToShortDateString()} | Orders: {item.Order1} & {item.Order2}");
            }

            //Cross join: All customer-product combinations
            System.Console.WriteLine("-----------------------------------------");

            var crossJoin =
                        from c in customers
                        from oi in orderItems
                        select new
                        {
                            CustomerName = c.Name,
                            Product = oi.Product
                        };
            foreach (var item in crossJoin)
            {
                Console.WriteLine($"{item.CustomerName} - {item.Product}");
            }
            System.Console.WriteLine("-------------------------------------------------------");
            //7. Join with Filtering (Customers with Completed Orders)
            var completedOrders = from c in customers
                                  join o in orders on c.Id equals o.CustomerId
                                  where o.Status == "Completed"
                                  select new
                                  {
                                      CustomerName = c.Name,
                                      OrderId = o.Id,
                                      Total = o.Total
                                  };
            foreach (var item in completedOrders)
            {
                System.Console.WriteLine($"{item.CustomerName} - {item.OrderId} - {item.Total}");
            }
            //8. Join with grouping: Customer order summary by month
            System.Console.WriteLine("---------------------------------------------------------");
            var monthlySummary =
        from c in customers
        join o in orders on c.Id equals o.CustomerId
        group o by new
        {
            c.Name,
            Month = o.OrderDate.Month,
            Year = o.OrderDate.Year
        } into g
        select new
        {
            CustomerName = g.Key.Name,
            Month = g.Key.Month,
            Year = g.Key.Year,
            TotalOrders = g.Count(),
            TotalAmount = g.Sum(x => x.Total)
        };

            foreach (var item in monthlySummary)
            {
                Console.WriteLine($"{item.CustomerName} - {item.Month}/{item.Year} : {item.TotalOrders} orders, Total = {item.TotalAmount}");
            }

        }
    }
}