using System.Net.Http.Json;
using KollektivSystem.Web.Models;

namespace KollektivSystem.Web.Services;

public sealed class UsersAdminClient
{
    private readonly HttpClient _http;
    private readonly AuthTokenService _authTokens;

    public UsersAdminClient(HttpClient http, AuthTokenService authTokens)
    {
        _http = http;
        _authTokens = authTokens;
    }

    public async Task<IReadOnlyList<AdminUserDto>> GetUsersAsync(CancellationToken ct = default)
    {
        var token = await _authTokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return Array.Empty<AdminUserDto>();

        using var req = new HttpRequestMessage(HttpMethod.Get, "/users");
        req.Headers.Authorization = new("Bearer", token);

        using var res = await _http.SendAsync(req, ct);

        if (!res.IsSuccessStatusCode)
        {
            // TODO: logge feilkode hvis du vil
            return Array.Empty<AdminUserDto>();
        }

        var list = await res.Content.ReadFromJsonAsync<List<AdminUserDto>>(cancellationToken: ct);
        return list ?? new List<AdminUserDto>();
    }

    public async Task<bool> UpdateRoleAsync(Guid userId, string newRole, CancellationToken ct = default)
    {
        var token = await _authTokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return false;

        using var req = new HttpRequestMessage(HttpMethod.Put, $"/users/{userId}/role");
        req.Headers.Authorization = new("Bearer", token);

        // Matcher UpdateUserRoleRequest(string Role) → { "role": "Admin" }
        var body = new { role = newRole };
        req.Content = JsonContent.Create(body);

        using var res = await _http.SendAsync(req, ct);
        return res.IsSuccessStatusCode;
    }
}
