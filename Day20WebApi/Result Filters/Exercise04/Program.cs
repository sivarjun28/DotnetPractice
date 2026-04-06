using Exercise04.Filters;
using Exercise04.Services;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<HateoasService>();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

// ✅ Controllers + Filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ResponseFormatterFilter>();
    options.Filters.Add<PaginationFilter>();

    // ❌ REMOVE this (was breaking response)
    // options.Filters.Add<CompressionFilter>();

    options.Filters.Add(new AddHeadersFilter(new Dictionary<string, string>
    {
        { "X-API-Version", "1.0" }
    }));
});

// ---------------- BUILD ----------------
var app = builder.Build();

// ✅ Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// ✅ Enable compression middleware
app.UseResponseCompression();

app.MapControllers();

app.Run();