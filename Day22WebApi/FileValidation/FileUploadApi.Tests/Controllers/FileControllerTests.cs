using Xunit;
using Moq;
using FileUploadApi.Controllers;
using FileUploadApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using FileUploadApi.Models;

namespace FileUpload.Tests.Controllers
{
    public class FileControllerTests
    {
        [Fact]
        public async Task UploadFile_Should_Return_Ok_When_File_Is_Valid()
        {
            var mockService = new Mock<IFileService>();
            mockService.Setup(s => s.SaveFileAsync(It.IsAny<IFormFile>()))
                       .ReturnsAsync("test.txt");

            var controller = new FileController(mockService.Object);

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(100);
            mockFile.Setup(f => f.FileName).Returns("test.txt");

            var result = await controller.UploadFile(mockFile.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task UploadFile_Should_Return_BadRequest_When_File_Is_Null()
        {
            var mockService = new Mock<IFileService>();
            var controller = new FileController(mockService.Object);

            var result = await controller.UploadFile(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetFiles_Should_Return_Ok()
        {
            var mockService = new Mock<IFileService>();
            mockService.Setup(s => s.GetUploadedFiles())
                       .Returns(new List<FileMetadataDto>());

            var controller = new FileController(mockService.Object);

            var result = controller.GetFiles();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task UploadMultipleFiles_Should_Return_Ok()
        {
            var mockService = new Mock<IFileService>();
            mockService.Setup(s => s.SaveMultipleFilesAsync(It.IsAny<List<IFormFile>>()))
                       .ReturnsAsync(new List<FileMetadataDto>());

            var controller = new FileController(mockService.Object);

            var files = new List<IFormFile>
            {
                new Mock<IFormFile>().Object
            };

            var result = await controller.UploadMultipleFiles(files);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task UploadMultipleFiles_Should_Return_BadRequest_When_Empty()
        {
            var mockService = new Mock<IFileService>();
            var controller = new FileController(mockService.Object);

            var result = await controller.UploadMultipleFiles(new List<IFormFile>());

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}