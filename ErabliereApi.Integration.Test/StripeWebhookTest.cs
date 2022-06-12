using ErabliereApi.Depot.Sql;
using ErabliereApi.Integration.Test.ApplicationFactory;
using ErabliereApi.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ErabliereApi.Integration.Test;

public class StripeWebhookTest : IClassFixture<StripeEnabledApplicationFactory<Startup>>
{
    private readonly StripeEnabledApplicationFactory<Startup> _factory;

    public StripeWebhookTest(StripeEnabledApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task NominalFlow()
    {
        await Task.Delay(4000);

        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });

        AssertCustomerDoesntExist();

        await Task.WhenAll(new Task[]
        {
            Step("setup_intent.succeeded", client),
            Step("setup_intent.created", client),
            Step("checkout.session.completed", client),
            Step("customer.created", client).ContinueWith(task =>
            {
                AssertCustomerExist();

                return Task.CompletedTask;
            }),
            Step("payment_method.attached", client),
            Step("customer.updated", client),
            Step("invoice.created", client),
            Step("invoice.finalized", client),
            Step("invoice.paid", client).ContinueWith(task =>
            {
                AssertCustomerApiKey();

                return Task.CompletedTask;
            }),
            Step("invoice.payment_succeeded", client),
            Step("customer.subscription.created", client).ContinueWith(task =>
            {
                AssertApiKeySubscriptionKey();

                return Task.CompletedTask;
            })
        });
    }

    private void AssertApiKeySubscriptionKey()
    {
        var database = _factory.Services.GetRequiredService<ErabliereDbContext>();

        var customer = database.Customers.Single(c => c.Email == "john@doe.com");

        var apiKeys = database.ApiKeys.Where(a => a.CustomerId == customer.Id).ToArray();

        var apiKey = Assert.Single(apiKeys);

        Assert.Equal("sub_nQGAY1IJGYRuKALqHnDOC9yt", apiKey.SubscriptionId);
    }

    private void AssertCustomerApiKey()
    {
        using var scope = _factory.Services.CreateScope();

        var database = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

        var customer = database.Customers.Single(c => c.Email == "john@doe.com");

        var apiKeys = database.ApiKeys.Where(a => a.CustomerId == customer.Id).ToArray();

        var apiKey = Assert.Single(apiKeys);

        Assert.NotNull(apiKey.Key);
        Assert.Null(apiKey.RevocationTime);
        Assert.Null(apiKey.DeletionTime);
        Assert.Null(apiKey.SubscriptionId);

        var emailService = _factory.Services.GetRequiredService<IEmailService>();

        emailService.ReceivedWithAnyArgs(1);
    }

    private void AssertCustomerDoesntExist()
    {
        using var scope = _factory.Services.CreateScope();

        var database = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

        var customer = database.Customers.SingleOrDefault(c => c.Email == "john@doe.com");

        Assert.Null(customer);
    }

    private void AssertCustomerExist()
    {
        using var scope = _factory.Services.CreateScope();

        var database = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

        var customer = database.Customers.Single(c => c.Email == "john@doe.com");

        Assert.NotNull(customer.Id);
        Assert.Equal("John Doe", customer.Name);
        Assert.Equal("Stripe.Customer", customer.AccountType);
        Assert.Equal("cus_2PXvRa6ztL96bV", customer.StripeId);
    }

    private async Task Step(string step, HttpClient client)
    {
        using var content = new StringContent(GetBody(step));

        var response = await client.PostAsync("/Checkout/Webhook", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    public static string GetBody(string step)
    {
        return File.ReadAllText(Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "StripeWebhookJson",
            "StripeWebhookTest",
            "NominalFlow",
            step + ".body.json"));
    }

    public static string GetEvent(string step)
    {
        return File.ReadAllText(Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "StripeWebhookJson",
            "StripeWebhookTest",
            "NominalFlow",
            step + ".json"));
    }
}
