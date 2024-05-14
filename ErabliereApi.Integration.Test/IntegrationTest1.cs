using ErabliereApi.Integration.Test.ApplicationFactory;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System;
using System.Net;
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

        var response = await client.PostAsync("/Checkout", content);

        response.StatusCode
            .ShouldBe(HttpStatusCode.InternalServerError, 
            await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task OpenApiSpecIsAvailable()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 1
        });

        var response = await client.GetAsync("/api/v1/swagger.json");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task ODataSelectIdQuery()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 1
        });

        var response = await client.GetAsync("/Erablieres?$select=id");

        var content = await response.Content.ReadAsStringAsync();

        Console.WriteLine(content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("alertes")]
    [InlineData("notes")]
    [InlineData("documentations")]
    [InlineData("alertes,notes,documentations")]
    public async Task ODataExpandProhibited(string expand)
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 1
        });

        var response = await client.GetAsync($"/Erablieres?$expand={expand}");

        var content = await response.Content.ReadAsStringAsync();

        Console.WriteLine(content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
