using ECommerceAPI.Models.Entities;
using ECommerceAPI.Models.Requests;
using ECommerceAPI.Models.Responses;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;

namespace ECommerceAPI.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _cartRepository;

        public ShoppingCartService(IShoppingCartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CartResponse> GetCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            return new CartResponse
            {
                Items = cart?.Items.Select(i => new CartItemResponse
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product?.Price ?? 0
                }).ToList() ?? new List<CartItemResponse>(),
                UpdatedAt = cart?.UpdatedAt ?? DateTime.Now
            };
        }

        public async Task<CartResponse> AddItemAsync(int userId, AddCartItemRequest request)
        {
            var item = new CartItem
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                AddedAt = DateTime.UtcNow
            };
            var cart = await _cartRepository.AddItemToCartAsync(userId, item);
            return new CartResponse
            {
                Items = cart.Items.Select(i => new CartItemResponse
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product?.Price ?? 0
                }).ToList(),
                UpdatedAt = cart.UpdatedAt
            };
        }

        public async Task<CartResponse> UpdateItemAsync(int userId, int itemId, int quantity)
        {
            var cart = await _cartRepository.UpdateCartItemAsync(userId, itemId, quantity);
            return new CartResponse
            {
                Items = cart?.Items.Select(i => new CartItemResponse
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product?.Price ?? 0
                }).ToList() ?? new List<CartItemResponse>(),
                UpdatedAt = cart?.UpdatedAt ?? DateTime.Now
            };
        }

        public async Task<CartResponse> RemoveItemAsync(int userId, int itemId)
        {
            var cart = await _cartRepository.RemoveItemFromCartAsync(userId, itemId);
            return new CartResponse
            {
                Items = cart?.Items.Select(i => new CartItemResponse
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product?.Price ?? 0
                }).ToList() ?? new List<CartItemResponse>(),
                UpdatedAt = cart?.UpdatedAt ?? DateTime.Now
            };
        }

        public async Task ClearCartAsync(int userId)
        {
            await _cartRepository.ClearCartAsync(userId);
        }
    }
}