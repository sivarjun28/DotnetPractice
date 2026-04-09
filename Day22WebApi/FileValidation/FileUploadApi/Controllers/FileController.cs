using FileUploadApi.Constants;
using FileUploadApi.Models;
using FileUploadApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

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
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new ApiResponse<ErrorResponse>
                    {
                        Success = false,
                        Message = "No file uploaded.",
                        Data = new ErrorResponse { Error = "No file", Details = "Please upload a file." }
                    });
                }

                var fileName = await _fileService.SaveFileAsync(file);

                var response = new ApiResponse<FileMetadataDto>
                {
                    Success = true,
                    Message = "File uploaded successfully.",
                    Data = new FileMetadataDto
                    {
                        FileName = fileName,
                        FilePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName),
                        UploadDate = DateTime.Now
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ErrorResponse>
                {
                    Success = false,
                    Message = "An error occurred while uploading the file.",
                    Data = new ErrorResponse { Error = ex.Message, Details = ex.StackTrace }
                });
            }
        }


        [HttpGet(RouteValue.File)]
        public IActionResult GetFiles()
        {
            try
            {
                var files = _fileService.GetUploadedFiles();
                var fileDtos = files.Select(f => new FileMetadataDto
                {
                    FileName = f.FileName,
                    FilePath = f.FilePath,
                    UploadDate = f.UploadDate
                }).ToList();

                var response = new ApiResponse<List<FileMetadataDto>>
                {
                    Success = true,
                    Message = "Files retrieved successfully.",
                    Data = fileDtos
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ErrorResponse>
                {
                    Success = false,
                    Message = "An error occurred while fetching the files.",
                    Data = new ErrorResponse { Error = ex.Message, Details = ex.StackTrace }
                });
            }
        }

        [HttpPost(RouteValue.UploadMultiple)]
        public async Task<IActionResult> UploadMultipleFiles([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "No files uploaded.",
                    Data = new ErrorResponse
                    {
                        Error = "No files",
                        Details = "Please upload at least one file."
                    }
                });
            }

            var uploadedFiles = await _fileService.SaveMultipleFilesAsync(files);

            return Ok(new ApiResponse<List<FileMetadataDto>>
            {
                Success = true,
                Message = "Files uploaded successfully.",
                Data = uploadedFiles
            });
        }

    }
}