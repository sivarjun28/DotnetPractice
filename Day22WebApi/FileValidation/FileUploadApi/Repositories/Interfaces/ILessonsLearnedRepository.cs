using FileUploadApi.Models.Entities;

namespace FileUploadApi.Repositories.Interfaces
{
    public interface ILessonsLearnedRepository
    {
        Task<LessonsLearned > AddAsync(LessonsLearned entity);
        Task<List<LessonsLearned>> GetAllAsync();
        Task<LessonsLearned?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(LessonsLearned entity);
    }
}