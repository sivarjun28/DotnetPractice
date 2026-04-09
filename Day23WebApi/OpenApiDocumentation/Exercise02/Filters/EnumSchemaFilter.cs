using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Exercise02.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum) return;

            schema.Enum.Clear();

            foreach (var name in Enum.GetNames(context.Type))
            {
                schema.Enum.Add(JsonValue.Create(name)); 
            }
        }


    }
}