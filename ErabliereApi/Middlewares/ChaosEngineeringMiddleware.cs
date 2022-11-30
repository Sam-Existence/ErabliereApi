namespace ErabliereApi.Middlewares;

/// <summary>
/// Middleware pour simuler des erreurs aléatoires
/// </summary>
public class ChaosEngineeringMiddleware : IMiddleware
{
    private static readonly Random random = new Random();
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructeur
    /// </summary>
    public ChaosEngineeringMiddleware(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Méthode d'entrée du middleware
    /// </summary>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var chaos = _configuration.GetValue<double>("ChaosEngineeringPercent");

        if (random.NextDouble() < chaos)
        {
            // return a random error
            context.Response.StatusCode = random.Next(400, 599);
            return;
        }

        await next(context);
    }
}
