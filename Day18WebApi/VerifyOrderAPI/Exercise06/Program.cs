using FluentValidation;
using FluentValidation.AspNetCore;
using Exercise06.Services;
using Exercise06.Models;
using Exercise06.Validators; // important

var builder = WebApplication.CreateBuilder(args);

// ✅ Controllers
builder.Services.AddControllers();

// ✅ FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();

// ✅ Register your service
builder.Services.AddScoped<IAddressValidationService, AddressValidationService>();

// Swagger (optional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

// ❗ Disable temporarily if debugging
// app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();