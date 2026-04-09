using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(2, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Products API",
        Version = "v2",
        Description = "API for managing products (v2)"
    });

    c.EnableAnnotations();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;
        var versions = methodInfo.DeclaringType?
            .GetCustomAttributes(true)
            .OfType<ApiVersionAttribute>()
            .SelectMany(a => a.Versions);
        return versions?.Any(v => $"v{v.ToString()}" == docName) ?? false;
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Products API v2");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();
app.Run();