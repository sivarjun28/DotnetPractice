using ECommerceAPI.Models.Entities;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetCartByUserIdAsync(int userId);
        Task<ShoppingCart> AddItemToCartAsync(int userId, CartItem item);
        Task<ShoppingCart> UpdateCartItemAsync(int userId, int itemId, int quantity);
        Task<ShoppingCart> RemoveItemFromCartAsync(int userId, int itemId);
        Task ClearCartAsync(int userId);
    }
}