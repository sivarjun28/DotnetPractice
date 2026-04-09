using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductApi.Models;
using ProductApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                return Ok(products);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products.");
                return StatusCode(500, "Internal server error");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving product with id {id}.");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = await _productService.CreateAsync(createProductDto);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the product.");
                return StatusCode(500, "Internal server error");
            }
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedProduct = await _productService.UpdateAsync(id, updateProductDto);
                if (updatedProduct == null)
                {
                    return NotFound();
                }
                return Ok(updatedProduct);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the product with id {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _productService.DeleteAsync(id);
                if (!success)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the product with id {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            try
            {
                var products = await _productService.SearchAsync(term);
                return Ok(products);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for products.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}