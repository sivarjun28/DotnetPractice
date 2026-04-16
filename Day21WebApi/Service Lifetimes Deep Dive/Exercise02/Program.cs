using Exercise02.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IApplicationCache, ApplicationCache>();
builder.Services.AddScoped<IRequestContext, RequestContext>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<IDbContext, DbContextStub>();
builder.Services.AddScoped<IOrderService, OrderServiceStub>();
builder.Services.AddHostedService<OrderProcessingBackgroundService>();
builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddOptions<ServiceProviderOptions>()
        .Configure(options => options.ValidateScopes = true);
}

var app = builder.Build();

app.MapControllers();

app.Run();