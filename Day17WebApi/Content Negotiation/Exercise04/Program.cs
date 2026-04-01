using Exercise04.Helper;

var builder = WebApplication.CreateBuilder(args);

// ADD SERVICES to the container
builder.Services.AddControllers(options =>
{
    // Add CSV formatter
    options.OutputFormatters.Add(new CsvOutputFormatter());
})
.AddXmlSerializerFormatters(); // Enables XML support

builder.Services.AddEndpointsApiExplorer();  // For Swagger
builder.Services.AddSwaggerGen();        
  // API documentation

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

app.Run();  // Start the web server

// ✅ Add this AFTER all top-level statements
public partial class Program { }