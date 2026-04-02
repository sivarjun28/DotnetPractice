using Exercise02.Dtos;
using Exercise02.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exercise02.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private static List<Product> _products = Product.GenerateSampleProducts();
        private static int _nextId = _products.Max(p => p.Id) + 1;

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,User")]
        public ActionResult<IEnumerable<Product>> GetAll() => Ok(_products);

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,User")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult<Product> Create(CreateProductDto dto)
        {
            var product = new Product
            {
                Id = _nextId++,
                Name = dto.Name,
                Price = dto.Price,
                Stock = dto.Stock
            };
            _products.Add(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult<Product> Update(int id, UpdateProductDto dto)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Stock = dto.Stock;

            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            _products.Remove(product);
            return NoContent();
        }

        [HttpPost("bulk-delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult BulkDelete([FromBody] List<int> ids)
        {
            _products.RemoveAll(p => ids.Contains(p.Id));
            return NoContent();
        }
    }
}