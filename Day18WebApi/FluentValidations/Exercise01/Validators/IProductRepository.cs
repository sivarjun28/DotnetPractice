using Microsoft.EntityFrameworkCore;
using Exercise01.Models;
namespace Exercise01.Validators
{
    public interface IProductRepository
    {
        Task<bool> SkuExistsAsync(string sku, CancellationToken ct = default);
        Task<bool> SkuExistsForOtherProductAsync(int id, string sku, CancellationToken ct = default);
    }

    public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = new()
    {
        new Product { Id = 1, Sku = "AB1234" },
        new Product { Id = 2, Sku = "CD5678" }
    };

    public Task<bool> SkuExistsAsync(string sku, CancellationToken ct = default)
    {
        var exists = _products.Any(p => p.Sku == sku);
        return Task.FromResult(exists);
    }

    public Task<bool> SkuExistsForOtherProductAsync(int id, string sku, CancellationToken ct = default)
    {
        var exists = _products.Any(p => p.Id != id && p.Sku == sku);
        return Task.FromResult(exists);
    }
}
}