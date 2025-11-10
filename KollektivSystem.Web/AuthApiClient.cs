using Microsoft.AspNetCore.Authorization;

namespace KollektivSystem.Web;

public record LoginRequest(string email, string password);
public record LoginResponse(string accessToken, string role, int expiresIn);

public class AuthApiClient(HttpClient httpClient) : IAuthApiClient
{
    public async Task<LoginResponse?> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var resp = await httpClient.PostAsJsonAsync("/auth/login", new LoginRequest(email, password), ct);
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: ct);
    }
}
