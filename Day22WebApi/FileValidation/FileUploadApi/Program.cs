using FileUploadApi.Data;
using FileUploadApi.Middleware;
using FileUploadApi.Repositories.Implementations;
using FileUploadApi.Repositories.Interfaces;
using FileUploadApi.Services.Implementations;
using FileUploadApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<FileUploadDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IBestPracticeRepository, BestPracticeRepository>();
builder.Services.AddScoped<ILessonsLearnedRepository, LessonsLearnedRepository>();
builder.Services.AddScoped<IMeetingNotesRepository, MeetingNotesRepository>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();