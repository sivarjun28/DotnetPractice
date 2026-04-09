using Xunit;
using FileUploadApi.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FileUpload.Tests.Services
{
    public class FileServiceTests
    {
        [Fact]
        public async Task SaveFileAsync_Should_Return_FileName()
        {
            var service = new FileService();

            var content = "Hello World";
            var fileName = "test.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.Length).Returns(stream.Length);
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default))
                    .Returns((Stream target, System.Threading.CancellationToken token) =>
                    {
                        stream.CopyTo(target);
                        return Task.CompletedTask;
                    });

            var result = await service.SaveFileAsync(mockFile.Object);

            Assert.Equal(fileName, result);
        }

        [Fact]
        public void GetUploadedFiles_Should_Return_List()
        {
            var service = new FileService();

            var result = service.GetUploadedFiles();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task SaveMultipleFilesAsync_Should_Save_All_Files()
        {
            var service = new FileService();
            var files = new List<IFormFile>();

            for (int i = 0; i < 2; i++)
            {
                var content = $"File {i}";
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

                var mockFile = new Mock<IFormFile>();
                mockFile.Setup(f => f.FileName).Returns($"file{i}.txt");
                mockFile.Setup(f => f.Length).Returns(stream.Length);
                mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default))
                        .Returns((Stream target, System.Threading.CancellationToken token) =>
                        {
                            stream.CopyTo(target);
                            return Task.CompletedTask;
                        });

                files.Add(mockFile.Object);
            }

            var result = await service.SaveMultipleFilesAsync(files);

            Assert.Equal(2, result.Count);
        }
    }
}