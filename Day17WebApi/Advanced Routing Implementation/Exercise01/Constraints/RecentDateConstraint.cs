using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace Exercise01.Constraints
{
    public class RecentDateConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
                          RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(routeKey)) return false;

            if (!DateTime.TryParse(values[routeKey]?.ToString(), out var date))
                return false;

            // Allow last 10 years for testing
            return date >= DateTime.Now.AddYears(-10) && date <= DateTime.Now.AddYears(1);
        }
    }
}