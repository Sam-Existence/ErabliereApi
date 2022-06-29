using ErabliereApi.Donnees.Ownable;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ErabliereApi.Attributes;

public class ValiderOwnershipLevelTwoAttribute : ActionFilterAttribute
{
    private readonly string _idParamName;
    private readonly Type _levelTwoOwnableType;

    public ValiderOwnershipLevelTwoAttribute(string idParamName, Type levelTwoOwnaleType)
    {
        if (!levelTwoOwnaleType.GetInterfaces().Any(i => i == typeof(ILevelTwoOwnable<>)))
        {
            throw new ArgumentException($"The type of arg {nameof(levelTwoOwnaleType)} must implement {nameof(ILevelTwoOwnable<IErabliereOwnable>)}");
        }

        _idParamName = idParamName;
        _levelTwoOwnableType = levelTwoOwnaleType;
    }

    public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        return base.OnActionExecutionAsync(context, next);
    }
}
