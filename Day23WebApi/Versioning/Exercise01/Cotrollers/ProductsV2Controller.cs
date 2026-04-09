using Exercise01.Models;
using Exercise01.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Exercise01.Controllers
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

        [HttpGet]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult<PagedResponse<ProductV2Response>>> GetAll(
                        [FromQuery] int page = 1,
                        [FromQuery] int pageSize = 10)
        {
            var allProducts = (await _productService.GetAllAsync()).ToList();

            var totalCount = allProducts.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedData = allProducts
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Select(p => MapToV2Response(p))
                            .ToList();
            var response = new PagedResponse<ProductV2Response>
            {
                Data = pagedData,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
            return Ok(response);
        }
        [HttpGet("{id}")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult<ProductV2Response>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(MapToV2Response(product));
        }
        [HttpPost]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult<ProductV2Response>> Create(CreateProductV2Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Name is required");

            if (request.Price <= 0)
                return BadRequest("Price must be greater than 0");

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                Stock = request.Stock,
                Tags = request.Tags,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _productService.CreateAsync(product);

            var response = MapToV2Response(created);

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id, version = "2.0" },
                response
            );
        }

        [HttpPut("{id}")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult<ProductV2Response>> Update(int id, UpdateProductV2Request request)
        {
            var existing = await _productService.GetByIdAsync(id);

            if (existing == null)
                return NotFound();
            existing.Name = request.Name;
            existing.Description = request.Description;
            existing.Price = request.Price;
            existing.Category = request.Category;
            existing.Stock = request.Stock;
            existing.Tags = request.Tags;
            existing.UpdatedAt = DateTime.UtcNow;

            await _productService.UpdateAsync(existing);
            return Ok(MapToV2Response(existing));
        }
        [HttpPatch("{id}")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult<ProductV2Response>> PartialUpdate(
        int id,
        [FromBody] JsonPatchDocument<UpdateProductV2Request> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            var updateDto = new UpdateProductV2Request
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                Stock = product.Stock,
                Tags = product.Tags
            };

            patchDoc.ApplyTo(updateDto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            product.Name = updateDto.Name;
            product.Description = updateDto.Description;
            product.Price = updateDto.Price;
            product.Category = updateDto.Category;
            product.Stock = updateDto.Stock;
            product.Tags = updateDto.Tags;
            product.UpdatedAt = DateTime.UtcNow;

            await _productService.UpdateAsync(product);

            return Ok(MapToV2Response(product));
        }

        private static ProductV2Response MapToV2Response(Product p)
        {
            return new ProductV2Response
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description ?? string.Empty,
                Price = p.Price,
                Category = p.Category ?? string.Empty,
                Stock = p.Stock,
                Tags = p.Tags ?? new List<string>(),
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            };
        }

    }
}