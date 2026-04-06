using Exercise03.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<ValidationExceptionFilter>();
builder.Services.AddScoped<NotFoundExceptionFilter>();
builder.Services.AddScoped<BusinessRuleExceptionFilter>();
builder.Services.AddScoped<GlobalExceptionFilter>();


var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();