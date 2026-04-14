using ECommerceAPI.Data;
using ECommerceAPI.Models.Entities;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if(order == null) return false;
            order.Status = OrderStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {

            return await _context.Orders
                        .Include(o => o.Items)
                        .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders 
                                .Where(o => o.UserId == userId)
                                .Include(o => o.Items)
                                .ToListAsync();
            
        }

        public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
             var order = await _context.Orders.FindAsync(orderId);
             if(order != null)
            {
                order.Status = status;
                await  _context.SaveChangesAsync();
            }
            return order!;
        }
    }
}