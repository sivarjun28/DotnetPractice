using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Exercise02.Filters
{

using Swashbuckle.AspNetCore.SwaggerGen;

public class AddResponseHeadersFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!operation.Responses.ContainsKey("200"))
            return;

        var response = operation.Responses["200"];


        response.Headers.Add("X-Request-ID", new OpenApiHeader
        {
            Description = "Request correlation ID",
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.String
            }
        });

        response.Headers.Add("X-Processing-Time", new OpenApiHeader
        {
            Description = "Processing time in ms",
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.Integer,
                Format = "int32"
            }
        });
    }
}
}