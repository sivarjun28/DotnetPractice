

using Execise01.MiddleWare;
using Exercise01.MiddleWare;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = "swagger";
    });
}


app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCorrelationId();
app.UseRequestLogging();
app.UseRateLimiting();
app.UseRequestValidation();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();