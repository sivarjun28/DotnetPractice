using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Hosting;

namespace Exercise02.Filters
{

    public class RequireHttpsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            var env = context.HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment)) as IWebHostEnvironment;

            
            if (!context.HttpContext.Request.IsHttps && env?.EnvironmentName != "Development")
            {
                context.Result = new StatusCodeResult(403); 
            }

            base.OnActionExecuting(context);
        }
    }
}