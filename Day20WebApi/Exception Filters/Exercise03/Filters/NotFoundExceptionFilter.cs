using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Exercise03.Filters
{
    public class NotFoundExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not NotFoundException notFoundException)
                return;

          var problemDetails = new ProblemDetails
          {
              Status = StatusCodes.Status404NotFound,
              Title = "Resource Not Found",
              Detail = $"The {notFoundException.ResourceType} with identifier {notFoundException.ResourceId} was not found",
              Instance = context.HttpContext.Request.Path
          };
          context.Result = new NotFoundObjectResult(problemDetails);
          context.ExceptionHandled = true;

           
        }
    }

    public class NotFoundException : Exception
    {
        public string ResourceType { get; }
        public object ResourceId { get; }

        public NotFoundException(string resourceType, object resourceId)
            : base($"{resourceType} with ID '{resourceId}' was not found.")
        {
            ResourceType = resourceType;
            ResourceId = resourceId;
        }
    }
}