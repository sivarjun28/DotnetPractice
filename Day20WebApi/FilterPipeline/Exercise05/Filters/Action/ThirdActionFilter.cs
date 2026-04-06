using Microsoft.AspNetCore.Mvc.Filters;

namespace Exercise05.Filters.Action
{
    public class ThirdActionFilter : IActionFilter, IOrderedFilter
{
    public int Order => 3;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("ThirdActionFilter.OnActionExecuting");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine("ThirdActionFilter.OnActionExecuted");
    }
}
}