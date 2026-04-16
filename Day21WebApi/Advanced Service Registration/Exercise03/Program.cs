using Exercise03.Models;
using Exercise03.Repository;
using Exercise03.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter(
                System.Text.Json.JsonNamingPolicy.CamelCase, true));
    });
builder.Services.AddOpenApi();

builder.Services.Configure<NotificationOptions>(
    builder.Configuration.GetSection("Notifications")
);

var notificationOptions = builder.Configuration
    .GetSection("Notifications")
    .Get<NotificationOptions>() ?? new();

if (notificationOptions.EnableEmail)
{
    builder.Services.AddScoped<INotificationService, EmailNotificationService>();
}

if (notificationOptions.EnableSms)
{
    builder.Services.AddScoped<INotificationService, SmsNotificationService>();
}

if (notificationOptions.EnablePush)
{
    builder.Services.AddScoped<INotificationService, PushNotificationService>();
}

builder.Services.AddScoped<INotificationServiceFactory, NotificationServiceFactory>();
builder.Services.AddKeyedScoped<INotificationService, EmailNotificationService>("email");
builder.Services.AddKeyedScoped<INotificationService, SmsNotificationService>("sms");
builder.Services.AddKeyedScoped<INotificationService, PushNotificationService>("push");
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

app.MapControllers();
app.Run();