
using HierarchicalOrganizationStructure.Data;
using HierarchicalOrganizationStructure.Services.Implementations;
using HierarchicalOrganizationStructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EmployeedbContext>(options =>
        options.UseMySql(connectionString,
                    new MySqlServerVersion(new Version(8, 0, 0)))

);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
        app.UseSwagger();
        app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();