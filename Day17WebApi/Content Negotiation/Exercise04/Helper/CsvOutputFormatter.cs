using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Exercise04.Helper
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add("text/csv");
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);

        }

        protected override bool CanWriteType(Type? type)
        {
            if (type == null) return false;
            return typeof(IEnumerable<object>).IsAssignableFrom(type) || !type.IsPrimitive;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            if (context.Object is IEnumerable<object> list)
            {
                var firstObj = list.FirstOrDefault();
                if (firstObj != null)
                {
                    var props = firstObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    buffer.AppendLine(string.Join(",", props.Select(p => p.Name)));

                    foreach (var item in list)
                    {
                        var values = props.Select(p => p.GetValue(item)?.ToString()?.Replace(",", ";"));
                        buffer.AppendLine(string.Join(",", values));
                    }
                }
            }
            else
            {
                var props = context.Object.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                buffer.AppendLine(string.Join(",", props.Select(p => p.Name)));
                var values = props.Select(p => p.GetValue(context.Object)?.ToString()?.Replace(",", ";"));
                buffer.AppendLine(string.Join(",", values));
            }

            await response.WriteAsync(buffer.ToString(), selectedEncoding);
        }
    }
}