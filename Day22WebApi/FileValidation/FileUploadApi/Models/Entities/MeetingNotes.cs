namespace FileUploadApi.Models.Entities
{
    public class MeetingNotes : FileRecord
    {
        public string MeetingTitle { get; set; } = string.Empty;

        public DateTime MeetingDate { get; set; } = DateTime.UtcNow;

        public string? Notes { get; set; }
    }
}