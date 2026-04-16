using FileUploadApi.Data;
using FileUploadApi.Models.Entities;
using FileUploadApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileUploadApi.Repositories.Implementations
{
    public class BestPracticeRepository : IBestPracticeRepository
    {

        private readonly FileUploadDbContext _context;
        public BestPracticeRepository(FileUploadDbContext context)
        {
            _context = context;
        }
        public async Task<BestPractice> AddAsync(BestPractice entity)
        {
            await _context.BestPractices.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(BestPractice entity)
        {
            _context.BestPractices.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BestPractice>> GetAllAsync()
        {
            return await _context.BestPractices
                        .OrderByDescending(x => x.UploadedDate)
                        .ToListAsync();
        }

        public async Task<BestPractice?> GetByIdAsync(int id)
        {
            return await _context.BestPractices
                .FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}