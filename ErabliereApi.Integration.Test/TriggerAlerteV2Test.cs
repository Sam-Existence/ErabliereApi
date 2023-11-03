using AutoFixture;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Integration.Test.ApplicationFactory;
using ErabliereApi.Services;
using ErabliereApi.Test.Autofixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Models;
using NSubstitute;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace ErabliereApi.Integration.Test;

public class TriggerAlerteV2Test : IClassFixture<ErabliereApiApplicationFactory<Startup>>
{
    private readonly ErabliereApiApplicationFactory<Startup> _factory;
    private readonly IFixture _fixture;
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public TriggerAlerteV2Test(ErabliereApiApplicationFactory<Startup> factory)
    {
        _factory = factory;
        _fixture = ErabliereFixture.CreerFixture();
    }

    [Fact]
    public async Task WhenMinIsSet()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });

        var id = await CreateErabiere(client);

        // When only min is set
        var capteurId = await CréerCapteurEtAlerte(client, id, 400, null);
        await EnvoyerDonneesCapeur(client, new PostDonneeCapteur
        {
            IdCapteur = capteurId,
            V = 100
        });
        await EnvoyerDonneesCapeur(client, new PostDonneeCapteur
        {
            IdCapteur = capteurId,
            V = 550
        }, noAlerte: true);
    }

    [Fact]
    public async Task WhenMaxIsSet()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });

        var id = await CreateErabiere(client);

        // When only max is set
        var capteurId = await CréerCapteurEtAlerte(client, id, null, 400);
        await EnvoyerDonneesCapeur(client, new PostDonneeCapteur
        {
            IdCapteur = capteurId,
            V = 100
        }, noAlerte: true);
        await EnvoyerDonneesCapeur(client, new PostDonneeCapteur
        {
            IdCapteur = capteurId,
            V = 550
        });
    }

    [Fact]
    public async Task WhenBothMinMaxAreSet()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });

        var id = await CreateErabiere(client);

        // When both values are set on valid values are in between
        var capteurId = await CréerCapteurEtAlerte(client, id, 400, 700);
        await EnvoyerDonneesCapeur(client, new PostDonneeCapteur
        {
            IdCapteur = capteurId,
            V = 100
        });
        await EnvoyerDonneesCapeur(client, new PostDonneeCapteur
        {
            IdCapteur = capteurId,
            V = 900
        });
        await EnvoyerDonneesCapeur(client, new PostDonneeCapteur
        {
            IdCapteur = capteurId,
            V = 550
        }, noAlerte: true);
    }

    private async Task EnvoyerDonneesCapeur(HttpClient client, PostDonneeCapteur postDonneeCapteur, bool noAlerte = false)
    {
        var response = await client.PostAsJsonAsync($"Capteurs/{postDonneeCapteur.IdCapteur}/DonneesCapteur", postDonneeCapteur);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var emailService = _factory.Services.GetRequiredService<IEmailService>();

        if (noAlerte)
        {
            var calls = emailService.ReceivedCalls();
            Assert.Empty(calls);
        }
        else
        {
            var calls = emailService.ReceivedCalls();
            Assert.NotEmpty(calls);
            emailService.ClearReceivedCalls();
        }
    }

    private async Task<Guid> CréerCapteurEtAlerte(HttpClient client, Guid id, short? min, short? max)
    {
        var postCapteur = _fixture.Create<PostCapteur>();
        postCapteur.IdErabliere = id;

        var capteurResponse = await client.PostAsJsonAsync($"Erablieres/{id}/Capteurs", postCapteur, _serializerOptions);

        Assert.NotNull(capteurResponse);

        var capteur = await capteurResponse.Content.ReadFromJsonAsync<Capteur>(_serializerOptions);

        Assert.NotNull(capteur);

        var alerteCapteur = new AlerteCapteur
        {
            IdCapteur = capteur.Id,
            EnvoyerA = "test@test.com",
            MaxValue = max,
            MinVaue = min,
            IsEnable = true,
            Nom = "Humidité sous-sol"
        };

        var alerteResponse = await client.PostAsJsonAsync($"Capteurs/{capteur.Id}/AlerteCapteurs", alerteCapteur, _serializerOptions);

        Assert.NotNull(alerteResponse);

        var alerte = await alerteResponse.Content.ReadFromJsonAsync<AlerteCapteur>(_serializerOptions);

        Assert.NotNull(alerte);

        Assert.NotNull(capteur.Id);

        return capteur.Id.Value;
    }

    private async Task<Guid> CreateErabiere(HttpClient client)
    {
        var initialResponse = await client.GetFromJsonAsync<Erabliere[]>("/Erablieres");

        Assert.NotNull(initialResponse);

        var newErabliere = _fixture.Create<PostErabliere>();

        var postResponse = await client.PostAsJsonAsync("/Erablieres", newErabliere);

        Assert.NotNull(postResponse);

        var newErabliereResponse = await postResponse.Content.ReadFromJsonAsync<Erabliere>(_serializerOptions);

        Assert.NotNull(newErabliereResponse);
        Assert.NotNull(newErabliere.Id);

        return newErabliere.Id.Value;
    }
}
