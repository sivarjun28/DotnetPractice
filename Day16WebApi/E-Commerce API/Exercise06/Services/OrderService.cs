using Exercise06.Helpers;
using Exercise06.Models;

namespace Exercise06.Services
{
    using Exercise06.Controller;
    using Exercise06.Helpers;

    public class OrderService
    {
        private readonly string path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "orders.json");
        private readonly ProductService productService = new ProductService();

        // Get all orders
        public List<Order> GetAll() => JsonHelper.Read<Order>(path);

        // Get orders by customer
        public List<Order> GetByCustomer(int customerId) => GetAll().Where(o => o.CustomerId == customerId).ToList();

        // Get single order
        public Order? Get(int id) => GetAll().FirstOrDefault(o => o.Id == id);

        // Create new order
        public object Create(Order order)
        {
            var products = productService.GetAll();
            decimal total = 0;

            foreach (var item in order.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null) return $"Product {item.ProductId} not found";
                if (product.Stock < item.Quantity) return $"Not enough stock for {product.Name}";

                product.Stock -= item.Quantity; // decrease stock
                item.Price = product.Price;
                total += item.Price * item.Quantity;
            }

            order.Id = GetAll().Count > 0 ? GetAll().Max(o => o.Id) + 1 : 1;
            order.OrderDate = DateTime.Now;
            order.Status = "Created";
            order.Total = total;

            var orders = GetAll();
            orders.Add(order);

            JsonHelper.Write(path, orders);
            JsonHelper.Write(Path.Combine(Directory.GetCurrentDirectory(), "Data", "products.json"), products);

            return order;
        }

        // Cancel order
        public object Cancel(int id)
        {
            var orders = GetAll();
            var order = orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return "Order not found";
            if (order.Status == "Cancelled") return "Already cancelled";

            var products = productService.GetAll();

            foreach (var item in order.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                    product.Stock += item.Quantity; // restore stock
            }

            order.Status = "Cancelled";

            JsonHelper.Write(path, orders);
            JsonHelper.Write(Path.Combine(Directory.GetCurrentDirectory(), "Data", "products.json"), products);

            return order;
        }

        // Update order status
        public Order UpdateStatus(int id, string status)
        {
            var orders = GetAll();
            var order = orders.FirstOrDefault(o => o.Id == id);
            if (order == null) throw new KeyNotFoundException($"Order with Id {id} not found");


            order.Status = status;
            JsonHelper.Write(path, orders);
            return order;
        }
    }
}