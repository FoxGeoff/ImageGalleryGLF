using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mavreak.IDP
{
    public static class Config
    {
        //test users
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    Username = "Frank",         //required, OpenId() resource for identity token
                    Password = "password",

                    Claims = new List<Claim>    //optional, Profile() resources for identity token
                    {
                        new Claim("given_name", "Frank"),
                        new Claim("famliy_name", "Underwood")
                    }
                },
                new TestUser
                {
                    SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    Username = "Claire",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Claire"),
                        new Claim("famliy_name", "Underwood")
                    }
                }
            };
        }

        //identity-related resources (scopes)
        public static IEnumerable<IdentityResource> GetIdentityResoures()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), // required
                new IdentityResources.Profile()
            };
        }

        //TODO: API-related resouces (scopes)

        //TODO: Code stub - DONE
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientName = "Image Gallery",
                    ClientId = "imagegalleryclient",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44364/signin-oidc" 
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
            };
        }
    }
}
