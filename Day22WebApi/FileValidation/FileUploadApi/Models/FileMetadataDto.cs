namespace FileUploadApi.Models
{
    public class FileMetadataDto
    {
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
    }
}