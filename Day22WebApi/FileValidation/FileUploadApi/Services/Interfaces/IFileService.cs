using FileUploadApi.Models;
using FileUploadApi.Models.Requests;

namespace FileUploadApi.Services.Interfaces
{
    public interface IFileService
    {
        Task<ApiResponse<object>> UploadAsync(FileUploadRequest request);
        Task<object> GetAllAsync(string moduleName);
        Task<bool> DeleteAsync(string moduleName, int id);
        Task<ApiResponse<object>> GetFilesAsync();
    }

}