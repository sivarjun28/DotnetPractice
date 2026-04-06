using Microsoft.AspNetCore.Mvc.Filters;

namespace Exercise05.Filters.Authorization
{
   

public class CustomAuthorizationFilter : IAuthorizationFilter, IOrderedFilter
{
    public int Order => 2;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        Console.WriteLine("AuthorizationFilter.OnAuthorization");

        
    }
}
}