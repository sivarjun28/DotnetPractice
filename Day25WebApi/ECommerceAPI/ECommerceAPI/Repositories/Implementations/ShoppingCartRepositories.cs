using ECommerceAPI.Data;
using ECommerceAPI.Models.Entities;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories.Implementations
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart?> GetCartByUserIdAsync(int userId)
        {
            return await _context.ShoppingCarts.Include(c => c.Items)
                                               .ThenInclude(i => i.Product)
                                               .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<ShoppingCart> AddItemToCartAsync(int userId, CartItem item)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId, Items = new List<CartItem>() };
                _context.ShoppingCarts.Add(cart);
            }
            cart.Items.Add(item);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<ShoppingCart> UpdateCartItemAsync(int userId, int itemId, int quantity)
        {
            var cart = await GetCartByUserIdAsync(userId);
            var item = cart?.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                item.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
            return cart;
        }

        public async Task<ShoppingCart> RemoveItemFromCartAsync(int userId, int itemId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            var item = cart?.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                cart.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            return cart;
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                cart.Items.Clear();
                await _context.SaveChangesAsync();
            }
        }
    }
}