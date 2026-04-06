using Microsoft.AspNetCore.Mvc.Filters;
namespace Exercise05.Filters.Action
{
    public class FirstActionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => 1;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("FirstActionFilter.OnActionExecuting");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("FirstActionFilter.OnActionExecuted");
        }
    }
}