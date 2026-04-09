using Microsoft.AspNetCore.Http;

namespace FileUploadApi.Services
{
    public interface IFileTypeService
    {
        bool Supports(IFormFile file);
        void Validate(IFormFile file);
        string GetFolderName();
    }
}