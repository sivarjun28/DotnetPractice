using Microsoft.AspNetCore.Mvc.Filters;

namespace Exercise05.Filters.Action
{
    public class SecondActionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => 2;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("SecondActionFilter.OnActionExecuting");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("SecondActionFilter.OnActionExecuted");
        }
    }
}