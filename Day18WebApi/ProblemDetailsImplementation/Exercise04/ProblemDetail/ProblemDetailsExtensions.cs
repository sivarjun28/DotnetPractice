using Microsoft.AspNetCore.Mvc;

namespace Exercise04.ProblemDetail
{
    public static class ProblemDetailsExtensions
    {
        public static ProblemDetails WithTraceId(
            this ProblemDetails problemDetails,
            string traceId)
        {
            problemDetails.Extensions["traceId"] = traceId;
            return problemDetails;
        }

        public static ProblemDetails WithTimestamp(
            this ProblemDetails problemDetails)
        {
            problemDetails.Extensions["timestamp"] = DateTime.UtcNow;
            return problemDetails;
        }

        public static ProblemDetails WithHelpLink(
            this ProblemDetails problemDetails,
            string helpLink)
        {
            problemDetails.Extensions["helpLink"] = helpLink;
            return problemDetails;
        }
    }
}