using ErabliereApi.Integration.Test.ApplicationFactory;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace ErabliereApi.Integration.Test;
public class OdataHttpQueryTest : IClassFixture<ErabliereApiApplicationFactory<Startup>>
{
    private readonly ErabliereApiApplicationFactory<Startup> _factory;

    public OdataHttpQueryTest(ErabliereApiApplicationFactory<Startup> factory)
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

        var response = await client.GetAsync(
            "/Erablieres?$expand=capteurs($filter=afficherCapteurDashboard eq true)");

        response.EnsureSuccessStatusCode();
    }
}
