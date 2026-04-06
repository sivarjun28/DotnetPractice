using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Exercise03.Filters
{
    public class BusinessRuleExceptionFilter : IExceptionFilter
    {

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not BusinessRuleException businessRuleException)
                return;
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Business Rule Violation",
                Detail = businessRuleException.Message,
            };

            problemDetails.Extensions["Rulecode"] = businessRuleException.RuleCode;
            problemDetails.Extensions["details"] = businessRuleException.Details;
            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };
            context.ExceptionHandled = true;
        }
    }
    public class BusinessRuleException : Exception
    {
        public string RuleCode { get; }
        public Dictionary<string, object> Details { get; }

        public BusinessRuleException(string ruleCode, string message, Dictionary<string, object>? details = null)
            : base(message)
        {
            RuleCode = ruleCode;
            Details = details ?? new Dictionary<string, object>();
        }
    }
}