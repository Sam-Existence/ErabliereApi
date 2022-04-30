using ErabliereApi.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NSubstitute;

namespace ErabliereApi.Integration.Test.ApplicationFactory;

public class StripeEnabledApplicationFactory<TStartup> : ErabliereApiApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(webBuilder =>
        {
            webBuilder.AddCommandLine(new[]
            {
                "Stripe.ApiKey=abcd"
            });
        });

        base.ConfigureWebHost(builder);

        builder.ConfigureServices((webContext, services) =>
        {
            services.RemoveAll<ICheckoutService>();
            services.TryAddScoped(sp =>
            {
                var checkoutService = Substitute.For<ICheckoutService>();

                checkoutService.CreateSessionAsync().Returns(session =>
                {
                    return new Stripe.ReviewSession();
                });

                return checkoutService;
            });
        });
    }
}
