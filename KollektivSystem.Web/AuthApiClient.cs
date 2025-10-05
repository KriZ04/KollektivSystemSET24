namespace KollektivSystem.Web;

public class AuthApiClient(HttpClient httpClient)
{
    public async Task<bool> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var loginRequest = new LoginRequest(email, password);

        var response = await httpClient.PostAsJsonAsync("/auth/login", loginRequest, cancellationToken);

        // Return true if successful (HTTP 200 OK)
        return response.IsSuccessStatusCode;
    }
}

public record LoginRequest(string email, string password);
