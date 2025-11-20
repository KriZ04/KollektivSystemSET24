using System.Net.Http.Json;
using KollektivSystem.Web.Models;

namespace KollektivSystem.Web.Services;

public sealed class TransitLineStopAdminClient
{
    private readonly HttpClient _http;
    private readonly AuthTokenService _authTokens;

    public TransitLineStopAdminClient(HttpClient http, AuthTokenService authTokens)
    {
        _http = http;
        _authTokens = authTokens;
    }

    public async Task<IReadOnlyList<TransitLineStopAdminDto>> GetAllAsync(CancellationToken ct = default)
    {
        var token = await _authTokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return Array.Empty<TransitLineStopAdminDto>();

        using var req = new HttpRequestMessage(HttpMethod.Get, "/transitlinestops");
        req.Headers.Authorization = new("Bearer", token);

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
            return Array.Empty<TransitLineStopAdminDto>();

        var list = await res.Content.ReadFromJsonAsync<List<TransitLineStopAdminDto>>(cancellationToken: ct);
        return list ?? new List<TransitLineStopAdminDto>();
    }

    public async Task<TransitLineStopAdminDto?> CreateAsync(int order, int transitLineId, int stopId, CancellationToken ct = default)
    {
        var token = await _authTokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return null;

        using var req = new HttpRequestMessage(HttpMethod.Post, "/transitlinestops");
        req.Headers.Authorization = new("Bearer", token);

        var body = new { Order = order, TransitLineId = transitLineId, StopId = stopId };
        req.Content = JsonContent.Create(body);

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
            return null;

        return await res.Content.ReadFromJsonAsync<TransitLineStopAdminDto>(cancellationToken: ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var token = await _authTokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return false;

        using var req = new HttpRequestMessage(HttpMethod.Delete, $"/transitlinestops/{id}");
        req.Headers.Authorization = new("Bearer", token);

        using var res = await _http.SendAsync(req, ct);
        return res.IsSuccessStatusCode;
    }
}
