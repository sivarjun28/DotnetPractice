using Microsoft.AspNetCore.Mvc.Filters;

namespace Exercise05.Filters.Resource
{
    public class CustomResourceFilter : IResourceFilter, IOrderedFilter
{
    public int Order => 3;

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        Console.WriteLine("ResourceFilter.OnResourceExecuting");
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        Console.WriteLine("ResourceFilter.OnResourceExecuted");
    }
}
}