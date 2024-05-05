using Microsoft.AspNetCore.HttpOverrides;
using System.Net;

namespace ErabliereApi.Extensions;

/// <summary>
/// Class containing extention method to add Forwarded Headers middleware base on environment variable of the app.
/// </summary>
public static class UseForwardedHeadersExtension
{
    /// <summary>
    /// Add forwarded headers services necessary when using the app behind a reverse proxy
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddErabliereAPIForwardedHeaders(this IServiceCollection services, IConfiguration configuration)
    {
        if (string.Equals(configuration.GetValue<string>("Forwarded_headers"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                string[] array = configuration.GetValue<string>("KNOW_NETWORKS")?.Split(';') ?? Array.Empty<string>();
                for (int i = 0; i < array.Length; i++)
                {
                    string? network = array[i];
                    var ipInfo = network.Split("/");

                    options.KnownNetworks.Add(new Microsoft.AspNetCore.HttpOverrides.IPNetwork(IPAddress.Parse(ipInfo[0]), int.Parse(ipInfo[1])));
                }
            });
        }

        return services;
    }

    /// <summary>
    /// Add forwarded headers middleware necessary when using the app behind a reverse proxy
    /// </summary>
    /// <param name="app"></param>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseErabliereAPIForwardedHeadersRules(this IApplicationBuilder app, ILogger<Startup> logger, IConfiguration configuration)
    {
        if (string.Equals(configuration.GetValue<string>("Forwarded_headers"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
        {
            app.UseForwardedHeaders();

            DebugHeaders(app, logger, configuration);

            if (string.Equals(configuration.GetValue<string>("USE_SCHEMA_FROM_PROXY"), bool.TrueString, StringComparison.CurrentCultureIgnoreCase))
            {
                app.Use(async (context, next) =>
                {
                    if (context.Request.Headers.TryGetValue("X-Forwarded-Proto", out var forwardedProto))
                    {
                        context.Request.Scheme = forwardedProto.ToString();
                    }

                    await next();
                });
            }
        }
        else if (string.Equals(configuration.GetValue<string>("DEBUG_HEADERS"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
        {
            DebugHeadersMiddleware(app, logger);
        }

        return app;
    }

    private static void DebugHeaders(IApplicationBuilder app, ILogger<Startup> logger, IConfiguration configuration)
    {
        if (string.Equals(configuration.GetValue<string>("Forwarded_headers.Debug_headers"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
        {
            DebugHeadersMiddleware(app, logger);
        }
    }

    private static void DebugHeadersMiddleware(IApplicationBuilder app, ILogger<Startup> logger)
    {
        app.Use(async (context, next) =>
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                // Headers
                // Request method, scheme, and path
                logger.LogDebug("Request Method: {Method}", SanatizeHeader(context.Request.Method));
                logger.LogDebug("Request Scheme: {Scheme}", SanatizeHeader(context.Request.Scheme));
                logger.LogDebug("Request Path: {Path}", SanatizeHeader(context.Request.Path));

                foreach (var header in context.Request.Headers)
                {
                    logger.LogDebug(
                        "Header: {Key}: {Value}", 
                        SanatizeHeader(header.Key), 
                        SanatizeHeader(header.Value));
                }

                // Connection: RemoteIp
                logger.LogDebug("Request RemoteIp: {RemoteIpAddress}",
                context.Connection.RemoteIpAddress);
            }

            await next();
        });
    }

    private static string? SanatizeHeader(string? value) 
    {
        return value?
            .Replace("\r\n", string.Empty)
            .Replace("\n", string.Empty)
            .Replace("\r", string.Empty);
    }
}
