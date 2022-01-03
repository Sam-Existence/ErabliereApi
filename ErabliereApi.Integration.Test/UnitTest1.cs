using ErabliereApi.Integration.Test.ApplicationFactory;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ErabliereApi.Integration.Test
{
    public class UnitTest1 : IClassFixture<AzureADApplicationFactory<Startup>>
    {
        private readonly AzureADApplicationFactory<Startup> _factory;

        public UnitTest1(AzureADApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task UserCanLogin()
        {
            var client = _factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("https://erabliereapi.freddycoder.com"),
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            });

            var response = await client.GetAsync("/");

            response.EnsureSuccessStatusCode();


        }
    }
}