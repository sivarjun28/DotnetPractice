using Microsoft.AspNetCore.Mvc;
using ProductApiExercise01.Models;


namespace ProductApiExercise01.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private static List<Product> products = new()
        {
            new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, Category = "Electronics", Stock = 10 },
            new Product { Id = 2, Name = "Mouse", Description = "Wireless mouse", Price = 25.50m, Category = "Electronics", Stock = 50 },
            new Product { Id = 3, Name = "Desk", Description = "Wooden desk", Price = 299.99m, Category = "Furniture", Stock = 15 }
        };

        private static int nextId = 4;

        // GET api/products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll([FromQuery] string? category)
        {
            if (!string.IsNullOrEmpty(category))
            {
                var filtered = products.Where(p =>
                    p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
                return Ok(filtered);
            }
            return Ok(products);
        }
        //GET api/products/5
        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = products.First(p => p.Id == id);
            if (product == null) return NotFound();
            return Ok(product);
        }
        [HttpGet("search")]
        public ActionResult<IEnumerable<Product>> Search([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name Query is Required");

            var matched = products
                            .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            return Ok(matched);
        }
        //POST api/products
        [HttpPost]
        public ActionResult<Product> Create([FromBody] CreateProductDto productDto)
        {
            if (productDto == null || string.IsNullOrWhiteSpace(productDto.Name))
                return BadRequest("Invalid Product Data");

            var product = new Product
            {
                Id = nextId++,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Category = productDto.Category,
                Stock = productDto.Stock

            };
            products.Add(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        // PUT api/products/5
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UpdateProductDto productDto)
        {
            var product = products.First(p => p.Id == id);
            if (product == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(productDto.Name)) product.Name = productDto.Name;
            if (!string.IsNullOrWhiteSpace(productDto.Description)) product.Description = productDto.Description;
            if (productDto.Price.HasValue) product.Price = productDto.Price.Value;
            if (!string.IsNullOrWhiteSpace(productDto.Category)) product.Category = productDto.Category;
            if (productDto.Stock.HasValue) product.Stock = productDto.Stock.Value;

            return NoContent();
        }

        //Delete
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            products.Remove(product);
            return NoContent();
        }
        //patch
        [HttpPatch("{id}/stock")]
        public ActionResult UpdateStock(int id, [FromBody] int quantity)
        {
            if (quantity < 0) return BadRequest("Quantity cannot be negative.");

            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            product.Stock = quantity;
            return NoContent();
        }

    }
}