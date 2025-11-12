using Microsoft.JSInterop;

namespace KollektivSystem.Web.Services
{
    public sealed class ProfileClient(HttpClient http, IJSRuntime js)
    {
        public async Task<UserProfile?> GetMeAsync(CancellationToken ct = default)
        {
            var token = await js.InvokeAsync<string?>("localStorage.getItem", "api_jwt");
            if (string.IsNullOrWhiteSpace(token)) return null;

            using var req = new HttpRequestMessage(HttpMethod.Get, "/users/me");
            req.Headers.Authorization = new("Bearer", token);
            using var res = await http.SendAsync(req, ct);
            if (!res.IsSuccessStatusCode) return null;

            return await res.Content.ReadFromJsonAsync<UserProfile>(cancellationToken: ct);
        }
    }

    public sealed record UserProfile(Guid Id, string? Name, string? Email);
}
