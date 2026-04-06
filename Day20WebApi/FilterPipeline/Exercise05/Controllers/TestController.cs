using Microsoft.AspNetCore.Mvc;
using Exercise05.Filters.Action;
namespace Exercise05.Controllers
{

[ApiController]
[Route("api/test")]
[ServiceFilter(typeof(FirstActionFilter))]
[ServiceFilter(typeof(SecondActionFilter))]
public class TestController : ControllerBase
{
    [HttpGet]
    [ServiceFilter(typeof(ThirdActionFilter))]
    public IActionResult Get()
    {
        Console.WriteLine("Action executing");
        return Ok("Success");
    }

    [HttpGet("error")]
    public IActionResult Error()
    {
        throw new Exception("Test exception");
    }
}
}