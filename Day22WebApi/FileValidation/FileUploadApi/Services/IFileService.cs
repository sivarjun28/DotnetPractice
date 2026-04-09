
using FileUploadApi.Models;

namespace FileUploadApi.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file);
        List<FileMetadataDto> GetUploadedFiles();
         Task<List<FileMetadataDto>> SaveMultipleFilesAsync(List<IFormFile> files);
    }
}