using Exercise04.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Exercise04.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private static readonly List<Product> Products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 1200 },
        new Product { Id = 2, Name = "Phone", Price = 800 }
    };

        private static readonly List<Sale> Sales = new()
    {
        new Sale { Id = 1, ProductId = 1, Quantity = 3 },
        new Sale { Id = 2, ProductId = 2, Quantity = 5 }
    };
        [HttpGet("products")]
        [Produces("application/json", "application/xml")]
        public IActionResult GetProducts()
        {
            return Ok(Products);
        }

        [HttpGet("products/export")]
        [Produces("application/json", "text/csv")]
        public IActionResult ExportProducts()
        {
            var accept = Request.Headers["Accept"].ToString();
            if (accept.Contains("text/csv"))
            {
                return new ObjectResult(Products)
                {
                    ContentTypes = { "text/csv" }
                };
            }
            return Ok(Products);
        }
        [HttpGet("sales")]
        public IActionResult GetSales([FromQuery] string? format)
        {
            if (format == "csv")
            {
                return new ObjectResult(Sales)
                {
                    ContentTypes = { "text/csv" }
                };
            }
            else if (format == "xml")
            {
                return new ObjectResult(Sales)
                {
                    ContentTypes = { "application/xml" }
                };
            }
            // Default to JSON
            return Ok(Sales);
        }
    }




}