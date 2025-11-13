using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models.Enums;
using KollektivSystem.ApiService.Services;
using Microsoft.Extensions.Caching.Memory;

namespace KollektivSystem.ApiService.Extensions.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");

        group.MapGet("/login", HandleLogin);

        group.MapGet("/callback", async (string? code, string? state, HttpRequest req, IMemoryCache cache, IAuthProvider provider, IAuthService auth, CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(state))
                return Results.BadRequest("Missing 'code' or 'state'.");

            if (!cache.TryGetValue<string>($"oidc_state:{state}", out var returnUrl) || string.IsNullOrWhiteSpace(returnUrl))
                return Results.BadRequest("Invalid state.");

            cache.Remove($"oidc_state:{state}");

            var redirectUri = new Uri($"{req.Scheme}://{req.Host}/auth/callback");

            var tokens = await provider.ExchangeCodeAsync(code, redirectUri, ct);

            var principal = provider.ValidateAndReadIdToken(tokens.IdToken);

            var (user, apiJwt, refreshToken) = await auth.SignInWithIdTokenAsync(provider.Provider, principal, ct);


            if (!Uri.TryCreate(returnUrl, UriKind.Absolute, out var ru) ||
                (ru.Scheme != Uri.UriSchemeHttps && ru.Scheme != Uri.UriSchemeHttp))
                return Results.BadRequest("invalid returnUrl");

            var sep = returnUrl.Contains('?') ? "&" : "?";
            var target = $"{returnUrl}{sep}token={Uri.EscapeDataString(apiJwt)}&refresh={Uri.EscapeDataString(refreshToken)}";

            return Results.Redirect(target, permanent: false);
        });

        group.MapPost("/refresh", async (RefreshRequest req, ITokenService tokenService, CancellationToken ct) =>
        {
            var (success, access, refresh) = await tokenService.RefreshAsync(req.RefreshToken, ct);

            if (!success)
                return Results.Unauthorized();

            return Results.Ok(new
            {
                access_token = access,
                refresh_token = refresh
            });
        });



        return app;
    }

    internal static IResult HandleLogin(HttpRequest req, IAuthProvider authProvider, IMemoryCache cache)
    {
        var returnUrl = req.Query["returnUrl"].ToString();
        if (string.IsNullOrWhiteSpace(returnUrl))
            return Results.BadRequest("missing returnUrl");

        if (!Uri.TryCreate(returnUrl, UriKind.Absolute, out var ru) ||
            (ru.Scheme != Uri.UriSchemeHttps && ru.Scheme != Uri.UriSchemeHttp))
            return Results.BadRequest("invalid returnUrl");

        var callback = new Uri($"{req.Scheme}://{req.Host}/auth/callback");
        var ch = authProvider.BuildAuthorizeRedirect(callback, ["openid", "email", "profile"]);
        cache.Set($"oidc_state:{ch.State}", returnUrl, TimeSpan.FromMinutes(5));

        var authorizeUrl = ch.RedirectUri.ToString();
        return Results.Redirect(authorizeUrl);
    }


    public sealed record RefreshRequest(string RefreshToken);
}
