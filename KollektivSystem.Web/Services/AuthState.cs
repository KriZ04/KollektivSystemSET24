using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KollektivSystem.Web.Services
{
    public sealed class AuthState
    {
        private readonly AuthTokenService _tokens;

        public AuthState(AuthTokenService tokens)
        {
            _tokens = tokens;
        }

        public async Task<string?> GetTokenAsync(CancellationToken ct = default)
            => await _tokens.GetValidAccessTokenAsync(ct);

        public async Task<bool> IsAuthenticatedAsync(CancellationToken ct = default)
            => !string.IsNullOrWhiteSpace(await _tokens.GetValidAccessTokenAsync(ct));

        public async Task<IDictionary<string, string>> GetClaimsAsync(CancellationToken ct = default)
        {
            var token = await _tokens.GetValidAccessTokenAsync(ct);
            if (string.IsNullOrWhiteSpace(token))
                return new Dictionary<string, string>();

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var dict = jwt.Claims
                .GroupBy(c => c.Type)
                .ToDictionary(g => g.Key, g => g.Last().Value);

            if (!dict.ContainsKey("name") && dict.TryGetValue(ClaimTypes.Name, out var n)) dict["name"] = n;
            if (!dict.ContainsKey("email") && dict.TryGetValue(ClaimTypes.Email, out var e)) dict["email"] = e;

            return dict;
        }

        public async Task LogoutAsync()
        {
            // her er det fint om AuthTokenService får en LogoutAsync() som bruker ITokenStore.ClearAsync()
            await _tokens.LogoutAsync();
        }
    }
}

