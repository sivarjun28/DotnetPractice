using FileUploadApi.Models.Entities;

namespace FileUploadApi.Repositories.Interfaces
{
    public interface IMeetingNotesRepository
    {
        Task<MeetingNotes> AddAsync(MeetingNotes entity);
        Task<List<MeetingNotes>> GetAllAsync();
        Task<MeetingNotes?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(MeetingNotes entity);
    }
}