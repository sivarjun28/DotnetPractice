using Microsoft.AspNetCore.Mvc;
namespace Exercise04.ProblemDetail
{
    public class ProblemDetailsFactory
    {
        public static ProblemDetails CreateNotFound(
            string resourceType,
            object id,
            string instance)
        {
            return new ProblemDetails
            {
                Status = 404,
                Title = "Not Found",
                Type = "/errors/not-found",
                Detail = $"{resourceType} with ID {id} was not found.",
                Instance = instance
            };
        }
        public static ValidationProblemDetails CreateValidation(IDictionary<string, string[]> errors, string instance)
        {
            var validationProblemDetails = new ValidationProblemDetails(errors)
            {
                Status = 400,
                Title = "Validation Error",
                Type = "/errors/validation",
                Instance = instance
            };
            return validationProblemDetails;
        }

        public static ProblemDetails CreateConflict(string message, string instance)
        {
            return new ProblemDetails
            {
                Status = 409,
                Title = "/conflict",
                Type = "/errors/conflict",
                Detail = message,
                Instance = instance
            };
        }
        public static ProblemDetails CreateBusinessRule(
       string ruleCode,
       string message,
       string instance)
        {
            // TODO: Create 422 unprocessable entity problem details
            return new BusinessRuleProblemDetails
            {
                Status = 422,
                Title = "Business Rule Violation",
                Type = "/errors/business-rule",
                Detail = message,
                Instance = instance,
                RuleCode = ruleCode,
                RuleData = new Dictionary<string, object>
            {
                { "ruleCode", ruleCode },
                { "message", message }
            }
            };
        }

        public static ProblemDetails CreateInternalError(
            string instance,
            bool includeDetails = false,
            Exception? exception = null)
        {
            // TODO: Create 500 internal error problem details
            // Only include exception details in development
            var problemDetails = new ProblemDetails
            {
                Status = 500,
                Title = "internal Server Error",
                Type = "/errors/internal-server-error",
                Detail = "An unexpected error occurred on the server.",
                Instance = instance
            };
            if (includeDetails & exception != null)
            {
                problemDetails.Extensions["exceptionDetails"] = exception.ToString();
            }
            return problemDetails;
        }
    }
}