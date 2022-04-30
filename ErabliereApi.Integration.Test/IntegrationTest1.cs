using ErabliereApi.Integration.Test.ApplicationFactory;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ErabliereApi.Integration.Test;

public class IntegrationTest1 : IClassFixture<ErabliereApiApplicationFactory<Startup>>
{
    private readonly ErabliereApiApplicationFactory<Startup> _factory;

    public IntegrationTest1(ErabliereApiApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetErablieres_ReturnSuccessStatusCode()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });

        var response = await client.GetAsync("/Erablieres");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task ByDefault_ThereIsNoCheckoutControllerEnabled()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });

        using var content = new StringContent("");

        InvalidOperationException? e = null;

        try
        {
            var response = await client.PostAsync("/Checkout", content);
        }
        catch (InvalidOperationException exception)
        {
            e = exception;
        }

        Assert.NotNull(e);
    }
}
