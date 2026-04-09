using System.Text.Json;
using FileUploadApi.Models;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

namespace FileUploadApi.Services
{
    public class FileService : IFileService
    {
        private readonly string _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
        private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "files.json");

        public FileService()
        {
            if (!Directory.Exists(_uploadDirectory))
                Directory.CreateDirectory(_uploadDirectory);
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            try
            {
                var filePath = Path.Combine(_uploadDirectory, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileMetadata = new FileMetadataDto
                {
                    FileName = file.FileName,
                    FilePath = filePath,
                    UploadDate = DateTime.Now
                };

                var files = GetUploadedFiles();
                files.Add(fileMetadata);

                var jsonData = JsonSerializer.Serialize(files, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_jsonFilePath, jsonData);

                return file.FileName;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error saving file.", ex);
            }
        }

        public List<FileMetadataDto> GetUploadedFiles()
        {
            try
            {
                if (!File.Exists(_jsonFilePath))
                    return new List<FileMetadataDto>();

                var jsonData = File.ReadAllText(_jsonFilePath);
                return JsonSerializer.Deserialize<List<FileMetadataDto>>(jsonData) ?? new List<FileMetadataDto>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error reading file metadata.", ex);
            }
        }

        public async Task<List<FileMetadataDto>> SaveMultipleFilesAsync(List<IFormFile> files)
        {
            var uploadedFiles = new List<FileMetadataDto>();

            foreach (var file in files)
            {
                var filePath = Path.Combine(_uploadDirectory, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileMetadata = new FileMetadataDto
                {
                    FileName = file.FileName,
                    FilePath = filePath,
                    UploadDate = DateTime.Now
                };

                uploadedFiles.Add(fileMetadata);
            }

            var existingFiles = GetUploadedFiles();
            existingFiles.AddRange(uploadedFiles);

            var jsonData = JsonSerializer.Serialize(existingFiles, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_jsonFilePath, jsonData);

            return uploadedFiles;
        }

    }
}
