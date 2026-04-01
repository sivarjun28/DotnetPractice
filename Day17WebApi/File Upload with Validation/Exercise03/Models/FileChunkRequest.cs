namespace Exercise03.Models
{
    public class FileChunkRequest
    {
        public required IFormFile Chunk { get; set; }
        public required string FileId { get; set; }
        public int ChunkNumber { get; set; }
        public int TotalChunks { get; set; }
        public string FileName { get; set; } = string.Empty;
    }

}