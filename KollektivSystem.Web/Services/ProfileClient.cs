using KollektivSystem.Web.Models;
using Microsoft.JSInterop;

namespace KollektivSystem.Web.Services;

public sealed class ProfileClient
{
    private readonly HttpClient _http;
    private readonly AuthTokenService _authTokens;

    public ProfileClient(HttpClient http, AuthTokenService authTokens)
    {
        _http = http;
        _authTokens = authTokens;
    }

    public async Task<(UserMeDto? user, bool notFoundOrUnauthorized)> GetMeAsync(CancellationToken ct = default)
    {
        var token = await _authTokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return (null, true);

        using var req = new HttpRequestMessage(HttpMethod.Get, "/users/me");
        req.Headers.Authorization = new("Bearer", token);

        using var res = await _http.SendAsync(req, ct);

        if (res.StatusCode is System.Net.HttpStatusCode.NotFound
            or System.Net.HttpStatusCode.Unauthorized
            or System.Net.HttpStatusCode.Forbidden)
        {
            return (user: null, true); // brukeren finnes ikke/token ugyldig
        }

        if (!res.IsSuccessStatusCode)
            return (user: null, false); // annen feil, men ikke nødvendigvis logg ut

        var dto = await res.Content.ReadFromJsonAsync<UserMeDto>(cancellationToken: ct);
        return (dto, false);
    }
}
