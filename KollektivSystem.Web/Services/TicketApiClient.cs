using System.Net.Http.Json;
using KollektivSystem.Web.Models;

namespace KollektivSystem.Web.Services;

public sealed class TicketApiClient : ITicketApiClient
{
    private readonly HttpClient _http;

    public TicketApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<IReadOnlyList<TicketDto>> GetTicketsAsync(CancellationToken ct = default)
    {
        var response = await _http.GetAsync("tickets", ct);
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<List<TicketDto>>(cancellationToken: ct);
        return data ?? [];
    }

    public async Task<PurchasedTicketDto?> PurchaseTicketAsync(int ticketId, CancellationToken ct = default)
    {
        var response = await _http.PostAsync($"tickets/{ticketId}/purchase", null, ct);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<PurchasedTicketDto>(cancellationToken: ct);
    }
}
