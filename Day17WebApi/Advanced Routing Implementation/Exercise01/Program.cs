using Exercise01.Constraints;

var builder = WebApplication.CreateBuilder(args);

// ADD SERVICES to the container
builder.Services.AddControllers();  // MVC controllers
builder.Services.AddEndpointsApiExplorer();  // For Swagger
builder.Services.AddSwaggerGen();  // API documentation
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("sku", typeof(SkuConstraint));
});
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("recentdate", typeof(RecentDateConstraint));
});
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