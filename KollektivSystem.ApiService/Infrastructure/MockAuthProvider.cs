using KollektivSystem.ApiService.Models.Domain;
using KollektivSystem.ApiService.Models.Enums;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace KollektivSystem.ApiService.Infrastructure
{
    public sealed class MockAuthProvider : IAuthProvider
    {
        private readonly string _issuer;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly HttpClient _httpClient;
        private readonly SecurityKey _signingKey;

        public MockAuthProvider(string issuer, string clientId, string clientSecret, string signingKey, HttpClient httpClient)
        {
            _issuer = issuer;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _httpClient = httpClient;
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

            return new AuthChallenge(new Uri(url), state, CodeVerifier: null);
        }
        sealed class TokenResponse
        {
            public string? AccessToken { get; set; }
            public string? IdToken { get; set; }
            public string? TokenType { get; set; }
            public int? ExpiresIn { get; set; }
        }
        public async Task<TokenResult> ExchangeCodeAsync(string code, Uri? redirectUri, CancellationToken ct = default)
        {
            var tokenEndpoint = new Uri(new Uri($"{redirectUri!.Scheme}://{redirectUri.Authority}"), "/oidc/token");

            using var form = new FormUrlEncodedContent(new Dictionary<string, string?>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["client_id"] = _clientId,
                ["client_secret"] = _clientSecret,
                ["redirect_uri"] = redirectUri?.ToString() // Usually required
            }!);

            using var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint) { Content = form };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(request, ct);

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync(ct);
                throw new HttpRequestException($"Token endpoint {response.StatusCode}: {err}");
            }
            //response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: ct)
                       ?? throw new InvalidOperationException("Empty token response.");

            return new TokenResult(json.AccessToken!, json.IdToken!, null);
        }
        public ClaimsPrincipal ValidateAndReadIdToken(string idToken)
        {
            var handler = new JwtSecurityTokenHandler();

            var validation = new TokenValidationParameters
            {
                ValidIssuer = _issuer,
                ValidateIssuer = true,

                ValidAudience = _clientId,        // audience = client_id ok for stub
                ValidateAudience = true,

                IssuerSigningKey = _signingKey,  
                ValidateIssuerSigningKey = true,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2)
            };

            handler.ValidateToken(idToken, validation, out _);

            return handler.ValidateToken(idToken, validation, out _);
        }

        private SigningCredentials SigningCreds() => new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
    }

}
