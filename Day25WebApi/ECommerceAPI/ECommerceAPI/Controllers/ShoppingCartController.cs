using System.Security.Claims;
using ECommerceAPI.Models.Requests;
using ECommerceAPI.Models.Responses;
using ECommerceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IShoppingCartService _cartService;

        public CartController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [ResponseCache(Duration = 0, NoStore = true)]
        [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<CartResponse>> GetCart()
        {
            var userId = GetUserId();
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("items")]
        [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CartResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CartResponse>> AddItem(AddCartItemRequest request)
        {
            var userId = GetUserId();
            var cart = await _cartService.AddItemAsync(userId, request);
            return Ok(cart);
        }

        [HttpPut("items/{itemId}")]
        [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CartResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CartResponse>> UpdateItem(int itemId, [FromBody] UpdateCartItemRequest request)
        {
            var userId = GetUserId();
            var cart = await _cartService.UpdateItemAsync(userId, itemId, request.Quantity);
            return Ok(cart);
        }

        [HttpDelete("items/{itemId}")]
        [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartResponse>> RemoveItem(int itemId)
        {
            var userId = GetUserId();
            var cart = await _cartService.RemoveItemAsync(userId, itemId);
            return Ok(cart);
        }

        [HttpPost("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> ClearCart()
        {
            var userId = GetUserId();
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }
    }
}