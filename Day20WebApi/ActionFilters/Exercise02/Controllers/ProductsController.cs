using Exercise02.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Exercise02.Models;
namespace Exercise02.Controllers
{
    [ApiController]
    [Route("api/products")]
    [ValidateModel]
    [LogExecutionTime(500)]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [Cache(60)]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(ProductStore.Products);
        }

        [HttpGet("{id}")]
        [Cache(120)]
        public ActionResult<Product> GetById(int id)
        {
            var product = ProductStore.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Exercise02.Filters.RequireHttps]
        [ServiceFilter(typeof(AuditActionAttribute))]
        public ActionResult<Product> Create(CreateProductDto dto)
        {
            var product = new Product
            {
                Id = ProductStore.Products.Count + 1,
                Name = dto.Name
            };

            ProductStore.Products.Add(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

     
        [HttpDelete("{id}")]
        [Exercise02.Filters.RequireHttps]
        [ServiceFilter(typeof(AuditActionAttribute))]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var product = ProductStore.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            ProductStore.Products.Remove(product);

            return NoContent();
        }
    }
}