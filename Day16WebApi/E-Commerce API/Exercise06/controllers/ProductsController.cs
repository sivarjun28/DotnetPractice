using Microsoft.AspNetCore.Mvc;
using Exercise06.Models;
using Exercise06.Services;
using Exercise06.Helpers;

namespace Exercise06.Controller
{


    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService service = new OrderService();

        [HttpGet]
        public IActionResult Get() => Ok(service.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var order = service.Get(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        public IActionResult GetByCustomer(int customerId) => Ok(service.GetByCustomer(customerId));

        [HttpPost]
        public IActionResult Create(Order order)
        {
            var result = service.Create(order);
            if (result is string) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] string status)
        {
            var updated = service.UpdateStatus(id, status);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult Cancel(int id)
        {
            var result = service.Cancel(id);
            if (result is string) return BadRequest(result);
            return Ok(result);
        }
    }
}