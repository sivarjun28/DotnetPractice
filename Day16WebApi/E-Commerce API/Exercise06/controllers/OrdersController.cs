using Microsoft.AspNetCore.Mvc;
using Exercise06.Models;
using Exercise06.Services;
using Exercise06.Helpers;

namespace Exercise06.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService service = new ProductService();

        [HttpGet]
        public IActionResult Get() => Ok(service.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = service.Get(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create(Product product) => Ok(service.Create(product));

        [HttpPut("{id}")]
        public IActionResult Update(int id, Product product)
        {
            product.Id = id;
            var updated = service.Update(product);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!service.Delete(id)) return NotFound();
            return NoContent();
        }
    }
}
