using ErabliereApi;
using ErabliereApi.StripeIntegration;

var builder =  WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var webApp = builder.Build();

var logger = webApp.Services.GetRequiredService<ILogger<Program>>();

try
{
    using (var scope = webApp.Services.CreateScope())
    {
        startup.Configure(webApp, webApp.Environment, scope.ServiceProvider);
    }

    var task = webApp.UseStripeUsageReccordTask();

    Console.WriteLine("Starting ErabliereApi");

    await task.RunAsync();
}
catch (Exception e)
{
    logger.LogCritical(e, "Error in Program.cs");

    throw;
}