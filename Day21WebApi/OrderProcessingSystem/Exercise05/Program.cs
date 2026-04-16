using Exercise05.Models;
using Exercise05.Processors;
using Exercise05.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IInventoryService, InventoryService>();
builder.Services.AddSingleton<IPaymentService, PaymentService>();
builder.Services.AddSingleton<IShippingService, ShippingService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddSingleton<IOrderProcessor, OrderProcessor>();

builder.Services.AddLogging(); 


var app = builder.Build();