using Microsoft.AspNetCore.Http;

namespace FileUploadApi.Services
{
    public class BestPracticeService : IFileTypeService
    {
        public bool Supports(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();
            return ext == ".pdf";
        }

        public void Validate(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".pdf")
                throw new Exception("Only PDF files are allowed");
        }

        public string GetFolderName()
        {
            return "BestPractice";
        }
    }
}