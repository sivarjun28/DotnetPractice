using ECommerceAPI.Constants;
using ECommerceAPI.Models.Entities;
using ECommerceAPI.Models.Responses;
using ECommerceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{

    [ApiController]
    [Route("api/v{version:apiVersion}/products")]
    [ApiVersion("2.0")]
    public class ProductsV2Controller : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsV2Controller(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products with pagination (V2)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ProductResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProductResponse>>> GetAllPaged(int page = 1, int pageSize = 10)
        {
            var criteria = new ProductSearchCriteria
            {
                PageNumber = page,
                PageSize = pageSize
            };

            var products = await _productService.SearchAsync(criteria);
            return Ok(products);
        }

        /// <summary>
        /// Get a specific product by its ID with related data (V2)
        /// </summary>
        /// <param name="id">The ID of the product</param>
        [HttpGet(RouteConstants.Id)]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponse>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /// <summary>
        /// Advanced search for products (V2)
        /// </summary>
        [HttpPost(RouteConstants.Search)]
        [ProducesResponseType(typeof(PagedResult<ProductResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProductResponse>>> Search([FromBody] ProductSearchCriteria criteria)
        {
            var products = await _productService.SearchAsync(criteria);
            return Ok(products);
        }
    }
}
