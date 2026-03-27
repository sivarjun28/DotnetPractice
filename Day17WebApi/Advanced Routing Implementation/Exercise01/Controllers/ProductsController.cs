using Microsoft.AspNetCore.Mvc;
using Exercise01.Models;
using Exercise01.Helper;
using Microsoft.AspNetCore.Mvc.TagHelpers;
namespace Exercise01.Controller

{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private static readonly List<Product> products = ProductHelper.LoadProducts();

        //Get All Products\
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(products);
        }
        //Get By Id
        [HttpGet("{id:int}", Order = 1)]
        public IActionResult GetProductById(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            return product == null ? NotFound() : Ok(product);
        }
        //Get product by SKU (format: XX9999)
        [HttpGet("sku/{sku:sku}")]
        public IActionResult GtBySku(string sku)
        {
            var product = products.FirstOrDefault(p => p.Sku == sku);
            return product == null ? NotFound() : Ok(product);
        }

        //Search products
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string? q, decimal? minPrice, decimal? maxPrice)
        {
            var query = products.AsQueryable();
            if (!string.IsNullOrEmpty(q))
                query = query.Where(p => p.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice);
            return Ok(query.ToList());
        }
        //Category with pagination
        [HttpGet("category/{category}")]
        public IActionResult GetByCategory(string category, int page = 1, int pageSize = 10)
        {
            var result = products
                        .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize);
            return Ok(result);


        }
        //Featured
        [HttpGet("featured")]
        public IActionResult Featured()
        {
            return Ok(products.Where(p => p.IsFeatured));
        }
        // New (last 30 days)
        [HttpGet("new")]
        public IActionResult New()
        {
            var cutOff = DateTime.Now.AddDays(-30);
            return Ok(products.Where(p => p.CreatedDate >= cutOff));
        }
        
        //Date Range
        [HttpGet("range/{start:recentdate}/{end:recentdate}")]
        public IActionResult DateRange(DateTime start, DateTime end)
        {
            var result = products.Where(p => p.CreatedDate >= start && p.CreatedDate <= end);
            return Ok(result);
        }
        // Version-specific route
        [HttpGet("v2/{id:int}")]
        public IActionResult GetV2(int id)
        {
            var product = products.Where(p => p.Id == id);
            return product == null ? NotFound() : Ok(new
            {
                Version = "v2",
                Data = product
            });
        }

        //catch all route
        [HttpGet("{path}")]
        public IActionResult Catch(string path)
        {
            return NotFound(new {message = $"No route Matched: {path}"});
        }

    }
}