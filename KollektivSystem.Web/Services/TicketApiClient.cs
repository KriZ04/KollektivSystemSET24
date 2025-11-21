using System.Net.Http.Json;
using System.Text.Json;
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
            throw new InvalidOperationException("Bruker er ikke logget inn.");

        using var req = new HttpRequestMessage(HttpMethod.Get, "/tickets-type");
        req.Headers.Authorization = new("Bearer", token);

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
        {
            var body = await res.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException(
                $"GET /tickets-type feilet med status {(int)res.StatusCode} ({res.StatusCode}). Body: {body}");
        }

        var data = await res.Content.ReadFromJsonAsync<List<TicketDto>>(cancellationToken: ct);
        return data ?? [];
    }

    public async Task<PurchasedTicketDto?> PurchaseTicketAsync(int ticketTypeId, CancellationToken ct = default)
    {
        var token = await _tokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException("Bruker er ikke logget inn.");

        using var req = new HttpRequestMessage(HttpMethod.Post, "/tickets");
        req.Headers.Authorization = new("Bearer", token);

        var bodyObject = new { TicketTypeId = ticketTypeId };
        req.Content = JsonContent.Create(bodyObject);

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
        {
            var body = await res.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException(
                $"POST /tickets feilet med status {(int)res.StatusCode} ({res.StatusCode}). Body: {body}");
        }

        var dto = await res.Content.ReadFromJsonAsync<PurchasedTicketDto>(cancellationToken: ct);
        return dto;
    }

    public async Task<IReadOnlyList<PurchasedTicketDto>> GetMyTicketsAsync(CancellationToken ct = default)
    {
        var token = await _tokens.GetValidAccessTokenAsync(ct);
        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException("Bruker er ikke logget inn.");

        // inkluder også utløpte/ugyldige billetter så vi iallfall ser noe
        using var req = new HttpRequestMessage(HttpMethod.Get, "/tickets/me?includeInvalid=true");
        req.Headers.Authorization = new("Bearer", token);

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
        {
            var body = await res.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException(
                $"GET /tickets/me feilet med status {(int)res.StatusCode} ({res.StatusCode}). Body: {body}");
        }

        var data = await res.Content.ReadFromJsonAsync<List<PurchasedTicketDto>>(cancellationToken: ct);
        return data ?? [];
    }
}
