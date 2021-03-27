// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace ErabliereApi.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address()
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("erabliereapi", "Access to erabliere api") { Scopes = { "erabliereapi" } }
            };

        public static IEnumerable<ApiScope> Scopes =>
            new ApiScope[]
            {
                new ApiScope("erabliereapi", "Access to erabliere api")
            };
        
        public static IEnumerable<Client> Clients =>
            new Client[] 
            { 
                new Client
                {
                    ClientName = "ErabliereAPI", 
                    ClientId = "erabliereapi",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:5001/api/oauth2-redirect.html"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:5005/signout-callback-oidc"
                    },
                    AllowedScopes = 
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "erabliereapi"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                },
                new Client
                {
                    ClientName = "ErabliereAPI - Swagger",
                    ClientId = "swagger-dev",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RedirectUris =
                    {
                        "https://localhost:5001/api/oauth2-redirect.html"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:5005/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "erabliereapi"
                    },
                    AllowedCorsOrigins =
                    {
                        "https://localhost:5001"
                    },
                    ClientSecrets =
                    {
                        new Secret("D6VDPBGKjmoyKkP~yFnxvMlLqS".Sha256())
                    }
                }
            };
    }
}