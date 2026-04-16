using ECommerceOrderSystem.Data;
using ECommerceOrderSystem.Services.Implementations;
using ECommerceOrderSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ECommerceDbContext>(options =>
        options.UseMySql(connectionString,
                    new MySqlServerVersion(new Version(8, 0, 0)))

);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IOrderService, OrderService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
        app.UseSwagger();
        app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();