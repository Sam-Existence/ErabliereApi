using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ErabliereApi.OperationFilter;

/// <summary>
/// Operation filter permettant d'ajouter les paramètres de OData à la page swagger
/// </summary>
public class ODataOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var mInfo = context.MethodInfo;

        var hasODataAttribute = mInfo.DeclaringType?.GetCustomAttributes(true).OfType<ODataOperationFilter>().Any() == true ||
                                mInfo.GetCustomAttributes(true).OfType<EnableQueryAttribute>().Any();

        if (hasODataAttribute)
        {
            AddODataParameter("$select", format: "string", type: "string");
            AddODataParameter("$filter");
            AddODataParameter("$top", format: "int32", type: "integer");
            AddODataParameter("$skip", format: "int32", type: "integer");
            AddODataParameter("$expand");
        }

        void AddODataParameter(string name, string format = "expression", string type = "string")
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = name,
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema
                {
                    Format = format,
                    Type = type
                }
            });
        }
    }
}
