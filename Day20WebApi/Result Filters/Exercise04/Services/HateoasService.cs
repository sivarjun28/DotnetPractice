namespace Exercise04.Services
{
    public class HateoasService
    {
        public object AddLinks(HttpContext context, IEnumerable<object> data)
        {
            var baseUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";

            return data.Select(item => new
            {
                data = item,
                links = new[]
                {
                new { rel = "self", href = baseUrl },
                new { rel = "update", href = baseUrl },
                new { rel = "delete", href = baseUrl }
            }
            });
        }
    }
}