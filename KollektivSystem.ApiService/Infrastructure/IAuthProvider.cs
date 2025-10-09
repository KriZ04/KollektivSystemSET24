using KollektivSystem.ApiService.Models.Domain;
using KollektivSystem.ApiService.Models.Enums;
using System.Security.Claims;

namespace KollektivSystem.ApiService.Infrastructure
{
    public interface IAuthProvider
    {
        AuthProvider Provider { get; }
        AuthChallenge BuildAuthorizeRedirect(Uri callback, string[] scopes);
        Task<TokenResult> ExchangeCodeAsync(string code, string? codeVerifier, CancellationToken ct = default);
        ClaimsPrincipal ValidateAndReadIdToken(string idToken);
    }
}
