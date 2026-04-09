using ProductApi.Repositories;
using ProductApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, InMemoryProductRepository>();
builder.Services.AddMemoryCache();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();