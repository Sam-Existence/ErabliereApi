using AutoMapper;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Stripe;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
            services.Configure<EmailConfig>(email =>
            {
                email.Sender = "Sender";
                email.SmtpServer = "SmtpServer";
                email.Email = "Email";
                email.Password = "Password";
                email.SmtpPort = 597;
            });

            services.RemoveAll<IEmailService>();
            services.TryAddScoped(sp =>
            {
                return Substitute.For<IEmailService>();
            });

            services.RemoveAll<ICheckoutService>();
            services.TryAddTransient(sp =>
            {
                var checkoutService = Substitute.For<ICheckoutService>();

                checkoutService.CreateSessionAsync(Arg.Any<CancellationToken>()).Returns(session =>
                {
                    return new Stripe.ReviewSession();
                });

                checkoutService.Webhook(Arg.Any<string>()).Returns(callInfo =>
                {
                    return Task.Run(async () =>
                    {
                        var json = JsonDocument.Parse(callInfo.Args()[0] as string ?? "");

                        var eventDeserialized = EventUtility.ParseEvent(json.RootElement.ToString());

                        if (eventDeserialized is null)
                        {
                            throw new ArgumentNullException(nameof(eventDeserialized));
                        }

                        await StripeCheckoutService.WebHookSwitchCaseLogic(
                            eventDeserialized,
                            sp.GetRequiredService<ILogger<StripeCheckoutService>>(),
                            sp.GetRequiredService<IMapper>(),
                            sp.GetRequiredService<IUserService>(),
                            sp.GetRequiredService<IApiKeyService>(),
                            CancellationToken.None);
                    });
                });

                return checkoutService;
            });
        });
    }

    /// <summary>
    /// Fonction utilitaire permettant de ne pas dupliquer la logique d'ajout de clé d'api
    /// lors de test d'intégration
    /// </summary>
    /// <returns>La clé d'api créer pouvant être ajouter en entête lors des appels http</returns>
    public string CreateValidApiKey()
    {
        var apiKeyService = Services.GetRequiredService<IApiKeyService>();

        var context = Services.GetRequiredService<ErabliereDbContext>();

        var key = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        var subId = Guid.NewGuid().ToString();

        context.ApiKeys.Add(new ApiKey
        {
            CreationTime = DateTimeOffset.Now,
            Key = apiKeyService.HashApiKey(key),
            SubscriptionId = subId
        });

        context.SaveChanges();

        return key;
    }
}
