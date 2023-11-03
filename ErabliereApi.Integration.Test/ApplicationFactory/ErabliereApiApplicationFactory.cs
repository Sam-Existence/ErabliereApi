using ErabliereApi.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;

namespace ErabliereApi.Integration.Test.ApplicationFactory;

public class ErabliereApiApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
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
