namespace Exercise03.Helpers
{


    public class FileValidation
    {
        public static bool IsValidFileType(IFormFile file, string[] allowedExtensions)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(fileExtension);
        }

        public static bool IsValidFileSize(IFormFile file, long maxSizeBytes)
        {
            return file.Length <= maxSizeBytes;
        }

        public static bool IsValidImage(IFormFile file)
        {
            var imageTypes = new[] { "image/jpeg", "image/png" };
            return imageTypes.Contains(file.ContentType);
        }
        public static async Task<(int width, int height)> GetImageDimensions(IFormFile file)
        {
            return (1000, 1000);
        }
    }
}