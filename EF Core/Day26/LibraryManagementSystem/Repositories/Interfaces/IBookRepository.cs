using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.Requests;

namespace LibraryManagementSystem.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> GetIsbnAsync(string isbn);
        Task<List<Book>> GetAllAsync();
        Task<List<Book>> GetAvailableAsync();
        Task<(List<Book> Books, int TotalCount)> SearchAsync(BookSearchCriteria criteria);
        Task<Book> CreateAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task<BookStatistics> GetStatisticsAsync();
        Task<List<BookSummaryDto>> SearchBooksAsync(string keyword);

    }
}