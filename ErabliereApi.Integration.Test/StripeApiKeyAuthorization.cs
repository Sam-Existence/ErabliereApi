using ErabliereApi.Integration.Test.ApplicationFactory;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using Microsoft.Extensions.DependencyInjection;
using ErabliereApi.Services;
using ErabliereApi.Depot.Sql;

namespace ErabliereApi.Integration.Test;
public class StripeApiKeyAuthorization : IClassFixture<StripeEnabledApplicationFactory<Startup>>
{
    private readonly StripeEnabledApplicationFactory<Startup> _factory;

    public StripeApiKeyAuthorization(StripeEnabledApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GoodApiKey_Get200()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });

        var (customer, key) = await _factory.CreateValidApiKeyAsync();

        using var content = new StringContent(JsonSerializer.Serialize(new PostErabliere
        {
            Nom = Guid.NewGuid().ToString()
        }), Encoding.UTF8, "application/json");

        content.Headers.Add("X-ErabliereApi-ApiKey", key);

        var response = await client.PostAsync("/Erablieres", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }    

    [Fact]
    public async Task DeletedApiKey_Get403()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });

        using var scope = _factory.Services.CreateScope();

        var apiKeyService = scope.ServiceProvider.GetRequiredService<IApiKeyService>();

        var context = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

        var key = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        context.ApiKeys.Add(new ApiKey
        {
            CreationTime = DateTimeOffset.Now,
            Key = apiKeyService.HashApiKey(key),
            DeletionTime = DateTimeOffset.Now
        });

        context.SaveChanges();

        using var content = new StringContent(JsonSerializer.Serialize(new PostErabliere
        {
            Nom = Guid.NewGuid().ToString()
        }), Encoding.UTF8, "application/json");

        content.Headers.Add("X-ErabliereApi-ApiKey", key);

        var response = await client.PostAsync("/Erablieres", content);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task RevokedApiKey_Get403()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });

        using var scope = _factory.Services.CreateScope();

        var apiKeyService = scope.ServiceProvider.GetRequiredService<IApiKeyService>();

        var context = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

        var key = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        context.ApiKeys.Add(new ApiKey
        {
             CreationTime = DateTimeOffset.Now,
             Key = apiKeyService.HashApiKey(key),
             RevocationTime = DateTimeOffset.Now
        });

        context.SaveChanges();

        using var content = new StringContent(JsonSerializer.Serialize(new PostErabliere
        {
            Nom = Guid.NewGuid().ToString()
        }), Encoding.UTF8, "application/json");

        content.Headers.Add("X-ErabliereApi-ApiKey", key);

        var response = await client.PostAsync("/Erablieres", content);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task SendRandomApiKey_Get403()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });

        using var content = new StringContent(JsonSerializer.Serialize(new PostErabliere
        {
            Nom = Guid.NewGuid().ToString()
        }), Encoding.UTF8, "application/json");

        content.Headers.Add("X-ErabliereApi-ApiKey", Guid.NewGuid().ToString());

        var response = await client.PostAsync("/Erablieres", content);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
