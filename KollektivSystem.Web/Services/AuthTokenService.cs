using System.IdentityModel.Tokens.Jwt;

namespace KollektivSystem.Web.Services;

public sealed class AuthTokenService
{
    private readonly ITokenStore _tokens;
    private readonly HttpClient _http;
    private static readonly TimeSpan RefreshSkew = TimeSpan.FromMinutes(1);

    private sealed record RefreshResponse(string access_token, string refresh_token);

    public AuthTokenService(ITokenStore tokens, HttpClient http)
    {
        _tokens = tokens;
        _http = http;
    }

    public async Task<string?> GetValidAccessTokenAsync(CancellationToken ct = default)
    {
        var access = await _tokens.GetAsync();
        if (string.IsNullOrWhiteSpace(access))
            return null;

        if (!IsExpiredOrNearExpiry(access))
            return access;

        var refresh = await _tokens.GetRefreshAsync();
        if (string.IsNullOrWhiteSpace(refresh))
        {
            await _tokens.ClearAsync();
            return null;
        }

        var refreshed = await TryRefreshAsync(refresh, ct);
        if (refreshed is null)
        {
            await _tokens.ClearAsync();
            return null;
        }

        await _tokens.SetAsync(refreshed.Value.Access);
        await _tokens.SetRefreshAsync(refreshed.Value.Refresh);

        return refreshed.Value.Access;
    }

    public async Task LogoutAsync()
    {
        await _tokens.ClearAsync();
    }

    private static bool IsExpiredOrNearExpiry(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken token;

        try
        {
            token = handler.ReadJwtToken(jwt);
        }
        catch
        {
            return true;
        }

        var now = DateTime.UtcNow;
        var exp = token.ValidTo;

        return exp <= now.Add(RefreshSkew);
    }

    private async Task<(string Access, string Refresh)?> TryRefreshAsync(string refreshToken, CancellationToken ct)
    {
        var body = new { RefreshToken = refreshToken };

        using var res = await _http.PostAsJsonAsync("/auth/refresh", body, ct);
        if (!res.IsSuccessStatusCode)
            return null;

        var payload = await res.Content.ReadFromJsonAsync<RefreshResponse>(cancellationToken: ct);
        if (payload is null ||
            string.IsNullOrWhiteSpace(payload.access_token) ||
            string.IsNullOrWhiteSpace(payload.refresh_token))
            return null;

        return (payload.access_token, payload.refresh_token);
    }
}
