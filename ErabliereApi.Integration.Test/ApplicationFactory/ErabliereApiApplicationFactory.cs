using ErabliereApi.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using System.Collections.Generic;

namespace ErabliereApi.Integration.Test.ApplicationFactory;

public class ErabliereApiApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var config = new Dictionary<string, string>
        {
            { "Stripe.ApiKey", "" },
            { "StripeUsageReccord.SkipRecord", "true" },
            { "ErabliereApiUserService.TestMode", "true" },
            { "USE_SQL", "false" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        builder.UseConfiguration(configuration)
               .ConfigureAppConfiguration(app =>
               {
                   app.AddInMemoryCollection(config);
               });

        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IEmailService>();

            services.AddSingleton(Substitute.For<IEmailService>());

            services.Configure<EmailConfig>(c =>
            {
                c.Email = "";
                c.Sender = "";
            });
        });
    }
}
