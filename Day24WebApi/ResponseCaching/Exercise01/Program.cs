using Exercise01.MiddleWares;
using Exercise01.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

builder.Services.AddResponseCaching();
builder.Services.AddMemoryCache();
// Register your service
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default", new CacheProfile
    {
        Duration = 60,
        Location = ResponseCacheLocation.Any
    });

    options.CacheProfiles.Add("Never", new CacheProfile
    {
        Location = ResponseCacheLocation.None,
        NoStore = true
    });

    options.CacheProfiles.Add("Aggressive", new CacheProfile
    {
        Duration = 100,
        Location = ResponseCacheLocation.Any,
        VaryByQueryKeys = new[] { "*" }
    });
});

var app = builder.Build();
app.UseMiddleware<CustomResponseCachingMiddleware>();

// Middleware
app.UseResponseCaching();

app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new CacheControlHeaderValue
        {
            Public = true,
            MaxAge = TimeSpan.FromSeconds(60)
        };

    context.Response.Headers[HeaderNames.Vary] =
        new[] { "Accept", "Accept-Encoding" };

    await next();
});

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapControllers();

app.Run();