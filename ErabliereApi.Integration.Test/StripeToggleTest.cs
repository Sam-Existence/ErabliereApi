using ErabliereApi.Integration.Test.ApplicationFactory;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ErabliereApi.Integration.Test;

//public class StripeToggleTest : IClassFixture<StripeEnabledApplicationFactory<Startup>>
//{
//    private readonly StripeEnabledApplicationFactory<Startup> _factory;

//    public StripeToggleTest(StripeEnabledApplicationFactory<Startup> factory)
//    {
//        _factory = factory;
//    }

//    [Fact]
//    public async Task ByDefault_ThereIsNoCheckoutControllerEnabled()
//    {
//        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
//        {
//            AllowAutoRedirect = true,
//            HandleCookies = true,
//            MaxAutomaticRedirections = 7
//        });

//        var response = await client.GetAsync("/Checkout");

//        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//    }
//}
