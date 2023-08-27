using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Integration.Test.ApplicationFactory;
using ErabliereApi.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ErabliereApi.Integration.Test;

public class CreerErabliereTest : IClassFixture<StripeEnabledApplicationFactory<Startup>> 
{
    private readonly StripeEnabledApplicationFactory<Startup> _factory;

    public CreerErabliereTest(StripeEnabledApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreerErabliereAnonyme()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });

        const string nomErabliere = "ErabliereAnonyme";

        using var content = new StringContent(JsonSerializer.Serialize(new PostErabliere
        {
            Nom = nomErabliere,
            AfficherSectionBaril = true,
            AfficherSectionDompeux = true,
            AfficherTrioDonnees = true,
            IndiceOrdre = 0,
            IpRules = "-",
            IsPublic= true,
        }), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/Erablieres", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Vérification BD
        await VerificationBd(response, nomErabliere, isPublic: true, apiKey: null);
    }

    [Fact]
    public async Task CreerErabliereAvecApiKey()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });

        var (customer, apiKey) = await _factory.CreateValidApiKeyAsync();

        client.DefaultRequestHeaders.Add("X-ErabliereApi-ApiKey", apiKey);

        const string nomErabliere = "ErabliereApiKey";

        using var content = new StringContent(JsonSerializer.Serialize(new PostErabliere
        {
            Nom = nomErabliere,
            AfficherSectionBaril = true,
            AfficherSectionDompeux = true,
            AfficherTrioDonnees = true,
            IndiceOrdre = 0,
            IpRules = "-"
        }), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/Erablieres", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Vérification BD
        await VerificationBd(response, nomErabliere, isPublic: false, apiKey: apiKey);

        // Obtenir l'érablière créer
        var responseGet = await client.GetAsync("/Erablieres?my=true");

        var contentGet = await responseGet.Content.ReadAsStringAsync();

        var erablieres = JsonSerializer.Deserialize<List<Erabliere>>(contentGet, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })
            ?? throw new System.InvalidOperationException("Deserializing contentGet result in null reference.");

        Assert.Single(erablieres);
        Assert.Contains(erablieres, e => e.IsPublic == false && e.Nom == nomErabliere);
    }

    private async Task VerificationBd(HttpResponseMessage response, 
                                      string nomErabliere,
                                      bool isPublic,
                                      string? apiKey)
    {
        var context = _factory.Services.GetRequiredService<ErabliereDbContext>();
        var erabliere = await context.Erabliere.FirstOrDefaultAsync(e => e.Nom == nomErabliere);
        Assert.NotNull(erabliere);
        if (erabliere != null)
        {
            Assert.Equal(nomErabliere, erabliere.Nom);
            Assert.True(erabliere.AfficherSectionBaril);
            Assert.True(erabliere.AfficherSectionDompeux);
            Assert.True(erabliere.AfficherTrioDonnees);
            Assert.Equal(0, erabliere.IndiceOrdre);
            Assert.Equal("-", erabliere.IpRule);

            // Valider si l'érablière est publique
            if (isPublic)
            {
                Assert.True(erabliere.IsPublic, "L'érablière doit avoir un status publique lorsque créer par un utilisateur non authentifié");
            }
            else
            {
                Assert.False(erabliere.IsPublic, "L'erabliere ne doit pas avoir le status publique lorsque créé par un utilisateur authentifié");

                if (apiKey != null)
                {
                    var apiKeyService = _factory.Services.GetRequiredService<IApiKeyService>();

                    var hashApiKey = apiKeyService.HashApiKey(apiKey);

                    var customerId = context.ApiKeys.Single(a => a.Key == hashApiKey).CustomerId;

                    var ownership = context.CustomerErablieres.Where(ce => ce.IdErabliere == erabliere.Id &&
                                                                           ce.IdCustomer == customerId).ToArray();

                    Assert.Single(ownership);
                    Assert.Equal(15, ownership[0].Access);
                }
            }

            // Déserialiser la réponse et valider l'id
            var erabliereResponse = JsonSerializer.Deserialize<Erabliere>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.NotNull(erabliereResponse);
            if (erabliereResponse != null)
            {
                Assert.Equal(erabliere.Id, erabliereResponse.Id);
            }
        }
    }
}