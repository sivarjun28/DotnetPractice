using System.Text.Json;
using FileUploadApi.Models;
using FileUploadApi.Models.Entities;
using FileUploadApi.Models.Requests;
using FileUploadApi.Repositories.Interfaces;
using FileUploadApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FileUploadApi.Services.Implementations
{

    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IBestPracticeRepository _bestRepo;
        private readonly ILessonsLearnedRepository _lessonRepo;
        private readonly IMeetingNotesRepository _meetingRepo;

        public FileService(
            IWebHostEnvironment env,
            IBestPracticeRepository bestRepo,
            ILessonsLearnedRepository lessonRepo,
            IMeetingNotesRepository meetingRepo)
        {
            _env = env;
            _bestRepo = bestRepo;
            _lessonRepo = lessonRepo;
            _meetingRepo = meetingRepo;
        }

        public async Task<ApiResponse<object>> UploadAsync(FileUploadRequest request)
        {
            try
            {
                if (request == null || request.Files == null || !request.Files.Any())
                    return ApiResponse<object>.ErrorResponse("Files are required");

                if (string.IsNullOrWhiteSpace(request.ModuleName))
                    return ApiResponse<object>.ErrorResponse("Module name is required");

                var moduleName = request.ModuleName.Trim().ToLower();

                var validModules = new[] { "bestpractice", "lessonslearned", "meetingnotes" };
                if (!validModules.Contains(moduleName))
                    return ApiResponse<object>.ErrorResponse("Invalid module name");

                var rootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var folderPath = Path.Combine(rootPath, "UploadedFiles", moduleName);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var results = new List<object>();

                foreach (var file in request.Files)
                {
                    if (file.Length == 0) continue;

                    var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                    var fullPath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var dbPath = Path.Combine("UploadedFiles", moduleName, fileName);

                    switch (moduleName)
                    {
                        case "bestpractice":
                            var best = new BestPractice
                            {
                                Title = request.Title ?? "",
                                Description = request.Description,
                                FileName = fileName,
                                FilePath = dbPath,
                                ContentType = file.ContentType,
                                FileSize = file.Length,
                                UploadedDate = DateTime.UtcNow
                            };
                            await _bestRepo.AddAsync(best);
                            results.Add(best);
                            break;

                        case "lessonslearned":
                            var lesson = new LessonsLearned
                            {
                                Topic = request.Title ?? "No Topic",
                                Description = request.Description,
                                FileName = fileName,
                                FilePath = dbPath,
                                ContentType = file.ContentType,
                                FileSize = file.Length,
                                UploadedDate = DateTime.UtcNow
                            };
                            await _lessonRepo.AddAsync(lesson);
                            results.Add(lesson);
                            break;

                        case "meetingnotes":
                            var meeting = new MeetingNotes
                            {
                                MeetingTitle = request.Title ?? "No Title",
                                MeetingDate = DateTime.UtcNow,
                                Notes = request.Description,
                                FileName = fileName,
                                FilePath = dbPath,
                                ContentType = file.ContentType,
                                FileSize = file.Length,
                                UploadedDate = DateTime.UtcNow
                            };
                            await _meetingRepo.AddAsync(meeting);
                            results.Add(meeting);
                            break;
                    }
                }

                return ApiResponse<object>.SuccessResponse(results, "Files uploaded successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResponse("Upload failed", new List<string> { ex.Message });
            }
        }

        public async Task<object> GetAllAsync(string moduleName)
        {
            if (string.IsNullOrWhiteSpace(moduleName))
                throw new ArgumentException("Module name is required");

            moduleName = moduleName.ToLower();

            return moduleName switch
            {
                "bestpractice" => await _bestRepo.GetAllAsync(),
                "lessonslearned" => await _lessonRepo.GetAllAsync(),
                "meetingnotes" => await _meetingRepo.GetAllAsync(),
                _ => throw new ArgumentException("Invalid module name")
            };
        }

        public async Task<bool> DeleteAsync(string moduleName, int id)
        {
            if (string.IsNullOrWhiteSpace(moduleName))
                throw new ArgumentException("Module name is required");

            moduleName = moduleName.ToLower();

            switch (moduleName)
            {
                case "bestpractice":
                    var best = await _bestRepo.GetByIdAsync(id);
                    if (best == null)
                        throw new KeyNotFoundException("Record not found");

                    DeletePhysicalFile(best.FilePath);
                    await _bestRepo.DeleteAsync(best);
                    break;

                case "lessonslearned":
                    var lesson = await _lessonRepo.GetByIdAsync(id);
                    if (lesson == null)
                        throw new KeyNotFoundException("Record not found");

                    DeletePhysicalFile(lesson.FilePath);
                    await _lessonRepo.DeleteAsync(lesson);
                    break;

                case "meetingnotes":
                    var meeting = await _meetingRepo.GetByIdAsync(id);
                    if (meeting == null)
                        throw new KeyNotFoundException("Record not found");

                    DeletePhysicalFile(meeting.FilePath);
                    await _meetingRepo.DeleteAsync(meeting);
                    break;

                default:
                    throw new ArgumentException("Invalid module name");
            }

            return true;
        }
        private void DeletePhysicalFile(string relativePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, relativePath);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public async Task<ApiResponse<object>> GetFilesAsync()
        {
            try
            {
                var best = await _bestRepo.GetAllAsync();
                var lessons = await _lessonRepo.GetAllAsync();
                var meetings = await _meetingRepo.GetAllAsync();

                var result = new
                {
                    BestPractices = best,
                    LessonsLearned = lessons,
                    MeetingNotes = meetings
                };

                return ApiResponse<object>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResponse("Failed to fetch data", new List<string> { ex.Message });
            }
        }

    }
}