namespace FileUploadApi.Models.Requests
{
    public class FileUploadRequest
    {
        public string ModuleName { get; set; }
        public List<IFormFile> Files { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}