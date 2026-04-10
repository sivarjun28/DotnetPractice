using Exercise04.Data;
using Exercise04.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercise04.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private static readonly Func<ApplicationDbContext, int, Task<Product?>> _getByIdCompiled =
            EF.CompileAsyncQuery((ApplicationDbContext context, int id) =>
                context.Products.FirstOrDefault(p => p.Id == id));
        public async Task<Product?> GetByIdCompiledAsync(int id)
        {
            return await _getByIdCompiled(_context, id);
        }

        public async Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;
            var query = _context.Products.AsNoTracking();

            var totalCount = await query.CountAsync();

            var items = await query
                        .OrderBy(p => p.Id)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
            return new PagedResult<Product>
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = pageSize,
                Page = page
            };
        }

        public async Task<IEnumerable<ProductListDto>> GetProductListAsync()
        {
            return _context.Products
                            .AsNoTracking()
                            .Select(p => new ProductListDto
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Price = p.Price,
                                Stock = p.Stock
                            });
        }
    }
}