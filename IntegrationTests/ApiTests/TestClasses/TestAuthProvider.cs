using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models.Domain;
using KollektivSystem.ApiService.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KollektivSystem.IntegrationTests.ApiTests.TestClasses
{
    internal sealed class TestAuthProvider : IAuthProvider
    {
        public AuthProvider Provider => AuthProvider.Mock;

        public AuthChallenge BuildAuthorizeRedirect(Uri callback, string[] scopes)
        {
            var redirectUri = new Uri("https://oidc.test/authorize?state=test-state");
            return new AuthChallenge(redirectUri, "test-state", CodeVerifier: null);
        }

        public Task<TokenResult> ExchangeCodeAsync(string code, Uri? redirectUri, CancellationToken ct = default)
        {
            var tokens = new TokenResult(
                IdToken: "test-id-token",
                AccessToken: "test-access-token",
                RefreshToken: "test-refresh-token");

            return Task.FromResult(tokens);
        }

        public ClaimsPrincipal ValidateAndReadIdToken(string accessToken)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-sub"),
                new Claim(ClaimTypes.Email, "test@example.com"),
            }, 
            authenticationType: "test");

            return new ClaimsPrincipal(identity);
        }
    }
}
