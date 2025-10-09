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

        public AuthProvider Provider => throw new NotImplementedException();

        public AuthChallenge BuildAuthorizeRedirect(Uri callback, string[] scopes)
        {
            throw new NotImplementedException();
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
