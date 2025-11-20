using System.Net.Http.Json;
using KollektivSystem.Web.Models;

namespace KollektivSystem.Web.Services;

public sealed class TransitLineAdminClient
{
    private readonly HttpClient _http;
    private readonly AuthTokenService _authTokens;

    public TransitLineAdminClient(HttpClient http, AuthTokenService authTokens)
    {
        _http = http;
        _authTokens = authTokens;
    }

    public async Task<IReadOnlyList<TransitLineAdminDto>> GetAllAsync(CancellationToken ct = default)
    {
        var token = await _authTokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return Array.Empty<TransitLineAdminDto>();

        using var req = new HttpRequestMessage(HttpMethod.Get, "/transitlines");
        req.Headers.Authorization = new("Bearer", token);

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
            return Array.Empty<TransitLineAdminDto>();

        var list = await res.Content.ReadFromJsonAsync<List<TransitLineAdminDto>>(cancellationToken: ct);
        return list ?? new List<TransitLineAdminDto>();
    }

    public async Task<TransitLineAdminDto?> CreateAsync(int id, string name, CancellationToken ct = default)
    {
        var token = await _authTokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return null;

        using var req = new HttpRequestMessage(HttpMethod.Post, "/transitlines");
        req.Headers.Authorization = new("Bearer", token);

        var body = new { Id = id, Name = name };
        req.Content = JsonContent.Create(body);

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
            return null;

        return await res.Content.ReadFromJsonAsync<TransitLineAdminDto>(cancellationToken: ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var token = await _authTokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return false;

        using var req = new HttpRequestMessage(HttpMethod.Delete, $"/transitlines/{id}");
        req.Headers.Authorization = new("Bearer", token);

        using var res = await _http.SendAsync(req, ct);
        return res.IsSuccessStatusCode;
    }
}
