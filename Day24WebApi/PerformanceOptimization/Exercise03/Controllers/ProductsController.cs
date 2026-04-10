using System.Runtime.CompilerServices;
using Exercise03.Models;
using Exercise03.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exercise03.Controllers
{
   [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("stream")]
        public async IAsyncEnumerable<Product> StreamProducts(
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllAsync();

            foreach (var product in products)
            {
                if (cancellationToken.IsCancellationRequested)
                    yield break;

                yield return product;
                await Task.Delay(10, cancellationToken);
            }
        }

        [HttpGet("export")]
        public async Task<IActionResult> Export()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            await writer.WriteLineAsync("Id,Name,Price,Stock");

            await foreach (var product in GetProductsAsync())
            {
                await writer.WriteLineAsync($"{product.Id},{product.Name},{product.Price},{product.Stock}");
            }

            await writer.FlushAsync();
            stream.Position = 0;

            return File(stream, "text/csv", "products.csv");
        }

        private async IAsyncEnumerable<Product> GetProductsAsync()
        {
            var products = await _productService.GetAllAsync();

            foreach (var p in products)
                yield return p;
        }
    }
}