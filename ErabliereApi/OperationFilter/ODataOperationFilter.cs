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
            AddODataParameter("$select", format: "string", type: "string", examples: GetSelectExamples());
            AddODataParameter("$filter", examples: GetFilterExamples());
            AddODataParameter("$top", format: "int32", type: "integer", examples: GetTopExamples(), defaultValue: "10");
            AddODataParameter("$skip", format: "int32", type: "integer", examples: GetSkipExamples());

            if (ExpandEnabled(enableQueryAttributes))
            {
                AddODataParameter("$expand", examples: GetExpandExamples());
            }

            AddODataParameter("$orderby", format: "string", type: "string", GetOrderByExamples());
        }

        void AddODataParameter(string name, string format = "expression", string type = "string", Dictionary<string, OpenApiExample>? examples = default, string? defaultValue = null)
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
                Examples = examples,
                Example = defaultValue != null ? new OpenApiString(defaultValue) : null
            });
        }
    }

    private Dictionary<string, OpenApiExample>? GetOrderByExamples()
    {
        var examples = new Dictionary<string, OpenApiExample>
            {
                { "empty", new OpenApiExample { Value = new OpenApiString("") } },
                { "asc", new OpenApiExample { Value = new OpenApiString("propertyName") } },
                { "desc", new OpenApiExample { Value = new OpenApiString("propertyName desc") } },
                { "complexe sort", new OpenApiExample { Value = new OpenApiString("propertyOne asc, propertyTwo desc") } }
            };

        return examples;
    }

    private Dictionary<string, OpenApiExample>? GetExpandExamples()
    {
        var examples = new Dictionary<string, OpenApiExample>
            {
                { "empty", new OpenApiExample { Value = new OpenApiString("") } },
                { "expand one property", new OpenApiExample { Value = new OpenApiString("propertyName") } },
                { "expand multiple property", new OpenApiExample { Value = new OpenApiString("propertyOne,propertyTwo") } },
                { "expand nested property", new OpenApiExample { Value = new OpenApiString("modelSourceProperty/childOneProperty/childTwoProperty") } }
            };

        return examples;
    }

    private Dictionary<string, OpenApiExample>? GetSkipExamples()
    {
        var examples = new Dictionary<string, OpenApiExample>
            {
                { "empty", new OpenApiExample { Value = new OpenApiString("") } },
                { "skip 10", new OpenApiExample { Value = new OpenApiString("10") } }
            };

        return examples;
    }

    private Dictionary<string, OpenApiExample>? GetTopExamples()
    {
        var examples = new Dictionary<string, OpenApiExample>
            {
                { "empty", new OpenApiExample { Value = new OpenApiString("") } },
                { "take 10", new OpenApiExample { Value = new OpenApiString("10") } }
            };

        return examples;
    }

    private Dictionary<string, OpenApiExample>? GetFilterExamples()
    {
        var examples = new Dictionary<string, OpenApiExample>
            {
                { "empty", new OpenApiExample { Value = new OpenApiString("") } },
                { "equal", new OpenApiExample { Value = new OpenApiString("propertyName eq 'Some value'") } },
                { "and", new OpenApiExample { Value = new OpenApiString("propertyOne eq 'Some value' and propertyTwo eq 'other Value'") } },
                { "or", new OpenApiExample { Value = new OpenApiString("propertyOne eq 'Some value' or propertyTwo eq 'other Value'") } },
                { "less than", new OpenApiExample { Value = new OpenApiString("propertyName lt 610") } },
                { "greater than", new OpenApiExample { Value = new OpenApiString("propertyName gt 610") } },
                { "less or equal than", new OpenApiExample { Value = new OpenApiString("propertyName le 610") } },
                { "greater or equal than", new OpenApiExample { Value = new OpenApiString("propertyName ge 610") } },
                { "not equal", new OpenApiExample { Value = new OpenApiString("propertyName ne 610") } },
                { "endswith", new OpenApiExample { Value = new OpenApiString("endwith(propertyName, 'value') eq true") } },
                { "startswith", new OpenApiExample { Value = new OpenApiString("startswith(propertyName, 'value') eq true") } },
                { "contains", new OpenApiExample { Value = new OpenApiString("contains(propertyName, 'value') eq true") } },
                { "indexof", new OpenApiExample { Value = new OpenApiString("indexof(propertyName, 'VALUE') eq 0") } },
                { "replace", new OpenApiExample { Value = new OpenApiString("replace(propertyName, 'VALUE', 'VALUEREPLACED') eq 'OTHER VALUE'") } },
                { "substring", new OpenApiExample { Value = new OpenApiString("substring(propertyName, 'OTHERVALUE') eq 'VALUE'") } },
                { "tolower", new OpenApiExample { Value = new OpenApiString("tolower(propertyName) eq 'VALUE'") } },
                { "toupper", new OpenApiExample { Value = new OpenApiString("toupper(propertyName) eq 'VALUE'") } },
                { "trim", new OpenApiExample { Value = new OpenApiString("trim(propertyName) eq 'VALUE'") } },
                { "concat", new OpenApiExample { Value = new OpenApiString("concat(concat(PropertyText1, ', '), PropertyText2) eq 'value1, value2'") } },
                { "floor", new OpenApiExample { Value = new OpenApiString("floor(propertyDecimal) eq 1") } },
                { "ceiling", new OpenApiExample { Value = new OpenApiString("ceiling(propertyDecimal) eq 1") } },
            };

        return examples;
    }

    private Dictionary<string, OpenApiExample>? GetSelectExamples()
    {
        var examples = new Dictionary<string, OpenApiExample>
            {
                { "empty", new OpenApiExample { Value = new OpenApiString("") } },
                { "select one property", new OpenApiExample { Value = new OpenApiString("propertyName") } },
                { "select multiple property", new OpenApiExample { Value = new OpenApiString("propertyOne,propertyTwo") } }
            };

        return examples;
    }

    private bool ExpandEnabled(IEnumerable<EnableQueryAttribute> enableQueryAttributes)
    {
        var enableQueryAttribute = enableQueryAttributes.Single();

        return enableQueryAttribute.MaxExpansionDepth > 0;
    }
}
