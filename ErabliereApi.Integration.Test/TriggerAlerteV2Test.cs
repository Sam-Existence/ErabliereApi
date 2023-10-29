using AutoFixture;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Integration.Test.ApplicationFactory;
using ErabliereApi.Test.Autofixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Graph.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

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
    public async Task ValidateTriggerAlerteV2()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });

        var id = await CreateErabiere(client);
        await CréerCapteurEtAlerte(client, id);

        // TODO: Tester les différents scénarios (voir issue)
        // https://github.com/freddycoder/ErabliereApi/issues/200
        // When only min is set
        // When only max is set
        // When both values are set on valid values are in between
        // When both values are set and valid values are outside both values
    }

    private async Task CréerCapteurEtAlerte(HttpClient client, Guid id)
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
            MaxValue = 700,
            MinVaue = 400,
            IsEnable = true,
            Nom = "Humidité sous-sol"
        };

        var alerteResponse = await client.PostAsJsonAsync($"Capteurs/{capteur.Id}/AlerteCapteurs", alerteCapteur, _serializerOptions);

        Assert.NotNull(alerteResponse);

        var alerte = await alerteResponse.Content.ReadFromJsonAsync<AlerteCapteur>(_serializerOptions);

        Assert.NotNull(alerte);
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
