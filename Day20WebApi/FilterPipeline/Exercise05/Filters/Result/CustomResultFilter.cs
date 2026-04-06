using Microsoft.AspNetCore.Mvc.Filters;

namespace Exercise05.Filters.Result
{
    public class CustomResultFilter : IResultFilter, IOrderedFilter
{
    public int Order => 5;

    public void OnResultExecuting(ResultExecutingContext context)
    {
        Console.WriteLine("ResultFilter.OnResultExecuting");
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        Console.WriteLine("ResultFilter.OnResultExecuted");
    }
}
}