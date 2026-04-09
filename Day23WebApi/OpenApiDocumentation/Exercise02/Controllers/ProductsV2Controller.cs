using System.ComponentModel.DataAnnotations;
using Exercise02.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Exercise02.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [Produces("application/json")]
    public class ProductsV2Controller : ControllerBase
    {
        /// <summary>
        /// Get all products with pagination
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ProductV2Response>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Get all products",
            Description = "Retrieves a paginated list of products",
            OperationId = "GetProducts",
            Tags = new[] { "Products" }
        )]
        public async Task<ActionResult<PagedResponse<ProductV2Response>>> GetAll(
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10)
        {
            var sampleProducts = new List<ProductV2Response>
        {
            new ProductV2Response { Id = 1, Name = "Laptop Pro 15", Price = 999.99m, Category = "Electronics", Stock = 50 }
        };

            var response = new PagedResponse<ProductV2Response>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = 1,
                Items = sampleProducts
            };

            return Ok(response);
        }

        /// <summary>
        /// Get a specific product by ID
        /// </summary>
        /// <param name="id">Product ID</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductV2Response), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get product by ID", OperationId = "GetProductById")]
        public async Task<ActionResult<ProductV2Response>> GetById(int id)
        {
            var product = new ProductV2Response
            {
                Id = id,
                Name = "Laptop Pro 15",
                Price = 999.99m,
                Category = "Electronics",
                Stock = 50
            };

            return Ok(product);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProductV2Response), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create product", Description = "Creates a new product.")]
        public async Task<ActionResult<ProductV2Response>> Create([FromBody] CreateProductV2Request request)
        {
            var createdProduct = new ProductV2Response
            {
                Id = 100,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                Stock = request.Stock,
                Tags = request.Tags
            };

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }
    }
}