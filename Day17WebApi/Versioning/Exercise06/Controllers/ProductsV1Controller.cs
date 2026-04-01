using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/v1/products")]
public class ProductsV1Controller : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var products = new[]
        {
            new{Id = 1, Name = "Laptop"},
            new{Id = 2 , Name = "Mobile"}
        };
        return Ok(products);
    }
}