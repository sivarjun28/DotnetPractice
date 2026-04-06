using Exercise05.Filters.Action;
using Exercise05.Filters.Authorization;
using Exercise05.Filters.Exception;
using Exercise05.Filters.Resource;
using Exercise05.Filters.Result;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<FirstActionFilter>();
builder.Services.AddScoped<SecondActionFilter>();
builder.Services.AddScoped<ThirdActionFilter>();

builder.Services.AddScoped<CustomExceptionFilter>();
builder.Services.AddScoped<CustomAuthorizationFilter>();
builder.Services.AddScoped<CustomResourceFilter>();
builder.Services.AddScoped<CustomResultFilter>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomExceptionFilter>(1);
    options.Filters.Add<CustomAuthorizationFilter>(2);
    options.Filters.Add<CustomResourceFilter>(3);
    options.Filters.Add<CustomResultFilter>(5);
});

var app = builder.Build();

app.MapControllers();

app.Run();