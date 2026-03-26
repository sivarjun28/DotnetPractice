var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


builder.Services.AddControllers();  // MVC controllers
builder.Services.AddEndpointsApiExplorer();  // For Swagger
builder.Services.AddSwaggerGen();  // API documentation

var app = builder.Build();

// CONFIGURE HTTP request pipeline (middleware)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();       // Swagger JSON endpoint
    app.UseSwaggerUI();     // Swagger UI
}

app.UseHttpsRedirection();  // Redirect HTTP to HTTPS
app.UseAuthorization();     // Authorization middleware
app.MapControllers();       // Map controller routes

app.Run();