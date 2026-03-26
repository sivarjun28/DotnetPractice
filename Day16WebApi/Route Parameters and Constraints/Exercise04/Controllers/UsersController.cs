using Microsoft.AspNetCore.Mvc;
namespace Exercise04.Controller
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet("{id:int}")]
        public ActionResult GetById(int id)
        {
            return Ok(new { id, message = "Get by ID" });
        }

        [HttpGet("{username:alpha}")]
        public ActionResult GetByUsername(string username)
        {
            return Ok(new { username, message = "Get by username" });
        }

        [HttpGet("valid/{id:int:min(1)}")]
        public ActionResult GetValidId(int id)
        {
            return Ok(new { id, message = "ID is valid (>=1)" });
        }

        [HttpGet("born/{year:int:range(1900,2024)}")]
        public ActionResult GetByBirthYear(int year)
        {
            return Ok(new { year, message = "Birth year route" });
        }

        [HttpGet("{country:alpha:length(2)}/cities")]
        public ActionResult GetCitiesByCountry(string country)
        {
            return Ok(new { country, message = "Cities by country code" });
        }
    }
}