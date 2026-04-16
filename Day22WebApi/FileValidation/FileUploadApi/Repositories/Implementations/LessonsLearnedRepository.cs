using FileUploadApi.Data;
using FileUploadApi.Models.Entities;
using FileUploadApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileUploadApi.Repositories.Implementations
{
    public class LessonsLearnedRepository : ILessonsLearnedRepository
    {
        private readonly FileUploadDbContext _context;

        public LessonsLearnedRepository(FileUploadDbContext context)
        {
            _context = context;
        }

        public async Task<LessonsLearned> AddAsync(LessonsLearned entity)
        {
            await _context.LessonsLearned.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<LessonsLearned>> GetAllAsync()
        {
            return await _context.LessonsLearned
                .OrderByDescending(x => x.UploadedDate)
                .ToListAsync();
        }

        public async Task<LessonsLearned?> GetByIdAsync(int id)
        {
            return await _context.LessonsLearned
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeleteAsync(LessonsLearned entity)
        {
            _context.LessonsLearned.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}