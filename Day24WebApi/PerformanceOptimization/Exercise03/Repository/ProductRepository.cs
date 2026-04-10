using Exercise03.Data;
using Exercise03.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercise03.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllOptimizedAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> SearchOptimizedAsync(ProductSearchCriteria criteria)
        {
            IQueryable<Product> query = _context.Products.AsNoTracking();

            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(p => p.Name.Contains(criteria.Name));

            if (criteria.MinPrice.HasValue)
                query = query.Where(p => p.Price >= criteria.MinPrice);

            if (criteria.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= criteria.MaxPrice);

            if (!string.IsNullOrEmpty(criteria.Category))
                query = query.Where(p => p.Category == criteria.Category);

            query = criteria.SortBy?.ToLower() switch
            {
                "price" => criteria.SortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "name" => criteria.SortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                _ => query.OrderBy(p => p.Id)
            };

            query = query
                .Skip((criteria.Page - 1) * criteria.PageSize)
                .Take(criteria.PageSize);

            return await query.ToListAsync();
        }

        public async Task BulkInsertAsync(List<Product> products)
        {
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
        }
    }
}