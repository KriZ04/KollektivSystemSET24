using KollektivSystem.Web.Models;
using Microsoft.JSInterop;

namespace KollektivSystem.Web.Services;

public sealed class ProfileClient
{
    private readonly HttpClient _http;

    public ProfileClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<(UserMeDto? user, bool notFoundOrUnauthorized)> GetMeAsync(string token, CancellationToken ct = default)
    {
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
