// SwaggerDefaultValues.cs

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyMovies;

public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;
        operation.Deprecated |= apiDescription.IsDeprecated;
    }
}