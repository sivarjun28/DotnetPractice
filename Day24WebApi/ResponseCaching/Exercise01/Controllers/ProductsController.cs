using Exercise01.ConstantValues;
using Exercise01.Models;
using Exercise01.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Exercise01.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }


        /// <summary>
        /// Get all products with caching
        /// </summary>
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
        {
            _logger.LogInformation("GetAll called - will be cached");
            var products = await _productService.GetAllAsync();
            var response = products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();

            var hash = string.Join(",", products.Select(p => p.Id + p.Name + p.Price));
            var eTag = "\"" + hash.GetHashCode().ToString() + "\"";

            if (Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var incomingTag))
            {
                if (incomingTag == eTag)
                {
                    return StatusCode(StatusCodes.Status304NotModified);
                }
            }

            Response.Headers[HeaderNames.ETag] = eTag;
            return Ok(products);
        }

        /// <summary>
        /// Get product by ID with aggressive caching
        /// </summary>
        [HttpGet(RouteConstants.GetId)]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, VaryByHeader = "Accept")]
        public async Task<ActionResult<ProductResponse>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var response = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
            var eTag = GenerateETag(product);

            if (Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var incomingTag))
            {
                if (incomingTag.ToString() == eTag)
                {
                    return StatusCode(StatusCodes.Status304NotModified);
                }
            }
            Response.Headers[HeaderNames.ETag] = eTag;
            return Ok(response);
        }

        private string GenerateETag(Product product)
        {
            var raw = $"{product.Id} - {product.Name} - {product.Price}";

            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(raw));

            return $"\"{Convert.ToBase64String(bytes)}\"";
        }

        /// <summary>
        /// Search products - vary by query string
        /// </summary>
        [HttpGet(RouteConstants.Search)]
        [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "q", "category" })]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> Search(
            [FromQuery] string q,
            [FromQuery] string category
        )
        {
            var products = await _productService.SearchAsync(q, category);
            var response = products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();

            return Ok(response);

        }
        /// <summary>
        /// Create product - no caching, invalidate cache
        /// </summary>
        [HttpPost]
        [ResponseCache(CacheProfileName = "Never")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductResponse>> Create(CreateProductRequest request)
        {

            var product = await _productService.CreateAsync(request);
            var response = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price

            };
            _logger.LogInformation("Product created - cache should refreshed");
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, response);

        }
        /// <summary>
        /// Update product - invalidate specific product cache
        /// </summary>
        [HttpPut(RouteConstants.GetId)]
        [ResponseCache(CacheProfileName = "Never")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponse>> Update(int id, UpdateProductDto dto)
        {
            var product = await _productService.UpdateAsync(id, dto);

            if (product == null)
                return NotFound();

            var response = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            _logger.LogInformation("Product updated - cache should be refreshed");

            return Ok(response);
        }

        /// <summary>
        /// Delete product - invalidate cache
        /// </summary>
        [HttpDelete(RouteConstants.GetId)]
        [ResponseCache(CacheProfileName = "Never")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var deletedProduct = _productService.DeleteAsync(id);
            if (deletedProduct == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Product deleted - related caches should be refreshed");

            return NoContent();
        }

    }
}