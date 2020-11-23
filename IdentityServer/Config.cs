using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    internal class Resources
    {
        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    DisplayName = "Role",
                    UserClaims = new List<string> { "role" }
                }
            };
        }

        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                // TODO: For managing this IdentityServer
                // http://docs.identityserver.io/en/latest/topics/add_apis.html
                //new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
                new ApiResource
                {
                    Name = "test.api",
                    DisplayName = "Test API",
                    Description = "Allow the application to access Test API on your behalf",
                    ApiSecrets = new List<Secret>
                    {
                        new Secret("ScopeSecret".Sha256()) // change me!
                    },
                    Scopes = new List<string>
                    {
                        "test.api.read",
                        "test.api.write"
                    },
                    UserClaims = new List<string> { "role" }
                }
            };
        }

        public static List<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("test.api.read", "Read Access to Test API"),
                new ApiScope("test.api.write", "Write Access to Test API")
            };
        }
    }

    internal class Clients
    {
        public static List<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "oauth",
                    ClientName = "Test client using client credentials",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("SuperSecretPassword".Sha256()) // change me!
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string> { "test.api.read" }
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC test client using the authorization code flow with Proof-Key for Code Exchange (PKCE)",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("SuperSecretPassword".Sha256()) // change me!
                    },
                    RedirectUris = new List<string> {
                        "https://localhost:5002/signin-oidc",
                        "https://mvc.test.sso.steffbeckers.eu/signin-oidc"
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "test.api.read"
                    },
                    RequirePkce = true,
                    AllowPlainTextPkce = false
                },
                new Client
                {
                    ClientId = "angular",
                    ClientName = "Angular test client using the authorization code flow with Proof-Key for Code Exchange (PKCE)",
                    RequireClientSecret = false,
                    RedirectUris = new List<string> {
                        "http://localhost:4200",
                        "https://angular.test.sso.steffbeckers.eu"
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowOfflineAccess = true,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "test.api.read"
                    },
                    RequirePkce = true,
                    AllowPlainTextPkce = false,
                    AllowedCorsOrigins = new List<string> {
                        "http://localhost:4200",
                        "https://angular.test.sso.steffbeckers.eu"
                    },
                }
            };
        }
    }

    internal class TestUsers
    {
        public static List<TestUser> Get()
        {
            return new List<TestUser>
            {
                new TestUser {
                    SubjectId = "DCC99315-C7D7-42A9-8099-D94E52DD15A5",
                    Username = "steff",
                    Password = "Password123!", // change me!
                    Claims = new List<Claim> {
                        new Claim(JwtClaimTypes.Email, "steff@steffbeckers.eu"),
                        new Claim(JwtClaimTypes.Role, "admin")
                    }
                },
                new TestUser {
                    SubjectId = "0409F062-85BE-4B6F-BA42-B700A3CD3E65",
                    Username = "alice",
                    Password = "Password123!", // change me!
                    Claims = new List<Claim> {
                        new Claim(JwtClaimTypes.Email, "alice@alice.com"),
                    }
                },
                new TestUser {
                    SubjectId = "7DDE665A-5CF3-4A1C-90FB-0EBF71666D99",
                    Username = "bob",
                    Password = "Password123!", // change me!
                    Claims = new List<Claim> {
                        new Claim(JwtClaimTypes.Email, "bob@bob.org"),
                    }
                }
            };
        }
    }
}
