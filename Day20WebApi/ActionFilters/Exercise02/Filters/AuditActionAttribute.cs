using Exercise02.Services;
using Exercise02.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class AuditActionAttribute : ActionFilterAttribute
{
    private readonly IAuditService _auditService;

    public AuditActionAttribute(IAuditService auditService)
    {
        _auditService = auditService;
    }

     public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();

            var entry = new AuditEntry
            {
                Timestamp = DateTime.UtcNow,
                User = context.HttpContext.User.Identity?.Name ?? "Anonymous",
                Controller = context.ActionDescriptor.RouteValues["controller"] ?? "",
                Action = context.ActionDescriptor.RouteValues["action"] ?? "",
                Parameters = context.ActionArguments.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            };

            ActionExecutedContext executedContext = null;
            try
            {
                executedContext = await next();
                entry.StatusCode = executedContext.HttpContext.Response.StatusCode;
            }
            catch (Exception ex)
            {
                entry.Exception = ex.Message;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                entry.DurationMs = stopwatch.ElapsedMilliseconds;
                await _auditService.LogAsync(entry);
            }
        }
    
    }