using IdentityServer4.Models;
using System.Collections.Generic;

namespace ErabliereApi.IdentityServer;

public class Config
{
    public List<IdentityResource> Ids { get; set; }

    public List<ApiResource> Apis { get; set; }

    public List<ApiScope> Scopes { get; set; }

    public List<Client> Clients { get; set; }
}
