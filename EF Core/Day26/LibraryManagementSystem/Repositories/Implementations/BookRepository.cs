using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.Requests;
using LibraryManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories.Implementations
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;
        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<Book> CreateAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books
                        .OrderBy(b => b.Title)
                        .ToListAsync();
        }

        public async Task<List<Book>> GetAvailableAsync()
        {
            return await _context.Books
                        .Where(b => b.AvailableCopies > 0)
                        .OrderBy(b => b.Title)
                        .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book?> GetIsbnAsync(string isbn)
        {
            return await _context.Books
                        .FirstOrDefaultAsync(b => b.Isbn == isbn);
        }

        public async Task<BookStatistics> GetStatisticsAsync()
        {
            var books = await _context.Books.ToListAsync();

            if (!books.Any())
            {
                return new BookStatistics();
            }

            var mostExpensive = books
                .OrderByDescending(b => b.Price)
                .FirstOrDefault();

            var longest = books
                .OrderByDescending(b => b.Pages)
                .FirstOrDefault();

            return new BookStatistics
            {
                TotalBooks = books.Count,
                AvailableBooks = books.Count(b => b.AvailableCopies > 0),
                TotalValue = books.Sum(b => b.Price * b.AvailableCopies),
                AveragePrice = books.Average(b => b.Price),
                TotalPages = books.Sum(b => b.Pages),
                MostExpensiveBook = mostExpensive?.Title ?? string.Empty,
                LongestBook = longest?.Title ?? string.Empty
            };
        }

        public async Task<(List<Book> Books, int TotalCount)> SearchAsync(BookSearchCriteria criteria)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(criteria.Title))
                query = query.Where(b => b.Title.Contains(criteria.Title));

            if (!string.IsNullOrWhiteSpace(criteria.Publisher))
                query = query.Where(b => b.Publisher.Contains(criteria.Publisher));

            if (criteria.MinPages.HasValue)
                query = query.Where(b => b.Pages >= criteria.MinPages.Value);

            if (criteria.MaxPages.HasValue)
                query = query.Where(b => b.Pages <= criteria.MaxPages.Value);

            if (criteria.MinPrice.HasValue)
                query = query.Where(b => b.Price >= criteria.MinPrice.Value);

            if (criteria.MaxPrice.HasValue)
                query = query.Where(b => b.Price <= criteria.MaxPrice.Value);

            if (criteria.PublishedAfter.HasValue)
                query = query.Where(b => b.PublishedDate >= criteria.PublishedAfter.Value);

            if (criteria.PublishedBefore.HasValue)
                query = query.Where(b => b.PublishedDate <= criteria.PublishedBefore.Value);

            if (criteria.OnlyAvailable == true)
                query = query.Where(b => b.AvailableCopies > 0);

            var totalCount = await query.CountAsync();

            query = criteria.SortBy?.ToLower() switch
            {
                "title" => criteria.SortDescending
                    ? query.OrderByDescending(b => b.Title)
                    : query.OrderBy(b => b.Title),

                "price" => criteria.SortDescending
                    ? query.OrderByDescending(b => b.Price)
                    : query.OrderBy(b => b.Price),

                "pages" => criteria.SortDescending
                    ? query.OrderByDescending(b => b.Pages)
                    : query.OrderBy(b => b.Pages),

                _ => query.OrderBy(b => b.Title)
            };

            var books = await query
                .Skip((criteria.Page - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync();

            return (books, totalCount);
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<List<BookSummaryDto>> SearchBooksAsync(string keyword)
{
    return await _context.Books
        .AsNoTracking()
        .Where(b => b.Title.Contains(keyword))
        .Select(b => new BookSummaryDto
        {
            Id = b.Id,
            Title = b.Title,
            Isbn = b.Isbn,
            Price = b.Price,
            IsAvailable = b.AvailableCopies > 0
        })
        .ToListAsync();
}
    }
}