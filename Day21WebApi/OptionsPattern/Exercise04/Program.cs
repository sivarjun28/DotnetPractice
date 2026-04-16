
using Exercise04.Features.Audit;
using Exercise04.Features.ConfigurationHistory;
using Exercise04.Features.Encryption;
using Exercise04.Features.Versioning;
using Exercise04.Options;
using Exercise04.Services;
using Exercise04.Validators;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddOptions<DatabaseOptions>()
    .Bind(builder.Configuration.GetSection(DatabaseOptions.Section))
    .ValidateOnStart();

builder.Services.AddSingleton<IValidateOptions<DatabaseOptions>, DatabaseOptionsValidator>();

builder.Services.Configure<CacheOptions>(
        builder.Configuration.GetSection(CacheOptions.Section)
);
builder.Services.Configure<EmailOptions>(
        builder.Configuration.GetSection(EmailOptions.Section)
);
builder.Services.Configure<ApiOptions>(
        builder.Configuration.GetSection(ApiOptions.Section)
);
builder.Services.Configure<ApiClientOptions>("OrdersApi",
        builder.Configuration.GetSection("Apis:Payments")
);

builder.Services.AddSingleton<DatabaseService1>();
builder.Services.AddScoped<DatabaseService2>();
builder.Services.AddSingleton<DatabaseService3>();
builder.Services.AddScoped<IntegrationService>();
builder.Services.AddSingleton<EmailService>();

builder.Services.AddSingleton<OptionsHistoryService>();
builder.Services.AddSingleton<ConfigurationEncryptionService>();
builder.Services.AddSingleton<ConfigurationVersionService>();
builder.Services.AddSingleton<ConfigurationAuditService>();


var app = builder.Build();
app.MapGet("/email", (EmailService emailService) =>
{
    emailService.SendEmail("test@example.com", "Hello", "Test Body");
    return "Email sent (simulated)";
});
app.Run();
