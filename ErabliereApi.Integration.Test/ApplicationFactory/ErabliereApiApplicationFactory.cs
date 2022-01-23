using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace ErabliereApi.Integration.Test.ApplicationFactory;

public class ErabliereApiApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    
}
