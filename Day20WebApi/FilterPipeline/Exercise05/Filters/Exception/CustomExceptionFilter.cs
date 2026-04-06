using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Exercise05.Filters.Exception
{
    public class CustomExceptionFilter : IExceptionFilter, IOrderedFilter
{
    public int Order => 1;

    public void OnException(ExceptionContext context)
    {
        Console.WriteLine("ExceptionFilter.OnException");

        context.Result = new ObjectResult("Exception handled")
        {
            StatusCode = 500
        };
    }
}
}