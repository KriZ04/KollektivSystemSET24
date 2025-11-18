using System.Net.Http.Json;
using KollektivSystem.Web.Models;

namespace KollektivSystem.Web.Services;

public sealed class TicketApiClient : ITicketApiClient
{
    private readonly HttpClient _http;
    private readonly AuthTokenService _tokens;

    public TicketApiClient(HttpClient http, AuthTokenService tokens)
    {
        _http = http;
        _tokens = tokens;
    }

    public async Task<IReadOnlyList<TicketDto>> GetTicketsAsync(CancellationToken ct = default)
    {
        var token = await _tokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return Array.Empty<TicketDto>();

        // Hent billettyper som admin/systemutvikler har laget:
        // GET /tickets-type (ikke admin-variant)
        using var req = new HttpRequestMessage(HttpMethod.Get, "/tickets-type");
        req.Headers.Authorization = new("Bearer", token);

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
            return Array.Empty<TicketDto>();

        var data = await res.Content.ReadFromJsonAsync<List<TicketDto>>(cancellationToken: ct);
        return data ?? [];
    }

    public async Task<PurchasedTicketDto?> PurchaseTicketAsync(int ticketTypeId, CancellationToken ct = default)
    {
        var token = await _tokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return null;

        // Backend forventer POST /tickets med body { ticketTypeId = ... }
        using var req = new HttpRequestMessage(HttpMethod.Post, "/tickets");
        req.Headers.Authorization = new("Bearer", token);

        var body = new { ticketTypeId };
        req.Content = JsonContent.Create(body);

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
            return null;

        return await res.Content.ReadFromJsonAsync<PurchasedTicketDto>(cancellationToken: ct);
    }

    public async Task<IReadOnlyList<PurchasedTicketDto>> GetMyTicketsAsync(CancellationToken ct = default)
    {
        var token = await _tokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            return Array.Empty<PurchasedTicketDto>();

        // Backend har /tickets/me, ikke /tickets/mine
        using var req = new HttpRequestMessage(HttpMethod.Get, "/tickets/me");
        req.Headers.Authorization = new("Bearer", token);

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
            return Array.Empty<PurchasedTicketDto>();

        var data = await res.Content.ReadFromJsonAsync<List<PurchasedTicketDto>>(cancellationToken: ct);
        return data ?? [];
    }
}
