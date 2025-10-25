using KollektivSystem.ApiService.Models.Domain;
using KollektivSystem.ApiService.Models.Enums;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace KollektivSystem.ApiService.Infrastructure
{
    public class MockAuthProvider : IAuthProvider
    {
        private readonly string _issuer;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly SecurityKey _signingKey;

        public MockAuthProvider(string issuer, string clientId, string clientSecret, string signingKey)
        {
            _issuer = issuer;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }

        public AuthProvider Provider => AuthProvider.Mock;

        public AuthChallenge BuildAuthorizeRedirect(Uri callback, string[] scopes)
        {
            var state = Guid.NewGuid().ToString("N");

            // using Authority to preserve port
            var baseUrl = $"{callback.Scheme}://{callback.Authority}/oidc/authorize";

            var url = $"{baseUrl}"
                    + $"?client_id={Uri.EscapeDataString(_clientId)}"
                    + $"&redirect_uri={Uri.EscapeDataString(callback.ToString())}"
                    + $"&response_type=code"
                    + $"&scope={Uri.EscapeDataString(string.Join(' ', scopes))}"
                    + $"&state={state}";

            return new AuthChallenge(new Uri(url), state, null);
        }

        public Task<TokenResult> ExchangeCodeAsync(string code, string? codeVerifier, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal ValidateAndReadIdToken(string idToken)
        {
            throw new NotImplementedException();
        }
    }
}
