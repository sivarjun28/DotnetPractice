using Exercise01.Repository;
using Exercise01.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

// Singleton - Created once for application lifetime
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
builder.Services.AddSingleton<IEventPublisher, InMemoryEventPublisher>();


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();

builder.Services.AddMemoryCache();
builder.Services.AddLogging();
builder.Services.AddControllers(); 

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();