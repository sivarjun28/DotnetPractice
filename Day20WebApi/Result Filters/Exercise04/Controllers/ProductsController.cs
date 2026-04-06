using Microsoft.AspNetCore.Mvc;
namespace Exercise04.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = Enumerable.Range(1, 50).Select(i => new
        {
            Id = i,
            Name = $"Product {i}"
        });

        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(int id)
    {
        var product = new
        {
            Id = id,
            Name = $"Product {id}"
        };

        return Ok(product);
    }

    [HttpGet("error")]
    public IActionResult GetError()
    {
        return BadRequest("Something went wrong");
    }
}
}