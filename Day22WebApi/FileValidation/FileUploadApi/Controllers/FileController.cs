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

        [HttpPost(RouteValue.Upload)]
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

        [HttpPost(RouteValue.UploadMultiple)]
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

        [HttpGet(RouteValue.File)]
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
    }
}