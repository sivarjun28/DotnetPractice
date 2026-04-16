using FileUploadApi.Models.Entities;

namespace FileUploadApi.Repositories.Interfaces
{
    public interface IBestPracticeRepository
    {
        Task<BestPractice> AddAsync(BestPractice entity);
        Task<List<BestPractice>> GetAllAsync();
        Task<BestPractice?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(BestPractice entity);
    }
}