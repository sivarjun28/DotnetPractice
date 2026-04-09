using System.Text.Json;
using FileUploadApi.Models;
using Microsoft.AspNetCore.Http;

namespace FileUploadApi.Services
{
    public class FileService : IFileService
    {
        private readonly IEnumerable<IFileTypeService> _services;
        private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
        private readonly string _jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "files.json");

        public FileService(IEnumerable<IFileTypeService> services)
        {
            _services = services;
        }

        public async Task<FileMetadataDto> SaveFileAsync(IFormFile file)
        {
            var service = _services.FirstOrDefault(s => s.Supports(file));

            if (service == null)
                throw new Exception("Unsupported file type");

            service.Validate(file);

            var folderPath = Path.Combine(_basePath, service.GetFolderName());

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var metadata = new FileMetadataDto
            {
                FileName = file.FileName,
                FilePath = filePath,
                UploadDate = DateTime.Now,
                FileType = service.GetFolderName()
            };

            var files = GetUploadedFiles(null);
            files.Add(metadata);

            var json = JsonSerializer.Serialize(files, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_jsonPath, json);

            return metadata;
        }

        public async Task<List<FileMetadataDto>> SaveMultipleFilesAsync(List<IFormFile> files)
        {
            var result = new List<FileMetadataDto>();

            foreach (var file in files)
            {
                result.Add(await SaveFileAsync(file));
            }

            return result;
        }

        public List<FileMetadataDto> GetUploadedFiles(string? fileType)
        {
            if (!File.Exists(_jsonPath))
                return new List<FileMetadataDto>();

            var json = File.ReadAllText(_jsonPath);
            var files = JsonSerializer.Deserialize<List<FileMetadataDto>>(json) ?? new();

            if (!string.IsNullOrEmpty(fileType))
                files = files
                    .Where(f => f.FileType.Equals(fileType, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            return files;
        }
    }
}