using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KollektivSystem.Web.Services
{
    public sealed class AuthState
    {
        private readonly ITokenStore _tokens;

        public AuthState(ITokenStore tokens)
        {
            _tokens = tokens;
        }

        public async Task<string?> GetTokenAsync() => await _tokens.GetAsync();

        public async Task<bool> IsAuthenticatedAsync() => string.IsNullOrWhiteSpace(await _tokens.GetAsync());

        public async Task<IDictionary<string, string>> GetClaimsAsync()
        {
            var token = await _tokens.GetAsync();
            if(string.IsNullOrWhiteSpace(token)) return new Dictionary<string, string>();

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var dict = jwt.Claims.GroupBy(c => c.Type).ToDictionary(g => g.Key, g => g.Last().Value);

            if (!dict.ContainsKey("name") && dict.TryGetValue(ClaimTypes.Name, out var n)) dict["name"] = n;
            if (!dict.ContainsKey("email") && dict.TryGetValue(ClaimTypes.Email, out var e)) dict["email"] = e;

            return dict;
        }

        public async Task LogoutAsync() => await _tokens.ClearAsync();

    }
}
