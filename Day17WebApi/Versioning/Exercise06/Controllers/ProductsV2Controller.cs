using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/v2/products")]
public class ProductsV2Controller : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var products = new[]
        {
            new{Id = 1, Name = "Laptop", Price = 1200},
            new{Id = 2 , Name = "Mobile", Price = 780}
        };
        return Ok(products);
    }
}