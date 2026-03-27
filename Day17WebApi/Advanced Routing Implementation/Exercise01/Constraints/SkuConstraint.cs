using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace Exercise01.Constraints
{
    public class SkuConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
                          RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(routeKey)) return false;
            var sku = values[routeKey]?.ToString();
            return !string.IsNullOrEmpty(sku) && Regex.IsMatch(sku, @"^[A-Z]{2}\d{4}$");
        }
    }
}