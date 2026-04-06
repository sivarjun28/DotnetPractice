using Microsoft.AspNetCore.Mvc;
namespace Exercise01.MiddleWare
{

    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Products retrieved successfully" });
        }

        [HttpPost]
        public IActionResult Create([FromBody] object payload)
        {
            return Ok(new { message = "Product created", data = payload });
        }

        [HttpGet("error")]
        public IActionResult ThrowError()
        {
            throw new Exception("Test exception");
        }
    }
}