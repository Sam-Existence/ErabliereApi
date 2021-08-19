using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using System.Threading.Tasks;

namespace ErabliereApi
{
    internal class MiniProfilerAsyncLogger : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<MiniProfilerAsyncLogger>>();

            using (MiniProfiler.Current.Step(context.ActionDescriptor.DisplayName))
            {
                await next();
            }

            logger.LogInformation(MiniProfiler.Current.RenderPlainText());
        }
    }
}