using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Any;
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

        var enableQueryAttributes = mInfo.GetCustomAttributes(true).OfType<EnableQueryAttribute>();

        var hasODataAttribute = mInfo.DeclaringType?.GetCustomAttributes(true).OfType<ODataOperationFilter>().Any() == true ||
                                enableQueryAttributes.Any();

        if (hasODataAttribute)
        {
            AddODataParameter("$select", format: "string", type: "string");
            AddODataParameter("$filter");
            AddODataParameter("$top", format: "int32", type: "integer");
            AddODataParameter("$skip", format: "int32", type: "integer");

            if (ExpandEnabled(enableQueryAttributes))
            {
                AddODataParameter("$expand");
            }
            
            AddODataParameter("$orderby", format: "string", type: "string"/*, GetOrderByExample()*/);
        }

        void AddODataParameter(string name, string format = "expression", string type = "string", IOpenApiAny? example = default)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = name,
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema
                {
                    Format = format,
                    Type = type
                },
                Example = example
            });
        }
    }

    //private IOpenApiAny? GetOrderByExample()
    //{
    //    return new OpenApiExample
    //    {
    //        Value = new OpenApiString("")
    //    };
    //}

    private bool ExpandEnabled(IEnumerable<EnableQueryAttribute> enableQueryAttributes)
    {
        var enableQueryAttribute = enableQueryAttributes.Single();

        return enableQueryAttribute.MaxExpansionDepth > 0;
    }
}
