using KollektivSystem.Web.Services;

public sealed class AuthApiClient(HttpClient http, ITokenStore tokens)
{
    public Uri BaseAddress => http.BaseAddress!;

    public async Task<HttpResponseMessage> GetProtectedAsync(string path, CancellationToken ct = default)
    {
        var token = await tokens.GetAsync();
        var req = new HttpRequestMessage(HttpMethod.Get, path);
        if (!string.IsNullOrEmpty(token))
            req.Headers.Authorization = new("Bearer", token);

        return await http.SendAsync(req, ct);
    }
}
