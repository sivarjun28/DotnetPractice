using FileUploadApi.Models;

namespace FileUploadApi.Services
{
    public interface IFileService
    {
        Task<FileMetadataDto> SaveFileAsync(IFormFile file);
        Task<List<FileMetadataDto>> SaveMultipleFilesAsync(List<IFormFile> files);
        List<FileMetadataDto> GetUploadedFiles(string? fileType);
    }
}