using FileUploadApi.Data;
using FileUploadApi.Models.Entities;
using FileUploadApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileUploadApi.Repositories.Implementations
{
    public class MeetingNotesRepository : IMeetingNotesRepository
    {
        private readonly FileUploadDbContext _context;

        public MeetingNotesRepository(FileUploadDbContext context)
        {
            _context = context;
        }

        public async Task<MeetingNotes> AddAsync(MeetingNotes entity)
        {
            await _context.MeetingNotes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<MeetingNotes>> GetAllAsync()
        {
            return await _context.MeetingNotes
                .OrderByDescending(x => x.UploadedDate)
                .ToListAsync();
        }

        public async Task<MeetingNotes?> GetByIdAsync(int id)
        {
            return await _context.MeetingNotes
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeleteAsync(MeetingNotes entity)
        {
            _context.MeetingNotes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}