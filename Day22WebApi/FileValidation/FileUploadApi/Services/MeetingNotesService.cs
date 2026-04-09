using Microsoft.AspNetCore.Http;

namespace FileUploadApi.Services
{
    public class MeetingNotesService : IFileTypeService
    {
        public bool Supports(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();
            return ext == ".txt";
        }

        public void Validate(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".txt")
                throw new Exception("Only TXT files are allowed");
        }

        public string GetFolderName()
        {
            return "MeetingNotes";
        }
    }
}