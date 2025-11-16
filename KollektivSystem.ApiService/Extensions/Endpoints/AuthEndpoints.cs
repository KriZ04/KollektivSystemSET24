using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace KollektivSystem.ApiService.Extensions.Endpoints;

public sealed class AuthEndpointsLoggerCategory { }

public static class AuthEndpoints
{
    private static readonly string[] DefaultScopes = ["openid", "email", "profile"];
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");

        group.MapGet("/login", HandleLogin);

        group.MapGet("/callback", HandleCallback);

        group.MapPost("/refresh", HandleRefresh);

        return app;
    }

    internal static IResult HandleLogin(HttpRequest req, IAuthProvider authProvider, IMemoryCache cache, ILogger<AuthEndpointsLoggerCategory> logger)
    {
        var returnUrl = req.Query["returnUrl"].ToString();
        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            logger.LoginMissingReturnUrl();
            return Results.BadRequest("missing returnUrl");

        }

        if (!IsValidReturnUrl(returnUrl, out _))
        {
            logger.LoginInvalidReturnUrl(returnUrl);
            return Results.BadRequest("invalid returnUrl");
        }

        var callback = new Uri($"{req.Scheme}://{req.Host}/auth/callback");
        var ch = authProvider.BuildAuthorizeRedirect(callback, DefaultScopes);
        var cacheKey = GetStateCacheKey(ch.State);
        cache.Set(cacheKey, returnUrl, TimeSpan.FromMinutes(5));

        logger.LoginRedirecting(ch.State, returnUrl);

        var authorizeUrl = ch.RedirectUri.ToString();
        return Results.Redirect(authorizeUrl);
    }

    internal static async Task<IResult> HandleCallback(string? code, string? state, HttpRequest req, IMemoryCache cache, IAuthProvider provider, IAuthService auth, ILogger<AuthEndpointsLoggerCategory> logger, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(state))
        {
            logger.CallbackMissingCodeOrState();
            return Results.BadRequest("Missing 'code' or 'state'.");
        }

        var cacheKey = GetStateCacheKey(state);

        if (!cache.TryGetValue<string>(cacheKey, out var returnUrl))
        {
            logger.CallbackInvalidState(state);
            return Results.BadRequest("Invalid state.");
        }

        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            logger.CallbackMissingReturnUrl();
            return Results.BadRequest("Missing return url.");
        }
        
        if (!IsValidReturnUrl(returnUrl, out _))
        {
            logger.CallbackInvalidReturnUrl(state, returnUrl);
            return Results.BadRequest("invalid returnUrl");
        }

        cache.Remove(cacheKey);

        var redirectUri = new Uri($"{req.Scheme}://{req.Host}/auth/callback");

        logger.CallbackExchangingCode(state, provider.Provider.ToString());
        var tokens = await provider.ExchangeCodeAsync(code, redirectUri, ct);

        var principal = provider.ValidateAndReadIdToken(tokens.IdToken);

        var (user, apiJwt, refreshToken) = await auth.SignInWithIdTokenAsync(provider.Provider, principal, ct);
        logger.CallbackUserSignedIn(user.Id, provider.Provider.ToString(), user.Sub);

        var sep = returnUrl.Contains('?') ? "&" : "?";
        var target = $"{returnUrl}{sep}token={Uri.EscapeDataString(apiJwt)}&refresh={Uri.EscapeDataString(refreshToken)}";

        logger.CallbackRedirecting(returnUrl);

        return Results.Redirect(target, permanent: false);
    }

    internal static async Task<IResult> HandleRefresh(RefreshRequest req, ITokenService tokenService, ILogger<AuthEndpointsLoggerCategory> logger, CancellationToken ct)
    {
        var (success, access, refresh) = await tokenService.RefreshAsync(req.RefreshToken, ct);

        if (!success)
        {
            logger.RefreshFailed();
            return Results.Unauthorized();
        }

        logger.RefreshSucceeded();
        return Results.Ok(new
        {
            access_token = access,
            refresh_token = refresh
        });
    }

    private static bool IsValidReturnUrl(string returnUrl, out Uri? uri)
    {
        if (!Uri.TryCreate(returnUrl, UriKind.Absolute, out var ru))
        {
            uri = null;
            return false;
        }

        if (ru.Scheme != Uri.UriSchemeHttps && ru.Scheme != Uri.UriSchemeHttp)
        {
            uri = null;
            return false;
        }

        uri = ru;
        return true;
    }

    private static string GetStateCacheKey(string state) => $"oidc_state:{state}";

    public sealed record RefreshRequest(string RefreshToken);
}
