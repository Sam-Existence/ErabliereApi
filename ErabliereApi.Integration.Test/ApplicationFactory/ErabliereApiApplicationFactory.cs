using Microsoft.AspNetCore.Mvc.Testing;

namespace ErabliereApi.Integration.Test.ApplicationFactory;

public class ErabliereApiApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{

}
