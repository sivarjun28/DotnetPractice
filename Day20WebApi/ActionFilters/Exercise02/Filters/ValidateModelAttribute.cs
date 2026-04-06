using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Exercise02.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.
                                ModelState
                                .Where(x => x.Value.Errors.Count > 0)
                                .ToDictionary(
                                    kvp => kvp.Key,
                                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                );
                var problemDetails = new ValidationProblemDetails(errors)
                {
                    Status = 400,
                    Title = "Model validation Failed"
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            base.OnActionExecuting(context);
        }
    }
}