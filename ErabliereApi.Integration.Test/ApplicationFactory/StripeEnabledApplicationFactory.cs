using ErabliereApi.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;

namespace ErabliereApi.Integration.Test.ApplicationFactory;

public class StripeEnabledApplicationFactory<TStartup> : ErabliereApiApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<ICheckoutService>();
            services.TryAddScoped(sp => Substitute.For<ICheckoutService>());
        });
    }
}
