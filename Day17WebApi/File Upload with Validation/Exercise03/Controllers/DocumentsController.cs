using Microsoft.AspNetCore.Mvc;
using Exercise03.Helpers;
using Exercise03.Models;
using System.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
namespace Exercise03.Controllers
{
    [ApiController]
    [Route("api/documents")]
    public class DocumentsController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<DocumentsController> logger;

        public DocumentsController(IWebHostEnvironment env,
                                    ILogger<DocumentsController> logger)
        {
            this.env = env;
            this.logger = logger;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            var allowedTypes = new[] { ".pdf", ".docx", ".xlsx" };
            if (!FileValidation.IsValidFileType(file, allowedTypes))
            {
                return BadRequest("Invalid file type. Only PDF, DOCX, and XLSX are allowed.");
            }

            if (!FileValidation.IsValidFileSize(file, 10 * 1024 * 1024))
            {
                return BadRequest("File size exceeds 10MB.");
            }

            // Use custom directory path (without relying on WebRootPath)
            string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            // Ensure the uploads folder exists
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }


            string fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadDirectory, fileName);



            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var documentInfo = new DocumentInfo
            {
                Id = Guid.NewGuid().ToString(),
                FileName = fileName,
                FileSize = file.Length,
                ContentType = file.ContentType,
                UploadedAt = DateTime.UtcNow,
                DownloadUrl = $"api/documents/{fileName}/download"
            };

            return Ok(documentInfo);
        }

        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultiple(List<IFormFile> files)
        {
            // Log number of files received
            logger.LogInformation($"Number of files received: {files?.Count}");

            if (files == null || files.Count == 0)
            {
                return BadRequest("No files uploaded");
            }

            var documentInfos = new List<DocumentInfo>();
            var failedUploads = new List<string>();

            foreach (var file in files)
            {
                try
                {
                    // Log file details to help debug
                    logger.LogInformation($"Processing file: {file.FileName}, Size: {file.Length} bytes");

                    // Check if file is null or empty
                    if (file == null || file.Length == 0)
                    {
                        logger.LogWarning($"File {file.FileName} is empty");
                        failedUploads.Add(file.FileName);
                        continue;
                    }

                    string[] allowedTypes = new[] { ".pdf", ".docx", ".xlsx" };
                    bool isValidType = FileValidation.IsValidFileType(file, allowedTypes);
                    bool isValidSize = FileValidation.IsValidFileSize(file, 10 * 1024 * 1024); // 10MB

                    // Log the validation result
                    logger.LogInformation($"File {file.FileName} type valid: {isValidType}, size valid: {isValidSize}");

                    if (!isValidType || !isValidSize)
                    {
                        logger.LogWarning($"File {file.FileName} failed validation");
                        failedUploads.Add(file.FileName);
                        continue;
                    }

                    // Create the upload directory if it doesn't exist
                    string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }

                    // Generate a random file name
                    string fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(uploadDirectory, fileName);

                    // Save the file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Create document info
                    var documentInfo = new DocumentInfo
                    {
                        Id = Guid.NewGuid().ToString(),
                        FileName = fileName,
                        FileSize = file.Length,
                        ContentType = file.ContentType,
                        UploadedAt = DateTime.UtcNow,
                        DownloadUrl = $"api/documents/{fileName}/download"
                    };

                    documentInfos.Add(documentInfo);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error processing file {file.FileName}: {ex.Message}");
                    failedUploads.Add(file.FileName);
                }
            }

            logger.LogInformation($"Upload complete. Success: {documentInfos.Count}, Failed: {failedUploads.Count}");
            return Ok(new { Success = documentInfos, Failed = failedUploads });
        }

        [HttpPost("upload-with-metadata")]
        public async Task<IActionResult> UploadWithMetaData([FromForm] DocumentUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("No file Uploaded");
            }
            string[] allowedTypes = new[] { ".pdf", ".docx", ".xlsx" };
            if (!FileValidation.IsValidFileType(request.File, allowedTypes) || !FileValidation.IsValidFileSize(request.File, 10 * 1024 * 1024))
            {
                return BadRequest("Invalid file type or file size exceeds the limit.");
            }
            string fileName = Path.GetRandomFileName() + Path.GetExtension(request.File.FileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

            var uploadDirectory = Path.GetDirectoryName(filePath);
            if (uploadDirectory != null && !Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }
            var documentInfo = new DocumentInfo
            {
                Id = Guid.NewGuid().ToString(),
                FileName = fileName,
                FileSize = request.File.Length,
                ContentType = request.File.ContentType,
                UploadedAt = DateTime.UtcNow,
                DownloadUrl = $"{Request.Scheme}://{Request.Host}/api/documents/{fileName}/download",
                Title = request.Title,
                Description = request.Description,
                Category = request.Category,
                Tags = request.Tags,
                IsPublic = request.IsPublic

            };
            return Ok(documentInfo);
        }

        [HttpPost("upload-chunk")]
        public async Task<IActionResult> UploadChunck([FromForm] FileChunkRequest chunk)
        {
            if (chunk.Chunk == null || chunk.Chunk.Length == 0)
            {
                return BadRequest("Chunk is empty.");
            }
            // Define the upload directory
            string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "chunks");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            string tempFilePath = Path.Combine(uploadDirectory, chunk.FileId);
            using (var stream = new FileStream(tempFilePath, FileMode.Append))
            {
                await chunk.Chunk.CopyToAsync(stream);
            }
            if (chunk.ChunkNumber == chunk.TotalChunks)
            {
                string finalFileName = $"{chunk.FileId}.finalFileName";
                string finalFilePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", finalFileName);
                System.IO.File.Move(tempFilePath, finalFilePath);

                return Ok(new { message = "File Uploaded successfully." });

            }
            var progress = (double)chunk.ChunkNumber / chunk.TotalChunks * 100;
            return Ok(new { Progress = progress });
        }


        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No image uploaded");

            var allowedTypes = new[] { ".jpg", ".jpeg", ".png" };
            if (!FileValidation.IsValidFileType(image, allowedTypes))
                return BadRequest("Invalid image type");

            string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            string fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
            string filePath = Path.Combine(uploadDir, fileName);

            // Save the original image
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Generate thumbnail
            string thumbnailPath = Path.Combine(uploadDir, "thumb_" + fileName);
            using (var img = await Image.LoadAsync(filePath)) // Load the image
            {
                img.Mutate(x => x.Resize(200, 200)); // Resize to 200x200
                await img.SaveAsync(thumbnailPath); // Save thumbnail
            }

            return Ok(new
            {
                OriginalUrl = $"uploads/{fileName}",
                ThumbnailUrl = $"uploads/thumb_{fileName}"
            });
        }
        [HttpGet("{fileName}/download")]
        public IActionResult Download(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found");

            // Determine content type based on extension
            string contentType = fileName.EndsWith(".png") ? "image/png" :
                                 fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg") ? "image/jpeg" :
                                 "application/octet-stream";

            // Return the file as a download
            return PhysicalFile(filePath, contentType, fileName);
        }
    }
}