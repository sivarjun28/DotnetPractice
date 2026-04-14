using ECommerceAPI.Data;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Models.Entities
{
    public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> AddAsync(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<PagedResult<Product>> SearchAsync(ProductSearchCriteria criteria)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrEmpty(criteria.Name))
        {
            query = query.Where(p => p.Name.Contains(criteria.Name));
        }

        if (!string.IsNullOrEmpty(criteria.Category))
        {
            query = query.Where(p => p.Category.Name.Contains(criteria.Category));
        }

        if (criteria.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= criteria.MinPrice);
        }

        if (criteria.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= criteria.MaxPrice);
        }

       

        var totalCount = await query.CountAsync();

        if (!string.IsNullOrEmpty(criteria.SortBy))
        {
            if (criteria.SortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
            {
                query = criteria.IsDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
            }
            else if (criteria.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                query = criteria.IsDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
            }
        }

        var items = await query
            .Skip((criteria.PageNumber - 1) * criteria.PageSize)
            .Take(criteria.PageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling((double)totalCount / criteria.PageSize);

        return new PagedResult<Product>
        {
            TotalCount = totalCount,
            TotalPages = totalPages,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize,
            Items = items
        };
    }

    public async Task<bool> SkuExistsAsync(string sku)
    {
        return await _context.Products.AnyAsync(p => p.Sku == sku);
    }

    public async Task<Product?> UpdateAsync(Product product)
    {
        var existingProduct = await _context.Products.FindAsync(product.Id);
        if (existingProduct == null) return null;

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Sku = product.Sku;
        existingProduct.Price = product.Price;
        existingProduct.CategoryId = product.CategoryId;
        existingProduct.Stock = product.Stock;
        existingProduct.IsActive = product.IsActive;
        existingProduct.Tags = product.Tags;
        existingProduct.Images = product.Images;
        existingProduct.Reviews = product.Reviews;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingProduct;
    }
}
}