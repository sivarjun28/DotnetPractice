namespace FileUploadApi.Models.Entities
{
    public class LessonsLearned : FileRecord
    {
        public string Topic { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}