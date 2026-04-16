namespace FileUploadApi.Models.Entities
{
    public abstract class FileRecord
    {
        public int Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;
    }
}