using Exercise01.Models;
using Exercise01.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exercise01.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/products")]
    [ApiVersion("1.0")]
    public class ProductsV1Controller : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsV1Controller(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<ProductV1Response>>> GetAll()
        {
            var products = await _productService.GetAllAsync();

            var response = products.Select(p => new ProductV1Response
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            });
            return Ok(response);
        }
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ProductV1Response>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            var response = new ProductV1Response
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
            return Ok(response);
        }
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ProductV1Response>> Create(CreateProductV1Request request)
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price
            };

            var createdProduct = await _productService.CreateAsync(product);

            var response = new ProductV1Response
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Price = createdProduct.Price
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id, version = "1.0" },
                response
            );
        }

    }
}