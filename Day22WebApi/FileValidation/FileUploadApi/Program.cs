using FileUploadApi.Middleware;
using FileUploadApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<IFileTypeService, BestPracticeService>();
builder.Services.AddScoped<IFileTypeService, LessonsLearnedService>();
builder.Services.AddScoped<IFileTypeService, MeetingNotesService>();

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