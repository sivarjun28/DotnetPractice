using Microsoft.AspNetCore.Http;

namespace FileUploadApi.Services
{
    public class LessonsLearnedService : IFileTypeService
    {
        public bool Supports(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();
            return ext == ".jpg" || ext == ".png";
        }

        public void Validate(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".jpg" && ext != ".png")
                throw new Exception("Only JPG and PNG files are allowed");
        }

        public string GetFolderName()
        {
            return "LessonsLearned";
        }
    }
}