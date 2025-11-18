using System.Net.Http.Json;
using KollektivSystem.Web.Models;

namespace KollektivSystem.Web.Services;

public sealed class TicketTypeAdminClient
{
    private readonly HttpClient _http;
    private readonly AuthTokenService _authTokens;

    public TicketTypeAdminClient(HttpClient http, AuthTokenService authTokens)
    {
        _http = http;
        _authTokens = authTokens;
    }

    public async Task<IReadOnlyList<TicketTypeAdminDto>> GetAllAsync(CancellationToken ct = default)
    {
        var token = await _authTokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return Array.Empty<TicketTypeAdminDto>();

        using var req = new HttpRequestMessage(HttpMethod.Get, "/tickets-type/admin");
        req.Headers.Authorization = new("Bearer", token);

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
            return Array.Empty<TicketTypeAdminDto>();

        var list = await res.Content.ReadFromJsonAsync<List<TicketTypeAdminDto>>(cancellationToken: ct);
        return list ?? new List<TicketTypeAdminDto>();
    }

    public async Task<TicketTypeAdminDto?> CreateAsync(string name, float price, int aliveTimeMinutes, CancellationToken ct = default)
    {
        var token = await _authTokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return null;

        using var req = new HttpRequestMessage(HttpMethod.Post, "/tickets-type/admin");
        req.Headers.Authorization = new("Bearer", token);

        var body = new { name, price, aliveTimeMinutes };
        req.Content = JsonContent.Create(body);

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
            return null;

        return await res.Content.ReadFromJsonAsync<TicketTypeAdminDto>(cancellationToken: ct);
    }

    public async Task<bool> DeactivateAsync(int id, CancellationToken ct = default)
    {
        var token = await _authTokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return false;

        using var req = new HttpRequestMessage(HttpMethod.Delete, $"/tickets-type/admin/{id}");
        req.Headers.Authorization = new("Bearer", token);

        using var res = await _http.SendAsync(req, ct);
        return res.IsSuccessStatusCode;
    }
}
