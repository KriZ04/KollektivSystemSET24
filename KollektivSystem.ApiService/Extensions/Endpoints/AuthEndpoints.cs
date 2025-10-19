using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace KollektivSystem.ApiService.Extensions.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/auth");

            group.MapGet("/login", (HttpRequest req, IAuthProvider authProvider, IMemoryCache cache) =>
            {
                var callback = new Uri($"{req.Scheme}://{req.Host}/auth/callback");
                var ch = authProvider.BuildAuthorizeRedirect(callback, ["openid", "email", "profile"]);
                cache.Set($"oidc_state:{ch.State}", true, TimeSpan.FromMinutes(5));
                return Results.Redirect(ch.RedirectUri.ToString());
            });

            group.MapGet("/callback", async (
            string code, string state,
            IMemoryCache cache, IAuthProvider provider, IAuthService auth, CancellationToken ct) =>
            {
                if (!cache.TryGetValue($"oidc_state:{state}", out _)) return Results.BadRequest("Invalid state");

                var tokens = await provider.ExchangeCodeAsync(code, null, ct);
                var principal = provider.ValidateAndReadIdToken(tokens.IdToken);

                var (user, apiJwt) = await auth.SignInWithIdTokenAsync(provider.Provider, principal, ct);
                return Results.Ok(new { token = apiJwt, user = new { user.Id, user.DisplayName, role = user.Role.ToString(), user.Email } });
            });


            return app;
        }
    }
}
