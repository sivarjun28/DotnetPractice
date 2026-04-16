

namespace FileUploadApi.Controllers
{
    using FileUploadApi.Constants;
    using FileUploadApi.Models;
    using FileUploadApi.Models.Requests;
    using FileUploadApi.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost(RouteConstants.Upload)]
        public async Task<IActionResult> Upload([FromForm] FileUploadRequest request)
        {
            var result = await _fileService.UploadAsync(request);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "File uploaded successfully",
                Data = result
            });
        }
        [HttpGet("{moduleName}")]
        public async Task<IActionResult> GetAll(string moduleName)
        {
            var result = await _fileService.GetAllAsync(moduleName);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Data fetched successfully",
                Data = result
            });
        }
        [HttpGet(RouteConstants.AllFiles)]
        public async Task<IActionResult> GetFiles()
        {
            var response = await _fileService.GetFilesAsync();

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpDelete("{moduleName}/{id}")]
        public async Task<IActionResult> Delete(string moduleName, int id)
        {
            var result = await _fileService.DeleteAsync(moduleName, id);

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "File deleted successfully",
                Data = result
            });
        }
    }
}