using ECommerceAPI.Models.Requests;
using ECommerceAPI.Models.Responses;

namespace ECommerceAPI.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task<CartResponse> GetCartAsync(int userId);
        Task<CartResponse> AddItemAsync(int userId, AddCartItemRequest request);
        Task<CartResponse> UpdateItemAsync(int userId, int itemId, int quantity);
        Task<CartResponse> RemoveItemAsync(int userId, int itemId);
        Task ClearCartAsync(int userId);
    }
}