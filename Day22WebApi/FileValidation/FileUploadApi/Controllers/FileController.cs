using FileUploadApi.Constants;
using FileUploadApi.Models;
using FileUploadApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost(RouteConstants.Upload)]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "No file uploaded"
                    });
                }

                var result = await _fileService.SaveFileAsync(file);

                return Ok(new ApiResponse<FileMetadataDto>
                {
                    Success = true,
                    Message = "File uploaded successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost(RouteConstants.UploadMultiple)]
        public async Task<IActionResult> UploadMultipleFiles([FromForm] List<IFormFile> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "No files uploaded"
                    });
                }

                var result = await _fileService.SaveMultipleFilesAsync(files);

                return Ok(new ApiResponse<List<FileMetadataDto>>
                {
                    Success = true,
                    Message = "Files uploaded successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet(RouteConstants.File)]
        public IActionResult GetFiles([FromQuery] string? fileType)
        {
            try
            {
                var files = _fileService.GetUploadedFiles(fileType);

                return Ok(new ApiResponse<List<FileMetadataDto>>
                {
                    Success = true,
                    Message = "Files retrieved successfully",
                    Data = files
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet(RouteConstants.AllFiles)]
        public IActionResult GetAllFiles()
        {
            try
            {
                var files = _fileService.GetUploadedFiles(null);

                var groupedFiles = files
                    .GroupBy(f => f.FileType)
                    .ToDictionary(
                        g => g.Key,
                        g => g.ToList()
                    );

                return Ok(new ApiResponse<Dictionary<string, List<FileMetadataDto>>>
                {
                    Success = true,
                    Message = "All files grouped by type retrieved successfully",
                    Data = groupedFiles
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error retrieving grouped files",
                    Data = new
                    {
                        Error = ex.Message,
                        Details = "Failed to fetch files grouped by type"
                    }
                });
            }
        }
    }
}